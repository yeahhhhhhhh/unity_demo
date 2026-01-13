using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class HallUI: UIBase
{
    //[Header("设置界面组件")]
    public TextMeshProUGUI uid_text_;
    public TextMeshProUGUI nickname_text_;
    public TextMeshProUGUI gold_text_;
    public TextMeshProUGUI diamond_text_;
    public TextMeshProUGUI exp_text_;
    public Button enter_scene_btn_;

    void Start()
    {
        ShowPlayerInfo();

        NetManager.AddMsgListener((short)MsgRespPbType.PRE_ENTER_DEFAULT_SCENE, OnPreEnterDefaultScene);
        enter_scene_btn_.onClick.AddListener(OnPreEnterSceneClick);
    }

    public void OnPreEnterSceneClick()
    {
        MsgPreEnterScene msg = new();
        NetManager.Send(msg);
    }
    public void ShowPlayerInfo()
    {
        uid_text_.text = MainPlayer.GetUid().ToString();
        nickname_text_.text = MainPlayer.player_.base_info_.nickname;
        gold_text_.text = MainPlayer.player_.base_info_.gold.ToString();
        diamond_text_.text = MainPlayer.player_.base_info_.diamond.ToString();
        exp_text_.text = MainPlayer.player_.base_info_.exp.ToString();
    }

    public void OnPreEnterDefaultScene(MsgBase msg)
    {
        UIManager.Instance.CloseUI("Hall");
        UIManager.Instance.OpenUI("Loading");
        MsgPreEnterScene.Response resp_msg = (MsgPreEnterScene.Response)msg;
        attributes.scene.SceneInfo scene_info = resp_msg.resp.SceneInfo;
        SceneMgr.Init(scene_info.SceneId, scene_info.SceneGid);
        // 切换场景
        SceneTransitionManager.Instance.LoadScene("FightScene");
    }
}
