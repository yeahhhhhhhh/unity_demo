using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncPlayerActor : SyncActor
{
    public Transform body_;
    public Transform mouth_;
    public Transform fire_point_;

    public GameObject bullet_prefab_;
    public override void Start()
    {
        base.Start();
        Debug.Log("SyncPlayerActor start");
        base.Start();
        body_ = transform.Find("Body");
        mouth_ = transform.Find("Mouth");
        fire_point_ = transform.Find("FirePoint");
        bullet_prefab_ = ResManager.LoadPrefab("BulletPrefab");
        if (bullet_prefab_ == null)
        {
            Debug.Log("bullet_prefab_ is null");
        }
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

    public override void Update()
    {
        base.Update();
    }

    public void Fire()
    {
        Debug.Log("fire!!!!");
        GameObject bullet = Instantiate(bullet_prefab_, fire_point_.position, fire_point_.rotation);
        Debug.Log("fire position:" + fire_point_.position.ToString());
        BulletActor bullet_actor = bullet.GetComponent<BulletActor>();
        bullet_actor.Fire(null);
    }
}
