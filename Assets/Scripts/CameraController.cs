using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour 
{
	//readonly Vector3 positionReset = new Vector3(-10f, 10f, -10f);
	//readonly Vector3 targetPositionReset = new Vector3(3.5f, 2.0f, 1.0f);
	readonly Vector3 positionReset = new Vector3(-15f, 15f, -15f);
	readonly Vector3 targetPositionReset = new Vector3(5f, 5f, 2f);
	readonly float speed = 3f;
	
	Vector3 targetPosition;
	Vector3 lookPosition;
	//Vector3 lookPositionLast;
	float lastDistance;
	
	void Awake() 
	{ 
		Game.camera = this;
	}
	
	void Start()
	{
		var grounds = GameObject.FindGameObjectsWithTag("Ground");
		lookPosition = new Vector3(0, 0, 0);
		foreach (var ground in grounds)
			lookPosition += ground.transform.position;
		lookPosition /= grounds.Length;
		Reset();
	}
	
	public void Reset() 
	{
		lastDistance = -1;
		transform.position = positionReset + lookPosition;
		targetPosition = targetPositionReset + lookPosition;
	}
	
	void Update() 
	{
		// keep following target's position
		transform.LookAt(lookPosition);
		transform.position += (targetPosition-transform.position)*0.1f;
		
		try 
		{
			if (Game.level.workspace.GetComponent<Canvas>().enabled) return;
		}
		catch (System.Exception e) { return; }
		
		// if not ready yet
		if (Game.blocky.isFrozen() || Game.blocky.isEnded()) return;
		
		// rotate camera angle when drag
		if ((Input.touchCount == 0 && Input.GetMouseButton(0)) || (Input.touchCount == 1)) 
		//if ((Input.touchCount == 0 && Input.GetMouseButton(0) && Input.mousePosition.y > 100) || 
		//	(Input.touchCount == 1 && Input.GetTouch(0).position.y > 100)) 
		{
			float dx = Input.GetAxis("Mouse X") * 3f;
			float dy = Input.GetAxis("Mouse Y") * 3f;
			if (Input.touchCount > 0) {
				dx = Input.touches[0].deltaPosition.x * 0.2f;
				dy = Input.touches[0].deltaPosition.y * 0.2f;
			}
			Vector3 angles = transform.eulerAngles;
			angles.z = 0;
			transform.eulerAngles = angles;
			transform.RotateAround(lookPosition, Vector3.up, dx);
			if (!(
				(Mathf.Abs(Mathf.DeltaAngle(angles.x, 90)) < 10 && dy < 0) || 
				(Mathf.Abs(Mathf.DeltaAngle(angles.x, 0)) < 10 && dy > 0)))
				transform.RotateAround(lookPosition, Camera.main.transform.right, -dy);
			targetPosition = transform.position;
		}
		
		// 2-touch zoom
		if (Input.touchCount != 2) 
			lastDistance = -1;
		if (Input.touchCount == 2) 
		{
			Vector3 pos1 = Input.GetTouch(0).position;
			Vector3 pos2 = Input.GetTouch(1).position;
			float distance = (pos1-pos2).magnitude;
			if (lastDistance == -1)
				lastDistance = distance;
			if (distance - lastDistance > 50) {
				Debug.Log("2-Touch Zoom In");
				ZoomIn(); 
				lastDistance = distance;
			}
			if (lastDistance - distance > 50) {
				Debug.Log("2-Touch Zoom Out");
				ZoomOut(); 
				lastDistance = distance;
			}
		}
		
		// keyboard zoom
		if (Input.GetKeyDown(",")) ZoomIn();
		if (Input.GetKeyDown(".")) ZoomOut();
		
	}
	
	public void ZoomIn() 
	{
		if ((transform.position - lookPosition).magnitude < 1) return;
		targetPosition -= (targetPosition-lookPosition)*0.2f;
	}
	
	public void ZoomOut() 
	{
		if ((transform.position - lookPosition).magnitude > 100) return;
		targetPosition += (targetPosition-lookPosition)*0.2f;
	}
}
