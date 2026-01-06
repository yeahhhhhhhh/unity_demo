using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseReq : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCloseClick()
    {
        Debug.Log("click close");
        NetManager.Close();
        SceneMgr.Init(0, 0);
        GameMain.SetMainCanvasActive(true);
        CameraFollower actor = Camera.main.GetComponent<CameraFollower>();
        if (actor != null)
        {
            Destroy(actor);
        }
    }
}
