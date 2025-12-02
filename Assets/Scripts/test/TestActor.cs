using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestActor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = new GameObject("myActor");
        BaseActor baseTank = obj.AddComponent<BaseActor>();
        baseTank.Init("RobotPrefab");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
