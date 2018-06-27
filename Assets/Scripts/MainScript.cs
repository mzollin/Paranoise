using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class MainScript : MonoBehaviour {

    public Camera FirstPersonCamera;
    public GameObject DetectedPlanePrefab;
    public GameObject SearchingForPlaneUI;

    public GameObject monsterPrefab;
    private Vector3 monsterRotation = new Vector3(0.0f, 0.0f, 0.0f);
    ////FIXME: lower spider partially below ground
    //private Vector3 monsterTranslation = new Vector3(0.0f, -0.3f, 0.0f);
    private Vector3 monsterTranslation = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 monsterScale = new Vector3(0.5f, 0.5f, 0.5f);

    public GameObject portalPrefab;

    public AudioSource bugnoise;
    public AudioClip bugclip;

    public GameObject floorPrefab;
    private Vector3 floorRotation = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 floorTranslation = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 floorScale = new Vector3(1.0f, 1.0f, 1.0f);

    public HealthScript health;

    private bool objectCreated = false;

    private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

    private bool m_IsQuitting = false;

    private float bugNoiseTimer = 5.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        _UpdateApplicationLifecycle();

        // Hide snackbar when currently tracking at least one plane.
        Session.GetTrackables<DetectedPlane>(m_AllPlanes);
        bool showSearchingUI = true;
        for (int i = 0; i < m_AllPlanes.Count; i++)
        {
            if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
            {
                showSearchingUI = false;

                if (!objectCreated)
                {
                    objectCreated = true;

                    var anchor = m_AllPlanes[i].CreateAnchor(m_AllPlanes[i].CenterPose);

                    var monsterObject = Instantiate(monsterPrefab, m_AllPlanes[i].CenterPose.position, m_AllPlanes[i].CenterPose.rotation);
                    monsterObject.transform.Rotate(monsterRotation);
                    monsterObject.transform.Translate(monsterTranslation);
                    monsterObject.transform.localScale = monsterScale;
                    monsterObject.transform.parent = anchor.transform;

                    var portalObject = Instantiate(portalPrefab, m_AllPlanes[i].CenterPose.position, m_AllPlanes[i].CenterPose.rotation);
                    portalObject.transform.parent = anchor.transform;


                    //var monsterMaterial = monsterObject.GetComponent<Material>();
                    //Vector3 planeNormal = m_AllPlanes[i].CenterPose.up.normalized;
                    //Vector3 planePos = m_AllPlanes[i].CenterPose.position;
                    //Vector4 sectionplane = new Vector4(planeNormal.x, planeNormal.y, planeNormal.z, -Vector3.Dot(planeNormal, planePos));
                    //monsterMaterial.SetVector("Sectionplane", sectionplane);

                    //var floorObject = Instantiate(floorPrefab, m_AllPlanes[i].CenterPose.position, m_AllPlanes[i].CenterPose.rotation);
                    //floorObject.transform.Rotate(floorRotation);
                    //floorObject.transform.Translate(floorTranslation);
                    //floorObject.transform.localScale = floorScale;
                    //floorObject.transform.parent = anchor.transform;


                    // Let the monster look at the camera but keep it on the ground (plane)
                    Vector3 cameraDirection = new Vector3();
                    cameraDirection = transform.InverseTransformPoint(FirstPersonCamera.transform.position) - monsterObject.transform.position;
                    cameraDirection.y = 0;
                    monsterObject.transform.rotation = Quaternion.LookRotation(cameraDirection, monsterObject.transform.TransformVector(Vector3.up));
                }

                break;
            }
        }

        // make creepy bug noises at random places
        if (bugNoiseTimer >= 0)
        {
            bugNoiseTimer -= Time.deltaTime;
        }
        else
        {
            Vector3 position = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
            AudioSource.PlayClipAtPoint(bugclip, position);
            bugNoiseTimer = Random.Range(0.0f, 10.0f);
        }

        SearchingForPlaneUI.SetActive(showSearchingUI);

        // If the player has not touched the screen, we are done with this update.
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }
    }

    // Check and update the application lifecycle.
    private void _UpdateApplicationLifecycle()
    {
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Only allow the screen to sleep when not tracking.
        if (Session.Status != SessionStatus.Tracking)
        {
            const int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        if (m_IsQuitting)
        {
            return;
        }

        // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            _ShowAndroidToastMessage("Camera permission is needed to run this application.");
            m_IsQuitting = true;
            Invoke("_DoQuit", 0.5f);
        }
        else if (Session.Status.IsError())
        {
            _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
            m_IsQuitting = true;
            Invoke("_DoQuit", 0.5f);
        }
    }

    // Actually quit the application.
    private void _DoQuit()
    {
        Application.Quit();
    }

    // Show an Android toast message.
    private void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                    message, 0);
                toastObject.Call("show");
            }));
        }
    }
}
