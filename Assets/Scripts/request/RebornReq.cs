using System;
using UnityEngine;

public class RebornReq : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickReborn()
    {
        if (MainPlayer.IsDead())
        {
            MsgReborn msg = new();
            msg.SetSendData(1);
            NetManager.Send(msg);
        }
    }

}
