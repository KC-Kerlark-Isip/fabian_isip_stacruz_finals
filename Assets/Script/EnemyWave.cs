using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    float sinCenterY;
    public float amplitude = 2;
    public float moveSpeed = 5;
    public float frequency = 2f;

    public bool inverted = false;


    // Start is called before the first frame update
    void Start()
    {
        sinCenterY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= moveSpeed * Time.fixedDeltaTime;

        float sin = Mathf.Sin(pos.x * frequency) * amplitude;
        pos.y = sinCenterY + sin;

        if (pos.x < -20)
        {
            Destroy(gameObject);
        }

        if (inverted)
        {
            sin *= -1;
        }

        transform.position = pos;
    }
}
