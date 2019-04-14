// Credit to damien_oconnell from http://forum.unity3d.com/threads/39513-Click-drag-camera-movement
// for using the mouse displacement for calculating the amount of camera movement and panning code.

using UnityEngine;
using System.Collections;

[AddComponentMenu("creXa/Camera-Control/Camera Movement")]
public class CameraMovement : MonoBehaviour 
{
	public bool locked = false;

	public Vector3 initpos;
	public Vector3 initrot;
	
	public float turnSpeed;		// Speed of camera turning when mouse moves in along an axis
	public float panSpeed;		// Speed of the camera when being panned
	public float zoomSpeed;		// Speed of the camera going back and forth
	
	public float maxpanSpeed;
	
	private Vector3 mouseOrigin;	// Position of cursor when mouse dragging starts
	private bool isPanning;		// Is the camera being panned?
	private bool isRotating;	// Is the camera being rotated?
	private bool isZooming;		// Is the camera zooming?
	
	void Start (){
		init();
    }
	
	void init(){
		transform.position = initpos;
		transform.eulerAngles = initrot;
	}
	
	void Update () 
	{
		if(!locked){
			if(Input.GetKeyDown(KeyCode.Home)){
				init();
			}
			
			if(Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(1))
			{
				// Get mouse origin
				mouseOrigin = Input.mousePosition;
				isRotating = true;
			}
			
			if(Input.GetMouseButtonDown(2) || Input.touchCount == 2)
			{
				// Get mouse origin
				mouseOrigin = Input.mousePosition;
				isPanning = true;
			}
			
			if(Input.GetKey(KeyCode.LeftControl) && Input.GetAxis("Mouse ScrollWheel") != 0)
			{
				// Get mouse origin
				mouseOrigin = Input.mousePosition;
				isZooming = true;
			}
			
			// Disable movements on button release
			if (!Input.GetKey(KeyCode.LeftControl) || !Input.GetMouseButton(1)) isRotating=false;
			if (!Input.GetMouseButton(2) && Input.touchCount != 2) isPanning=false;
			if (!Input.GetKey(KeyCode.LeftControl) || Input.GetAxis("Mouse ScrollWheel") == 0) isZooming=false;
			
			// Rotate camera along X and Y axis
			if (isRotating)
			{
				Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
				
				transform.RotateAround(transform.position, transform.right, -pos.y * turnSpeed);
				transform.RotateAround(transform.position, Vector3.up, pos.x * turnSpeed);
			}
			
			// Move the camera on it's XY plane
			if (isPanning)
			{
				Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
				
				Vector3 move = new Vector3(Mathf.Clamp(pos.x, -maxpanSpeed, maxpanSpeed) * panSpeed, Mathf.Clamp(pos.y, -maxpanSpeed, maxpanSpeed) * panSpeed, 0);
				transform.Translate(move, Space.Self);
			}
			
			// Move the camera linearly along Z axis
			if (isZooming)
			{
				Vector3 move = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * transform.forward; 
				transform.Translate(move, Space.World);
			}
		}
	}
}