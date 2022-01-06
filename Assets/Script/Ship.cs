using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{

    Gun[] guns;

    EnemyGun[] enemyguns;

    float moveSpeed = 8;

    bool moveUp;
    bool moveDown;
    bool moveLeft;
    bool moveRight;
    bool shoot;

    GameObject shield;



    public int powerUpGunLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        shield = transform.Find("Shield").gameObject;
        guns = transform.GetComponentsInChildren<Gun>();
        enemyguns = transform.GetComponentsInChildren<EnemyGun>();
        foreach (Gun gun in guns)
        {
            gun.isActive = true;
            if (gun.powerUpLevelRequirement != 0)
            {
                gun.gameObject.SetActive(false);
            }
        }


        foreach (EnemyGun enemygun in enemyguns)
        {
            enemygun.isActive = true;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveUp = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        moveDown = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
        moveLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        moveRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);

        shoot = Input.GetKeyDown(KeyCode.Space);
        if (shoot)
        {
            shoot = false;
            foreach(Gun gun in guns)
            {
                if (gun.powerUpLevelRequirement == powerUpGunLevel) { 
                    gun.Shoot();
                }
                
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;

        float moveAmount = moveSpeed * Time.fixedDeltaTime;
        Vector2 move = Vector2.zero;

        if (moveUp)
        {
            move.y += moveAmount;
        }

        if (moveDown)
        {
            move.y -= moveAmount;
        }

        if (moveLeft)
        {
            move.x -= moveAmount;
        }

        if (moveRight)
        {
            move.x += moveAmount;
        }

        float moveMagnitude = Mathf.Sqrt(move.x * move.x + move.y * move.y);

        if (moveMagnitude > moveAmount)
        {
            float ratio = moveAmount / moveMagnitude;
            move *= ratio;
        }

        pos += move;

        if(pos.x <= -9f)
        {
            pos.x = -9f;
        }

        if (pos.x >= 7.2f)
        {
            pos.x = 7.2f;
        }

        if (pos.y <= -4.4f)
        {
            pos.y = -4.4f;
        }

        if (pos.y >= 4.4f)
        {
            pos.y = 4.4f;
        }
        transform.position = pos;

    }


    void ActivateShield()
    {
        shield.SetActive(true);
    }

    void DeactivateShield()
    {
        shield.SetActive(false);
    }

    bool HasShield()
    {
        return shield.activeSelf;
    }

    void AddGuns()
    {
        powerUpGunLevel++;

        foreach (Gun gun in guns)
        {
            
            if (gun.powerUpLevelRequirement == powerUpGunLevel)
            {
                gun.gameObject.SetActive(true);
            }
        }
        
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.GetComponent<Bullet>();
        if(bullet != null)
        {
            if (bullet.isEnemy)
            {

                if (HasShield())
                {
                    DeactivateShield();
                }
                else
                {
                    Destroy(gameObject);
                    GameManager.Instance.Life--;
                }
                Destroy(bullet.gameObject);
            }

        }

        Hit hit = collision.GetComponent<Hit>();
        if (hit != null)
        {
            if (HasShield())
            {
                DeactivateShield();
            }
            else
            {
                Destroy(gameObject);
                GameManager.Instance.Life--;
            }
            Destroy(hit.gameObject);
        }

        PowerUp powerUp = collision.GetComponent<PowerUp>();
        if (powerUp)
        {
            if (powerUp.activateShield)
            {
                ActivateShield();
            }
            if (powerUp.addGuns)
            {
                AddGuns();
            }
            Destroy(powerUp.gameObject);
        }
    }

}
