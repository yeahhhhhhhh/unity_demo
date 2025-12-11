using System;
using System.Collections.Generic;

public static class SkillConfig
{
    public static Dictionary<Int32, SkillBaseInfo> skill_id2data_ = new();
    public static void Init()
    {
        SkillBaseInfo skill = new()
        {
            skill_id_ = 1,
            skill_name_ = "SkillQ",
            max_life_time_ = 3f,
            skill_prefab_ = ResManager.LoadPrefab("Skill1Prefab"),
            radius_ = 1,
            range_x_ = 3,
            range_y_ = 0.2f,
            range_z_ = 3,
        };

        skill_id2data_[skill.skill_id_] = skill;
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
