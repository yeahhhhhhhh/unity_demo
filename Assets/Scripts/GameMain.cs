using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class App { 
    public static void Init()
    {

    }
}


public static class MainPlayer
{
    public static PlayerInfo player_ = new();
    public static bool is_dead_ = false;

    public static void SetPlayerBaseInfo(PlayerBaseInfo player_base_info)
    {
        player_.base_info_ = player_base_info;
    }

    public static void SetPlayerSceneInfo(Int32 scene_id, Int32 scene_gid, Vector3 pos)
    {
        player_.scene_info_.scene_id_ = scene_id;
        player_.scene_info_.scene_gid_ = scene_gid;
        player_.scene_info_.pos_ = pos;
    }

    public static void SetPlayerFightInfo(attributes.combat.FightInfo fight_info)
    {
        player_.fight_info_.Copy(fight_info);
    }

    public static void SetPlayerEntity(EntitySimpleInfo entity)
    {
        player_.entity_.entity_info_ = entity;
    }

    public static bool IsValid()
    {
        return !IsDead() && GetUid() > 0;
    }

    public static Int64 GetUid() {  return player_.base_info_.uid; }
    public static Int64 GetGlobalID() { return player_.entity_.entity_info_.global_id_; }

    public static bool IsDead() { return is_dead_; }
    public static void SetDead(bool dead) { is_dead_ = dead; }

    public static EntitySimpleInfo GetEntity()
    {
        return player_.entity_.entity_info_;
    }
}

public class GameMain : MonoBehaviour
{
    private static GameMain _instance;
    public static GameMain Instance => _instance;

    public string ip_ = "172.20.131.148";
    public int port_ = 8002;
    public string device_id;
    public float connect_check_interval_ = 5f;

    void Awake()
    {
        device_id = SystemInfo.deviceUniqueIdentifier;
        Debug.Log("device_id:" + device_id);
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("main start");
        MsgTypeRegister.InitPbRegister();
        MoveManager.Instance.Init();

        SkillConfig.Init();
        NpcConfig.Init();
        SkillManager.Instance.Init();
        FightManager.Instance.Init();

        UIManager.Instance.OpenUI("Login");

        NetManager.AddEventListener(NetManager.NetEvent.ConnectSucc, OnConnectSucc);
        NetManager.AddEventListener(NetManager.NetEvent.ConnectFail, OnConnectFail);
        NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);

        NetManager.Connect(ip_, port_);
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
        SceneMgr.Init(0, 0);
        // 重置玩家场景数据
        MainPlayer.player_.scene_info_ = new();
        CameraFollower actor = Camera.main.GetComponent<CameraFollower>();
        if (actor != null)
        {
            Destroy(actor);
        }
    }

    public void ConnectCheck()
    {
        connect_check_interval_ -= Time.deltaTime;
        if (connect_check_interval_ <= 0f)
        {
            connect_check_interval_ = 5f;
            if (!NetManager.IsConnected())
            {
                NetManager.Connect(ip_, port_);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 5s尝试连接一次
        ConnectCheck();
        NetManager.Update();
    }
}
