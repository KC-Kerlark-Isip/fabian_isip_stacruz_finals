using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{

    bool canBeDestroyed = false;
    public int points = 10;
    public int hits = 1;

    public Material hitMaterial;

    Material _orgMaterial;
    Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.AddHit();
        _renderer = GetComponent<Renderer>();
        _orgMaterial = _renderer.sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < 8f && !canBeDestroyed)
        {
            canBeDestroyed = true;
            Gun[] guns = transform.GetComponentsInChildren<Gun>();
            EnemyGun[] enemyguns = transform.GetComponentsInChildren<EnemyGun>();
            foreach (EnemyGun enemygun in enemyguns)
            {
                enemygun.isActive = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeDestroyed)
        {
            return;
        }

        Bullet bullet = collision.GetComponent<Bullet>();
        if(bullet != null)
        {
            if (!bullet.isEnemy)
            {
                hits--;

                if (hits <= 0)
                {
                    GameManager.Instance.Score += points;
                    Destroy(gameObject);
                    Destroy(bullet.gameObject);
                }
                _renderer.sharedMaterial = hitMaterial;
                Invoke("RestoreMaterial", 0.05f);
            }
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.RemoveHit();

    }

    void RestoreMaterial()
    {
        _renderer.sharedMaterial = _orgMaterial;
    }
}
