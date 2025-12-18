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
    public static FightInfo fight_info_ = new();

    public static void SetPlayerBaseInfo(Int64 uid, string username, string nickname)
    {
        player_.base_info_.uid = uid;
        player_.base_info_.username = username;
        player_.base_info_.nickname = nickname;
    }

    public static void SetPlayerSceneInfo(Int32 scene_id, Int32 scene_gid, Vector3 pos)
    {
        player_.scene_info_.scene_id_ = scene_id;
        player_.scene_info_.scene_gid_ = scene_gid;
        player_.scene_info_.pos_ = pos;
    }

    public static void SetPlayerFightInfo(attributes.combat.FightInfo fight_info)
    {
        fight_info_.Copy(fight_info);
    }

    public static Int64 GetUid() {  return player_.base_info_.uid; }
}

public class GameMain : MonoBehaviour
{
    public static GameObject main_ctl_canvas_;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("main start");
        MsgTypeRegister.InitPbRegister();
        MoveManager.Instance.Init();

        SkillConfig.Init();
        SkillManager.Instance.Init();
        FightManager.Instance.Init();

        main_ctl_canvas_ = GameObject.Find("CtlText");

        // ≤‚ ‘ResManager
        //GameObject skinRes = ResManager.LoadPrefab("RobotPrefab");
        //GameObject skin = (GameObject)Instantiate(skinRes);
    }

    // Update is called once per frame
    void Update()
    {
        NetManager.Update();
    }

    public static void SetMainCanvasActive(bool active)
    {
        main_ctl_canvas_.SetActive(active);
    }
}
