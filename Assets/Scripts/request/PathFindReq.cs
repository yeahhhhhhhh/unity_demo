using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindReq : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickPathFind()
    {
        MsgPathFind msg = new MsgPathFind();
        msg.SetSendData(2, 3);
        NetManager.Send(msg);
    }
}
