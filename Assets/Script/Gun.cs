using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public bool isActive = false;
    Vector2 direction;
    public int powerUpLevelRequirement = 0;
    public Bullet bullet;

    public bool autoshoot = false;
    public float shootIntervalSeconds = 0.5f;
    public float shootDelayseconds = 0.0f;
    public float shootTimer = 0f;
    public float delayTimer = 0f;



    // Start is called before the first frame update
    void Start()
    {
        direction = (transform.localRotation * Vector2.right).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            return;
        }

        direction = (transform.localRotation * Vector2.right).normalized;
        if (autoshoot)
        {
            if (delayTimer >= shootDelayseconds)
            {
                if (shootTimer >= shootIntervalSeconds)
                {
                    Shoot();
                    shootTimer = 0;
                }
                else
                {
                    shootTimer += Time.deltaTime;
                }
            }
            else
            {
                delayTimer += Time.deltaTime;
            }
        }
    }

    public void Shoot()
    {
        GameObject go = Instantiate(bullet.gameObject, transform.position, Quaternion.identity);
        Bullet goBullet = go.GetComponent<Bullet>();
        goBullet.direction = direction;
    }
}
