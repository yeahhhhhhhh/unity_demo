using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectReq : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetManager.AddEventListener(NetManager.NetEvent.ConnectSucc, OnConnectSucc);
        NetManager.AddEventListener(NetManager.NetEvent.ConnectFail, OnConnectFail);
        NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 玩家点击连接按钮
    public void OnConnectClick()
    {
        Debug.Log("click connect btn");
        NetManager.Connect("172.20.131.148", 8002);
        //TODO:开始转圈圈,提示“连接中”
    }

    // 连接成功回调
    void OnConnectSucc(string err)
    {
        Debug.Log("OnConnectSucc");
        //TODO:进入游戏
    }
    // 连接失败回调
    void OnConnectFail(string err)
    {
        Debug.Log("OnConnectFail" + err);
        //TODO:弹出提示框(连接失败,请重试)
    }

    // 关闭连接
    void OnConnectClose(string err)
    {
        Debug.Log("OnConnectClose");
        //TODO:弹出提示框(网络断开)
        //TODO:弹出按钮(重新连接)
        SceneManager.Init(0, 0);
        // 重置玩家场景数据
        MainPlayer.player_.scene_info_ = new();
        GameMain.SetMainCanvasActive(true);
        CameraFollower actor = Camera.main.GetComponent<CameraFollower>();
        if (actor != null)
        {
            Destroy(actor);
        }
    }
}
