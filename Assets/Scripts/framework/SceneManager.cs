using attributes.scene;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class SceneManager
{
    public static Int32 scene_id_ = 0;
    public static Int32 scene_gid_ = 0;
    public static bool is_in_scene_ = false;
    
    public static Dictionary<Int64, EntitySimpleInfo> scene_entities = new();
    public static Dictionary<Int64, NpcInfo> scene_npcs = new();

    public static void Init(Int32 scene_id, Int32 scene_gid)
    {
        foreach (var item in scene_entities)
        {
            EntitySimpleInfo info = item.Value;
            if(info.skin_ != null && !info.skin_.IsDestroyed())
            {
                GameObject.Destroy(info.skin_);
            }
        }
        scene_entities.Clear();
        scene_id_ = scene_id;
        scene_gid_ = scene_gid;

        if (scene_id == 0)
        {
            is_in_scene_ = false;
        }
        else
        {
            is_in_scene_ = true;
        }
    }

    public static void AddEntity(EntitySimpleInfo entity)
    {
        Int64 global_id = entity.global_id_;
        if (scene_entities.ContainsKey(global_id))
        {
            return;
        }

        scene_entities[global_id] = entity;
    }

    public static void AddNpc(NpcInfo npc)
    {
        Int64 npc_gid = npc.npc_gid_;
        if (scene_npcs.ContainsKey(npc_gid))
        {
            return;
        }

        scene_npcs[npc_gid] = npc;
    }

    public static void RemoveNpc(Int64 npc_gid)
    {
        scene_npcs.Remove(npc_gid);
    }

    public static void RemoveEntity(Int64 global_id)
    {
        scene_entities.Remove(global_id);
    }

    public static NpcInfo FindNpc(Int64 npc_gid)
    {
        if (scene_npcs.ContainsKey(npc_gid))
        {
            return scene_npcs[npc_gid];
        }

        return null;
    }

    public static EntitySimpleInfo FindEntity(Int64 global_id)
    {
        if (scene_entities.ContainsKey(global_id))
        {
            return scene_entities[global_id];
        }

        return null;
    }

    public static NpcInfo CreateNpc(Vector3 pos, Vector3 rotation, Int32 npc_id, Int64 npc_gid)
    {
        if (SceneManager.FindNpc(npc_gid) != null)
        {
            Debug.Log("already npc, npc_gid:" + npc_gid);
            return null;
        }

        GameObject prefab = ResManager.LoadPrefab("NpcPrefab");
        if (prefab == null)
        {
            Debug.Log("NpcPrefab is null");
            return null;
        }

        NpcInfo npc = new();
        npc.CopyNpcConfig(npc_id);
        Debug.Log("create player, npc_gid:" + npc_gid.ToString() + ", x:" + pos.x + " y:" + pos.y + " z:" + pos.z);
        GameObject instance = UnityEngine.GameObject.Instantiate(prefab, pos, Quaternion.Euler(rotation));
        if (instance == null)
        {
            return null;
        }

        instance.name = npc.name_;
        npc.skin_ = instance;
        npc.npc_gid_ = npc_gid;
        SceneManager.AddNpc(npc);

        return npc;
    }

    public static GameObject GetEntityPrefab(Int32 type, Int64 id)
    {
        GameObject prefab = null;
        switch (type)
        {
            case (Int32)EntityTypes.PLAYER:
                {
                    prefab = ResManager.LoadPrefab("PlayerPrefab");
                    break;
                }
            case (Int32)EntityTypes.NPC:
                {
                    // TODO: 根据id选择不同prefab
                    prefab = ResManager.LoadPrefab("NpcPrefab");
                    break;
                }
            case (Int32)EntityTypes.FIGHT:
                {
                    SkillBaseInfo skill = SkillConfig.GetSkillInfo((Int32)id);
                    if (skill != null)
                    {
                        return skill.skill_prefab_;
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
        return prefab;
    }

    public static EntitySimpleInfo CreateEntity(EntitySimpleInfo entity)
    {
        Int64 global_id = entity.global_id_;
        if (SceneManager.FindEntity(global_id) != null)
        {
            Debug.Log("already_player, global_id:" + global_id);
            return null;
        }

        // 根据类型和id选中prefab
        GameObject prefab = GetEntityPrefab(entity.type_, entity.id_);
        if (prefab == null)
        {
            Debug.Log("PlayerPrefab is null， type:" + entity.type_ + " id:" + entity.id_);
            return null;
        }

        Vector3 pos = entity.position_;
        Vector3 rotation = MoveManager.GetRotaionByDirection(entity.direction_);
        GameObject instance = UnityEngine.GameObject.Instantiate(prefab, pos, Quaternion.Euler(rotation));
        entity.skin_ = instance;
        instance.name = "entity" + global_id.ToString();
        
        // 挂上同步脚本
        if (entity.type_ == (Int32)EntityTypes.PLAYER)
        {
            instance.AddComponent<SyncPlayerActor>();
            instance.SetActive(true);
        }else if (entity.type_ == (Int32)EntityTypes.NPC)
        {

        }else if(entity.type_ == (Int32) EntityTypes.FIGHT)
        {
            ActiveSkillActor actor = instance.AddComponent<ActiveSkillActor>();
            actor.Init((Int32)entity.id_, global_id, pos, rotation);
            instance.SetActive(true);
        }

        // 显示昵称UI
        HUDManager hud_manager = instance.transform.GetComponentInChildren<HUDManager>();
        if (hud_manager != null)
        {
            hud_manager.UpdateNickname(entity.nickname_);
            hud_manager.UpdateHealth(entity.cur_hp_, entity.max_hp_);
        }

        SceneManager.AddEntity(entity);

        return entity;
    }

    public static void DeleteEntity(Int64 global_id)
    {
        EntitySimpleInfo entity = FindEntity(global_id);
        if (entity != null)
        {
            RemoveEntity(global_id);
            GameObject.Destroy(entity.skin_);
        }
    }

    public static void DeleteNpc(Int64 npc_gid)
    {
        NpcInfo npc = SceneManager.FindNpc(npc_gid);
        if (npc != null)
        {
            SceneManager.RemoveNpc(npc_gid);
            GameObject.Destroy(npc.skin_);
        }
    }

}
