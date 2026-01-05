using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;


public enum EntityTypes
{
    BEGIN = 0,
    PLAYER = 1,  // 玩家
    NPC = 2,     // 怪物
    FOLLOWER = 3,  // 跟随物
    BLOCK = 4,   // 阻挡物
    PROP = 5,    // 道具
    FIGHT = 6,   // 攻击物，可由技能发出
    END = 7
}

public class EntitySimpleInfo
{
    public Int32 type_ = 0;
    public Int64 id_ = 0;
    public Int64 global_id_ = 0;
    public String nickname_ = null;
    public Vector3 position_;
    public Int32 direction_;
    public Int32 max_hp_ = 0;
    public Int32 cur_hp_ = 0;

    public GameObject skin_;

    public void Copy(attributes.scene.EntitySceneInfo data)
    {
        type_ = data.Type;
        id_ = data.Id;
        global_id_ = data.GlobalId;
        nickname_ = data.Nickname;
        position_ = new()
        {
            x = data.Position.X,
            y = data.Position.Y,
            z = data.Position.Z
        };
        direction_ = data.Position.Direction;
        max_hp_ = data.MaxHp;
        cur_hp_ = data.CurHp;
    }
}

public class PlayerEntitySimpleInfo
{
    public Int64 uid_ = 0;
    public EntitySimpleInfo entity_info_ = new();

    public void Copy(attributes.scene.EntitySceneInfo data)
    {
        uid_ = data.Id;
        entity_info_.Copy(data);
    }
}

public class FightEntitySimpleInfo
{
    public EntitySimpleInfo entity_info_ = new();
    public void Copy(attributes.scene.EntitySceneInfo data)
    {
        entity_info_.Copy(data);
    }
}
