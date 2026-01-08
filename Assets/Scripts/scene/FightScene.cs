using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class FightScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 生成主玩家
        // 进入场景，生成模型
        EntitySimpleInfo entity = MainPlayer.GetEntity();
        entity = SceneMgr.CreateEntity(entity);
        Int64 uid = entity.id_;
        if (entity != null)
        {
            // 第一次进入获取战斗信息
            MsgGetFightInfo get_fight_info_msg = new();
            get_fight_info_msg.SetSendData(uid);
            NetManager.Send(get_fight_info_msg);

            // 设置摄像头
            CameraFollower follower = Camera.main.AddComponent<CameraFollower>();
            follower.Init(entity.skin_.transform, new Vector3()
            {
                x = 0,
                y = 12,
                z = -10
            });

            entity.skin_.name = "MainPlayer" + uid.ToString();
            // 挂上控制脚本
            entity.skin_.AddComponent<MainPlayerActor>();

            MainPlayer.SetPlayerEntity(entity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
