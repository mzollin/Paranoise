using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animateSpider2 : MonoBehaviour {
    public Camera Player;
    public int moveSpeed = 4;
    public int maxDist = 10;
    public int minDist = 5;

    private bool activeSpider;

    void Start()
    {
        GetComponent<Animation>().CrossFade("idle");
        activeSpider = false;
        StartCoroutine(nop());
    }

    void Update()
    {
        if (activeSpider)
        {
            Vector3 cameraDirection = new Vector3();
            cameraDirection = Player.transform.position - transform.position;
            cameraDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(cameraDirection, transform.TransformVector(Vector3.up));

            if (Vector3.Distance(transform.position, Player.transform.position) >= minDist)
            {

                transform.position += transform.forward * moveSpeed * Time.deltaTime;


                GetComponent<Animation>().CrossFade("run");



                if (Vector3.Distance(transform.position, Player.transform.position) <= maxDist)
                {
                    StartCoroutine(attack());
                }

            }
        }
    }

    IEnumerator attack()
    {
        for (int i = 0; i < 100; i++)
        {
            if (Vector3.Distance(transform.position, Player.transform.position) > maxDist)
            {
                break;
            }



            if (i % 2 == 0)
            {
                Debug.Log("attack1");
                GetComponent<Animation>().CrossFade("attack1");
            }
            else
            {
                Debug.Log("attack2");
                GetComponent<Animation>().CrossFade("attack2");
            }

            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(1.5f);

        }
    }

    IEnumerator nop()
    {
        yield return new WaitForSeconds(10);
        activeSpider = true;
    }
}
