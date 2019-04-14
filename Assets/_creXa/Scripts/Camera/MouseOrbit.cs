using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("creXa/Camera-Control/MouseOrbit")]
public class MouseOrbit : MonoBehaviour {

    public Vector3 target;
    public float distance = 10.0f;

    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20.0f;
    public float yMaxLimit = 80.0f;

    float x = 0.0f;
    float y = 0.0f;
    float smooth = 0.0f;

	// Use this for initialization
	void Start () {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
	}
	
	// Update is called once per frame
	void LateUpdate () {

        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl))
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        }

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0.5f, -distance) + target;

        transform.rotation = rotation;
        transform.position = position;

        if (Input.GetAxis("Mouse ScrollWheel") != 0 && Input.GetKey(KeyCode.LeftControl))
        {
            smooth += Input.GetAxis("Mouse ScrollWheel");
        }
        distance += smooth;
        if (distance < 1)
            distance = 1;
        if (distance > 6)
            distance = 6;
        if (smooth != 0)
            smooth /= 1.2f;

    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
