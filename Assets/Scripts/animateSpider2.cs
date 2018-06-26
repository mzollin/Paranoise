using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animateSpider2 : MonoBehaviour
{
    public Camera Player;
    public float moveSpeed;
    public float minDist;

    private float timer = 5.0f;
    private bool start_timer = false;
    private float attack_timer = 0.0f;

    void Start()
    {
        GetComponent<Animation>().CrossFade("idle");
        start_timer = true;
    }

    void Update()
    {
        if (timer >= 0.0f)
        {
            if (start_timer)
            {
                timer -= Time.deltaTime;
            }
        }
        else
        {
            Vector3 cameraDirection = new Vector3();
            cameraDirection = Player.GetComponent<Transform>().position - transform.position;
            cameraDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(cameraDirection, transform.TransformVector(Vector3.up));

            if (Vector3.Distance(transform.position, Player.GetComponent<Transform>().position) >= minDist)
            {
                // run
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
                GetComponent<Animation>().CrossFade("run");
                attack_timer = 0.0f;
            }
            else
            {
                attack();
            }
        }
    }

    void attack()
    {
        if (attack_timer >= 0)
        {
            attack_timer -= Time.deltaTime;
        }
        else
        {
            Debug.Log("attack1");
            GetComponent<Animation>().CrossFade("attack1");
            GetComponent<AudioSource>().Play();
            attack_timer = 1.5f;
        }

    }
}