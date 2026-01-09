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
        NetManager.AddMsgListener((short)MsgRespPbType.ENTER_DEFAULT_SCENE_RESPONSE, OnEnterDefaultScene);
        MsgEnterScene enter_scene_msg = new();
        NetManager.Send(enter_scene_msg);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnterDefaultScene(MsgBase msg)
    {
        MsgEnterScene.Response resp_msg = (MsgEnterScene.Response)msg;
        attributes.scene.EntitySceneInfo entity_scene_info = resp_msg.resp.EntitySceneInfo;
        attributes.scene.SceneInfo scene_info = resp_msg.resp.SceneInfo;
        Debug.Log("OnEnterScene, error_code:" + resp_msg.resp.ErrorCode + " uid:" + entity_scene_info.Id);
        Int64 uid = entity_scene_info.Id;
        Int32 scene_id = scene_info.SceneId;
        Int32 scene_gid = scene_info.SceneGid;

        EntitySimpleInfo entity = new();
        entity.Copy(entity_scene_info);
        MainPlayer.SetPlayerEntity(entity);
        SceneMgr.Init(scene_id, scene_gid);

        // 生成主玩家
        // 进入场景，生成模型
        entity = SceneMgr.CreateEntity(entity);
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
}
