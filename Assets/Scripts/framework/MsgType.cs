using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;


public enum MsgType{
    None = 0,
    Move = 1,
    Login = 2,
}

public enum MsgPbType
{
    LOGIN = 1,
    LOGOUT = 2,
    REGIST = 3,
    GET_PLAYER_BASE_INFO = 4,
    ENTER_DEFAULT_SCENE = 5,
    MOVE = 6,
    USE_SKILL = 7,
    MOVE_UPDATE_POS = 8,
    GET_SCENE_PLAYERS = 9,
}

public enum MsgRespPbType
{
    COMMON = 1,
    LOGIN_RESPONSE = 2,
    PLAYER_BASE_INFO = 3,
    REGIST_RESPONSE = 4,
    ENTER_DEFAULT_SCENE_RESPONSE = 5,
    MOVE = 6,
    USE_SKILL_RESPONSE = 7,
    MOVE_UPDATE_POS = 8,
    GET_SCENE_PLAYERS = 9,
}

public static class MsgTypeRegister
{
    static Dictionary<short, string> msg_id2string_ = new();

    static Dictionary<short, Type> msg_req_pb_id2msg_pb_type_ = new();
    static Dictionary<short, Type> msg_req_pb_id2msg_class_type_ = new();

    static Dictionary<short, Type> msg_resp_pb_id2msg_pb_type_ = new();
    static Dictionary<short, Type> msg_resp_pb_id2msg_class_type_ = new();


    static public void InitRegister()
    {
        Debug.Log("InitRegister");
    }

    static public void InitPbRegister()
    {
        Debug.Log("InitPbRegister");
        RegisterRequestPbMsg((short)MsgPbType.LOGIN, "service.account.LoginRequest", "MsgLogin");
        RegisterRequestPbMsg((short)MsgPbType.ENTER_DEFAULT_SCENE, "service.scene.RequestEnterDefaultScene", "MsgEnterScene");
        RegisterRequestPbMsg((short)MsgPbType.GET_SCENE_PLAYERS, "service.scene.RequestGetScenePlayers", "MsgGetScenePlayers");

        RegisterResponsePbMsg((short)MsgRespPbType.LOGIN_RESPONSE, "service.account.LoginRequest+Response", "MsgLogin+Response");
        RegisterResponsePbMsg((short)MsgRespPbType.ENTER_DEFAULT_SCENE_RESPONSE, "service.scene.RequestEnterDefaultScene+Response", "MsgEnterScene+Response");
        RegisterResponsePbMsg((short)MsgRespPbType.GET_SCENE_PLAYERS, "service.scene.RequestGetScenePlayers+Response", "MsgGetScenePlayers+Response");
    }

    static public string GetStringTypeByID(short id)
    {
        if (msg_id2string_.ContainsKey(id))
        {
            return msg_id2string_[id];
        }

        return null;
    }

    static public bool RegisterMsg(short cmd_id, string type)
    {
        if (msg_id2string_.ContainsKey(cmd_id))
        {
            Debug.Log("cmd id already register:" + cmd_id.ToString());
            return false;
        }

        msg_id2string_[cmd_id] = type;
        return true;
    }

    static public bool RegisterRequestPbMsg(short cmd_id, string type, string class_type)
    {
        if (msg_req_pb_id2msg_pb_type_.ContainsKey(cmd_id))
        {
            Debug.Log("pb cmd id already register:" + cmd_id.ToString());
            return false;
        }

        Type t = Type.GetType(type);
        if (t == null)
        {
            Debug.Log("RegisterRequestPbMsg type is null");
            return false;
        }

        if (msg_req_pb_id2msg_class_type_.ContainsKey(cmd_id))
        {
            Debug.Log("pb cmd id already register:" + cmd_id.ToString());
            return false;
        }

        Type ct = Type.GetType(class_type);
        if (t == null)
        {
            Debug.Log("RegisterRequestPbMsg class_type is null");
            return false;
        }

        msg_req_pb_id2msg_pb_type_[cmd_id] = t;
        msg_req_pb_id2msg_class_type_[cmd_id] = ct;
        return true;
    }

    static public bool RegisterResponsePbMsg(short cmd_id, string type, string class_type)
    {
        if (msg_resp_pb_id2msg_pb_type_.ContainsKey(cmd_id))
        {
            Debug.Log("pb cmd id already register:" + cmd_id.ToString());
            return false;
        }

        Type t = Type.GetType(type);
        if (t == null)
        {
            Debug.Log("RegisterResponsePbMsg type is null");
            return false;
        }

        if (msg_resp_pb_id2msg_class_type_.ContainsKey(cmd_id))
        {
            Debug.Log("pb cmd id already register:" + cmd_id.ToString());
            return false;
        }

        Type ct = Type.GetType(class_type);
        if (t == null)
        {
            Debug.Log("RegisterResponsePbMsg class_type is null");
            return false;
        }

        msg_resp_pb_id2msg_pb_type_[cmd_id] = t;
        msg_resp_pb_id2msg_class_type_[cmd_id] = ct;
        return true;
    }
    

    static public Type GetMsgClassTypeByID(short id)
    {
        if (msg_req_pb_id2msg_class_type_.ContainsKey(id))
        {
            return msg_req_pb_id2msg_class_type_[id];
        }

        return null;
    }

    static public Type GetRespMsgClassTypeByID(short id)
    {
        if (msg_resp_pb_id2msg_class_type_.ContainsKey(id))
        {
            return msg_resp_pb_id2msg_class_type_[id];
        }

        return null;
    }

    static public Type GetPbTypeByID(short id)
    {
        if (msg_req_pb_id2msg_pb_type_.ContainsKey(id))
        {
            return msg_req_pb_id2msg_pb_type_[id];
        }

        return null;
    }

    static public Type GetRespPbTypeByID(short id)
    {
        if (msg_resp_pb_id2msg_pb_type_.ContainsKey(id))
        {
            return msg_resp_pb_id2msg_pb_type_[id];
        }

        return null;
    }
}