using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animateSpider2 : MonoBehaviour
{
    private Camera Player;
    public float moveSpeed;
    public float minDist;

    private float timer = 5.0f;
    private bool start_timer = false;
    private float attack_timer = 0.0f;

    void Start()
    {
        GetComponent<Animation>().CrossFade("idle");
        start_timer = true;
        Player = FindObjectOfType<Camera>();
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
            Transform t = GetComponent<Transform>();
            cameraDirection = Player.GetComponent<Transform>().position - t.position;
            cameraDirection.y = 0;
            //transform.rotation = Quaternion.LookRotation(cameraDirection, transform.TransformVector(Vector3.up));
            t.LookAt(Player.GetComponent<Transform>());
            t.eulerAngles = new Vector3(0, t.eulerAngles.y, 0);


            Vector3 monster_position = new Vector3();
            Vector3 player_position = new Vector3();
            monster_position = transform.position;
            player_position = Player.GetComponent<Transform>().position;
            monster_position.y = 0;
            player_position.y = 0;

            //if (Vector3.Distance(transform.position, Player.GetComponent<Transform>().position) >= minDist)
            if (Vector3.Distance(monster_position, player_position) >= minDist)
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