using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody enemyRb;
    GameObject player;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position.y < -10)
        {
            Destroy(gameObject);
        }
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        if(speed > 0)
        {
            enemyRb.AddForce(lookDirection * speed);
        }
        else if(speed < 0)
        {
            enemyRb.AddForce(lookDirection * speed, ForceMode.Impulse);
        }
    }
}
