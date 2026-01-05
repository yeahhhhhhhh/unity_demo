using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class App { 
    public static void Init()
    {

    }
}


public static class MainPlayer
{
    public static PlayerInfo player_ = new();
    public static bool is_dead_ = false;

    public static void SetPlayerBaseInfo(PlayerBaseInfo player_base_info)
    {
        player_.base_info_ = player_base_info;
    }

    public static void SetPlayerSceneInfo(Int32 scene_id, Int32 scene_gid, Vector3 pos)
    {
        player_.scene_info_.scene_id_ = scene_id;
        player_.scene_info_.scene_gid_ = scene_gid;
        player_.scene_info_.pos_ = pos;
    }

    public static void SetPlayerFightInfo(attributes.combat.FightInfo fight_info)
    {
        player_.fight_info_.Copy(fight_info);
    }

    public static void SetPlayerEntity(EntitySimpleInfo entity)
    {
        player_.entity_.entity_info_ = entity;
    }

    public static bool IsValid()
    {
        return !IsDead() && GetUid() > 0;
    }

    public static Int64 GetUid() {  return player_.base_info_.uid; }
    public static Int64 GetGlobalID() { return player_.entity_.entity_info_.global_id_; }

    public static bool IsDead() { return is_dead_; }
    public static void SetDead(bool dead) { is_dead_ = dead; }

    public static EntitySimpleInfo GetEntity()
    {
        return player_.entity_.entity_info_;
    }
}

public class GameMain : MonoBehaviour
{
    public static GameObject main_ctl_canvas_;
    public static GameObject reborn_btn_;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("main start");
        MsgTypeRegister.InitPbRegister();
        MoveManager.Instance.Init();

        SkillConfig.Init();
        NpcConfig.Init();
        SkillManager.Instance.Init();
        FightManager.Instance.Init();

        main_ctl_canvas_ = GameObject.Find("CtlText");
        reborn_btn_ = GameObject.Find("RebornBtn");
        reborn_btn_.SetActive(false);

        // ≤‚ ‘ResManager
        //GameObject skinRes = ResManager.LoadPrefab("RobotPrefab");
        //GameObject skin = (GameObject)Instantiate(skinRes);
    }

    // Update is called once per frame
    void Update()
    {
        NetManager.Update();
    }

    public static void SetMainCanvasActive(bool active)
    {
        main_ctl_canvas_.SetActive(active);
    }

    public static void SetRebornBtnActive(bool active)
    {
        reborn_btn_.SetActive(active);
    }
}
