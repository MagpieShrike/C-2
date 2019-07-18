using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float start;
    private float min;
    private float max;


    // Start is called before the first frame update
    void Start()
    {
        start = transform.position.x;

        min = start - 1;
        max = start + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < start)
        {
            transform.localScale = new Vector2(1f, 1f);
        }
        else if (transform.position.x > start)
        {
            transform.localScale = new Vector2(-1f, 1f);
        }

        transform.position = new Vector2(Mathf.PingPong(Time.time, max - min) + min, transform.position.y);

        
    }
}
