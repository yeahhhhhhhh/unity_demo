using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletActor : BaseActor
{
    public float max_life_time_ = 5;
    private float life_time = 0;
    public GameObject owner_;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        life_time = 0;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        life_time += Time.deltaTime;
        if (life_time > max_life_time_)
        {
            Destroy(gameObject);
            if (owner_ != null && !owner_.IsDestroyed())
            {
                MainPlayerActor actor = owner_.GetComponent<MainPlayerActor>();
                if (actor)
                {
                    actor.NotifiedBulletDestroy();
                }
            }
        }
    }

    public void Fire(GameObject owner)
    {
        owner_ = owner;
        Rigidbody rb = transform.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed_;
    }
}
