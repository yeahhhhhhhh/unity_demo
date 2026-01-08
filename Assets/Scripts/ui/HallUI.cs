using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class HallUI: UIBase
{
    //[Header("设置界面组件")]
    public TextMeshProUGUI player_info_text_;
    public Button enter_scene_btn_;

    void Start()
    {
        ShowPlayerInfo(MainPlayer.GetUid().ToString());

        NetManager.AddMsgListener((short)MsgRespPbType.ENTER_DEFAULT_SCENE_RESPONSE, OnEnterDefaultScene);
        enter_scene_btn_.onClick.AddListener(OnEnterSceneClick);
    }

    public void OnEnterSceneClick()
    {
        Debug.Log("OnEnterSceneClick");
        MsgEnterScene enter_scene_msg = new();
        NetManager.Send(enter_scene_msg);
    }
    public void ShowPlayerInfo(string player_info)
    {
        player_info_text_.text = player_info;
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

        UIManager.Instance.CloseUI("Hall");
        // 切换场景
        SceneMgr.LoadScene("FightScene");
    }
}
