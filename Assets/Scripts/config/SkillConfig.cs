using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public static class SkillConfig
{
    public static Dictionary<Int32, SkillBaseInfo> skill_id2data_ = new();
    public static void Init()
    {
        //SkillBaseInfo skill = new()
        //{
        //    skill_id_ = 1,
        //    skill_name_ = "SkillQ",
        //    max_life_time_ = 2f,
        //    skill_prefab_ = ResManager.LoadPrefab("Prefabs/AoE effects/Crystals crossfade"),
        //    radius_ = 1,
        //    range_x_ = 0.5f,
        //    range_y_ = 1f,
        //    range_z_ = 0.5f,
        //};

        //SkillBaseInfo skill3 = new()
        //{
        //    skill_id_ = 3,
        //    skill_name_ = "Skill3",
        //    max_life_time_ = 2f,
        //    skill_prefab_ = ResManager.LoadPrefab("Prefabs/AoE effects/Crystals crossfade"),
        //    radius_ = 1,
        //    range_x_ = 0.5f,
        //    range_y_ = 1f,
        //    range_z_ = 0.5f,
        //};


        List<SkillBaseInfo> skill_list = new()
        {
            new(){
                skill_id_ = 1,
                skill_name_ = "aoe矩形瞬时攻击技能",
                max_life_time_ = 2f,
                skill_prefab_ = ResManager.LoadPrefab("Prefabs/AoE effects/Crystals crossfade"),
                radius_ = 1,
                range_x_ = 0.5f,
                range_y_ = 1f,
                range_z_ = 0.5f,
                speed_ = 0,
            },
            new(){
                skill_id_ = 2,
                skill_name_ = "aoe子弹攻击技能",
                max_life_time_ = 5f,
                skill_prefab_ = ResManager.LoadPrefab("Prefabs/AoE effects/Snow AOE"),
                radius_ = 1,
                range_x_ = 0.5f,
                range_y_ = 1f,
                range_z_ = 0.5f,
                speed_ = 2f,
            },
            new(){
                skill_id_ = 3,
                skill_name_ = "aoe圆形瞬时攻击技能",
                max_life_time_ = 1f,
                skill_prefab_ = ResManager.LoadPrefab("Prefabs/Hits and explosions/Explosion"),
                radius_ = 1,
                range_x_ = 0.5f,
                range_y_ = 1f,
                range_z_ = 0.5f,
                speed_ = 0,
            },
            new(){
                skill_id_ = 4,
                skill_name_ = "aoe圆形持续治疗技能",
                max_life_time_ = 4f,
                skill_prefab_ = ResManager.LoadPrefab("Prefabs/Magic circles/Healing circle"),
                radius_ = 1,
                range_x_ = 0.5f,
                range_y_ = 1f,
                range_z_ = 0.5f,
                speed_ = 0,
            },
            new(){
                skill_id_ = 5,
                skill_name_ = "aoe圆形持续攻击技能",
                max_life_time_ = 5f,
                skill_prefab_ = ResManager.LoadPrefab("Prefabs/AoE effects/Laser AOE"),
                radius_ = 1,
                range_x_ = 0.5f,
                range_y_ = 1f,
                range_z_ = 0.5f,
                speed_ = 0,
            },
            new(){
                skill_id_ = 6,
                skill_name_ = "aoe矩形瞬时蓄力攻击技能",
                max_life_time_ = 2f,
                skill_prefab_ = ResManager.LoadPrefab("Prefabs/Slash effects/Charge slash blue"),
                radius_ = 1,
                range_x_ = 0.5f,
                range_y_ = 1f,
                range_z_ = 0.5f,
                speed_ = 0,
            },
        };

        for (int i = 0; i < skill_list.Count; ++i)
        {
            skill_id2data_[skill_list[i].skill_id_] = skill_list[i];
        }
    }

    public static SkillBaseInfo GetSkillInfo(Int32 skill_id)
    {
        if (skill_id2data_.ContainsKey(skill_id))
        {
            return skill_id2data_[skill_id];
        }

        return null;
    }
}
