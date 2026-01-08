// 全局设置界面
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : UIBase
{
    [Header("设置界面组件")]
    //public Slider volumeSlider;
    //public Toggle fullscreenToggle;
    public Button loginButton;
    public Button visitorLoginBtn;
    public Button visitorRegisterBtn;
    public Button registerBtn;

    public Button testLogin1;
    public Button testLogin2;
    //public Button closeButton;


    void Start()
    {
        // 初始化组件事件
        loginButton.onClick.AddListener(OnLoginClick);
        visitorRegisterBtn.onClick.AddListener(OnVisitorRegisterClick);
        visitorLoginBtn.onClick.AddListener(OnVisitorLoginClick);

        testLogin1.onClick.AddListener(OnTestLogin1Click);
        testLogin2.onClick.AddListener(OnTestLogin2Click);
        //closeButton.onClick.AddListener(OnCloseClick);
        //volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    public void OnVisitorLoginClick()
    {
        Debug.Log("OnVisitorLoginClick");
        NetManager.RemoveMsgListener((short)MsgRespPbType.LOGIN_RESPONSE, OnLoginResp);
        NetManager.AddMsgListener((short)MsgRespPbType.LOGIN_RESPONSE, OnLoginResp);
        MsgLogin login_msg = new();
        login_msg.SetVisitorData(GameMain.Instance.device_id);
        NetManager.Send(login_msg);
    }

    public void OnLoginClick()
    {
        Debug.Log("OnLoginClick");
    }

    public void OnVisitorRegisterClick()
    {
        Debug.Log("OnVisitorRegisterClick");
        // 获取设备id
        string device_id = GameMain.Instance.device_id;
        MsgRegister msg = new();
        msg.SetVisitorData(device_id);
        NetManager.Send(msg);
    }

    public void OnTestLogin1Click()
    {
        NetManager.RemoveMsgListener((short)MsgRespPbType.LOGIN_RESPONSE, OnLoginResp);
        NetManager.AddMsgListener((short)MsgRespPbType.LOGIN_RESPONSE, OnLoginResp);
        Debug.Log("OnTestLogin1Click");
        MsgLogin login_msg = new();
        login_msg.SetSendData("why", "123");
        NetManager.Send(login_msg);
    }

    public void OnTestLogin2Click()
    {
        NetManager.RemoveMsgListener((short)MsgRespPbType.LOGIN_RESPONSE, OnLoginResp);
        NetManager.AddMsgListener((short)MsgRespPbType.LOGIN_RESPONSE, OnLoginResp);
        Debug.Log("OnTestLogin2Click");
        MsgLogin login_msg = new();
        login_msg.SetSendData("why2", "123");
        NetManager.Send(login_msg);
    }

    public override void OnOpen()
    {
        base.OnOpen();
        // 初始化界面数据
        //volumeSlider.value = AudioListener.volume;
        //fullscreenToggle.isOn = Screen.fullScreen;
    }

    //void OnCloseClick()
    //{
    //    UIManager.Instance.CloseUI("GlobalSettings");
    //}

    //void OnVolumeChanged(float value)
    //{
    //    AudioListener.volume = value;
    //}

    public void OnLoginResp(MsgBase msg)
    {
        Debug.Log("OnLoginResp");
        MsgLogin.Response resp_msg = (MsgLogin.Response)msg;
        Debug.Log("errorcode:" + resp_msg.resp.ErrorCode + " uid:" + resp_msg.resp.PlayerBaseInfo.Uid + " username:" + resp_msg.resp.PlayerBaseInfo.Username);

        PlayerBaseInfo player_base_info = new();
        player_base_info.Copy(resp_msg.resp.PlayerBaseInfo);
        MainPlayer.SetPlayerBaseInfo(player_base_info);

        NetManager.RemoveMsgListener((short)MsgRespPbType.LOGIN_RESPONSE, OnLoginResp);

        if (resp_msg.resp.ErrorCode == 0)
        {
            UIManager.Instance.CloseUI("Login");
            SceneMgr.LoadScene("HallScene");
        }
    }
}