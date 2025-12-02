using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("main start");
        MsgTypeRegister.InitPbRegister();

        // ≤‚ ‘ResManager
        //GameObject skinRes = ResManager.LoadPrefab("RobotPrefab");
        //GameObject skin = (GameObject)Instantiate(skinRes);
    }

    // Update is called once per frame
    void Update()
    {
        NetManager.Update();
    }
}
