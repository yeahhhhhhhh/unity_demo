using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBaseActor : MonoBehaviour
{
    public float max_life_time_ = 2f;
    public float cur_life_time_ = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cur_life_time_ += Time.deltaTime;
        if (cur_life_time_ > max_life_time_)
        {
            Destroy(gameObject);
        }
    }
}
