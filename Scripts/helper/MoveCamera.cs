using UnityEngine;
#if WINDOWS_UWP
using Windows.System.Profile;
#endif

/// <summary>
/// move camera in UWP build
/// (use for debugging without deploying to hololens)
/// </summary>
public class MoveCamera : MonoBehaviour {
#if WINDOWS_UWP
    private float sensitivity = 0.003f;

    private Vector3 startMousePos = Vector3.zero;
    private Vector3 startCamPos = Vector3.zero;

    void Start()
    {
        string family = AnalyticsInfo.VersionInfo.DeviceFamily;
        // ignore mouse on Hololens
        gameObject.SetActive(family == "Windows.Desktop");
    }

    void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            startCamPos = new Vector3(
                Camera.main.transform.position.x,
                Camera.main.transform.position.y,
                Camera.main.transform.position.z);
            startMousePos = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                Input.mousePosition.z);
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 camPos = new Vector3(
                startCamPos.x,
                startCamPos.y,
                startCamPos.z);


            camPos.x += (Input.mousePosition.x - startMousePos.x) * sensitivity;
            camPos.y += (Input.mousePosition.y - startMousePos.y) * sensitivity;
            camPos.z += (Input.mousePosition.z - startMousePos.z) * sensitivity;
            Camera.main.transform.position = camPos;
        }

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 pos = Camera.main.transform.position;
            pos.z += .01f;
            Camera.main.transform.position = pos;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Vector3 pos = Camera.main.transform.position;
            pos.z -= .01f;
            Camera.main.transform.position = pos;
        }
    }
#endif
}
