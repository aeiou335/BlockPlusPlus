using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {
	
	//readonly Vector3 positionReset = new Vector3(-10f, 10f, -10f);
	//readonly Vector3 targetPositionReset = new Vector3(3.5f, 2.0f, 1.0f);
	readonly Vector3 positionReset = new Vector3(-15f, 15f, -15f);
	readonly Vector3 targetPositionReset = new Vector3(5f, 5f, 2f);
	readonly float speed = 3f;
	
	Vector3 targetPosition;
	Vector3 lookPosition;
	//Vector3 lookPositionLast;
	float lastDistance;
	
	void Awake() { 
		Game.camera = this;
	} 
	
	void Start() {
		Reset();
	}
	
	public void Reset() {
		lastDistance = -1;
		transform.position = positionReset + lookPosition;
		targetPosition = targetPositionReset + lookPosition;
		{ // find center of world
			var grounds = GameObject.FindGameObjectsWithTag("Ground");
			lookPosition = new Vector3(0, 0, 0);
			foreach (var ground in grounds)
			{
				lookPosition += ground.transform.position;
				Debug.Log(ground.transform.position);
			}
			lookPosition /= grounds.Length;
			Debug.Log(lookPosition);
		}
		/*
		lookPosition = Game.blocky.transform.position;
		lookPositionLast = lookPosition;
		var angle = 0.0f;
		switch (Game.blocky.direction) {
			case "XP": angle = Mathf.PI*0.0f; break;
			case "ZN": angle = Mathf.PI*0.5f; break;
			case "XN": angle = Mathf.PI*1.0f; break;
			case "ZP": angle = Mathf.PI*1.5f; break;
		}
		//Debug.Log(angle);
		transform.position = RotateY(positionReset, angle) + lookPosition;
		targetPosition = RotateY(targetPositionReset, angle) + lookPosition;
		*/
	}
	
	void Update() {
		
		{ // keep following target's position
			transform.LookAt(lookPosition);
			transform.position += (targetPosition-transform.position)*0.1f;
			/*
			lookPosition = Game.blocky.transform.position;
			transform.LookAt(lookPosition);
			targetPosition += (lookPosition-lookPositionLast);
			lookPositionLast = lookPosition;
			transform.position += (targetPosition-transform.position)*0.1f;
			*/
		}
		
		if (Game.workspace.canvas.enabled) return;
		if (Game.blocky.isFrozen()) return;
		
		// rotate camera angle when drag
		if ((Input.touchCount == 0 && Input.GetMouseButton(0) && Input.mousePosition.y > 100) || 
			(Input.touchCount == 1 && Input.GetTouch(0).position.y > 100)) {
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
		if (Input.touchCount == 2) {
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
		
		// zoom
		if (Input.GetKeyDown(",")) ZoomIn();
		if (Input.GetKeyDown(".")) ZoomOut();
		
	}
	
	// rotate a point around Y-axis by some angle
	Vector3 RotateY(Vector3 point, float angle) {
		float x = Mathf.Cos(angle)*point.x+Mathf.Sin(angle)*point.z;
		float y = point.y;
		float z = Mathf.Cos(angle)*point.z-Mathf.Sin(angle)*point.x;
		return new Vector3(x, y, z);
	}
	
	public void ZoomIn() {
		if ((transform.position - lookPosition).magnitude < 1) return;
		targetPosition -= (targetPosition-lookPosition)*0.2f;
	}
	
	public void ZoomOut() {
		if ((transform.position - lookPosition).magnitude > 100) return;
		targetPosition += (targetPosition-lookPosition)*0.2f;
	}
}


/*

public class DragCamera : MonoBehaviour
{
	public Camera camera;
	public GameObject player;
	public float speed = 2f;
	
	void Awake() {
		camera = Camera.main;
		player = GameObject.FindWithTag("Player");
	}
	
	void Update() {
		if (Input.GetMouseButton(0)) {
			camera.transform.RotateAround(
				new Vector3(0, 0, 0), //player.transform.position, 
				camera.transform.up,
				+Input.GetAxis("Mouse X")*speed
			);
			camera.transform.RotateAround(
				new Vector3(0, 0, 0), //player.transform.position, 
				camera.transform.right,
				-Input.GetAxis("Mouse Y")*speed
			);
			camera.transform.rotation = Quaternion.Euler(
				camera.transform.rotation.eulerAngles.x, 
				camera.transform.rotation.eulerAngles.y, 0f );
		}
	}
	
}

 public class DragCamera : MonoBehaviour {
     public float speed = 3.5f;
     private float X;
     private float Y;
 
     void Update() {
         if(Input.GetMouseButton(0)) {
             transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed, -Input.GetAxis("Mouse X") * speed, 0));
             X = transform.rotation.eulerAngles.x;
             Y = transform.rotation.eulerAngles.y;
             transform.rotation = Quaternion.Euler(X, Y, 0);
         }
     }
 }
 
*/