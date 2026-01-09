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

        NetManager.AddMsgListener((short)MsgRespPbType.PRE_ENTER_DEFAULT_SCENE, OnPreEnterDefaultScene);
        enter_scene_btn_.onClick.AddListener(OnPreEnterSceneClick);
    }

    public void OnPreEnterSceneClick()
    {
        MsgPreEnterScene msg = new();
        NetManager.Send(msg);
    }
    public void ShowPlayerInfo(string player_info)
    {
        player_info_text_.text = player_info;
    }

    public void OnPreEnterDefaultScene(MsgBase msg)
    {
        UIManager.Instance.CloseUI("Hall");
        UIManager.Instance.OpenUI("Loading");
        // 切换场景
        SceneTransitionManager.Instance.LoadScene("FightScene");
    }
}
