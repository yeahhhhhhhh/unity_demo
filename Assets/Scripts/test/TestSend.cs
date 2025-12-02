using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSend : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSendClick()
    {
        Debug.Log("click send");
        MsgLogin msgLogin = new MsgLogin();
        msgLogin.SetSendData("why", "123");
        NetManager.Send(msgLogin);
    }
}
