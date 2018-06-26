using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour {

    public Camera camera = null;

    private float time;
    public float lifetime = 5f;

	// Use this for initialization
	void Start () {
        time = Time.time;
		if(camera == null)
        {
            camera = FindObjectOfType<Camera>();
        }  
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Transform>().LookAt(camera.GetComponent<Transform>());
	}

    public void FixedUpdate()
    {
        if(Time.time - time > lifetime)
        {
            Destroy(this);
        }
        float scale = Mathf.Sin((Time.time - time) * Mathf.PI / lifetime);
        GetComponent<Transform>().localScale = new Vector3(scale, scale, scale);
    }
}
