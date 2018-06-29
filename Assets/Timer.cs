using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public float starttime;
    public float endtime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void StartTimer()
    {
        starttime = Time.time;
    }

    public void StopTimer()
    {
        endtime = Time.time;
		GetComponent<Text>().text = Mathf.Round((endtime - starttime) * 100f) / 100f + "s";
    }
}
