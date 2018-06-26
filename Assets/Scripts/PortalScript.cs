using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour {

    public Camera camera = null;

	// Use this for initialization
	void Start () {
		if(camera == null)
        {
            camera = FindObjectOfType<Camera>();
        }  
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Transform>().LookAt(camera.GetComponent<Transform>());
	}

    private float last_attack = 0f;

    private void FixedUpdate()
    {
        if(last_attack + 1.5f < Time.time)
        {
            last_attack = Time.time;
            attack();
        }
    }

    private void attack()
    {
        //attack
    }
}
