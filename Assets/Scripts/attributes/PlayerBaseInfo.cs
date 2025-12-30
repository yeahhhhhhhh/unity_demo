using System;
using System.Collections.Generic;

public class PlayerBaseInfo
{
    public Int64 uid = 0;
    public string username = null;
    public string nickname = null;
    public Int64 exp = 0;
    public Int64 gold = 0;
    public Int64 diamond = 0;

    public void Copy(attributes.player.PlayerBaseInfo data)
    {
        uid = data.Uid;
        username = data.Username;
        nickname = data.Nickname;
        exp = data.Exp;
        gold = data.Gold;
        diamond = data.Diamond;
    }
}
