using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerActor : CtrlActor
{
    public Transform body_;
    public Transform mouth_;
    public Transform fire_point_;

    public GameObject bullet_prefab_;
    public int max_bullet_count_ = 5;
    public int cur_bullet_count_ = 0;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        body_ = transform.Find("Body");
        mouth_ = transform.Find("Mouth"); 
        fire_point_ = transform.Find("FirePoint");
        if (body_ == null)
        {
            Debug.Log("body is null");
        }
        if (mouth_ == null)
        {
            Debug.Log("mouth is null");
        }
        if (fire_point_ == null)
        {
            Debug.Log("fire_point is null");
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        Fire();
    }

    public void Fire()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (cur_bullet_count_ >= max_bullet_count_)
            {
                Debug.Log("已发射最大数量bullet:" + max_bullet_count_.ToString());
                return;
            }
            Debug.Log("fire!!!!");

            GameObject bullet = Instantiate(bullet_prefab_, fire_point_.position, fire_point_.rotation);
            Debug.Log("fire position:" + fire_point_.position.ToString());
            BulletActor bullet_actor = bullet.GetComponent<BulletActor>();
            bullet_actor.Fire(gameObject);
            cur_bullet_count_++;
        }
    }

    public void NotifiedBulletDestroy()
    {
        cur_bullet_count_--;
    }
}
