using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {
	
	readonly Vector3 resetPosition = new Vector3(-10f, 10f, -10f);
	readonly Vector3 resetTargetPosition = new Vector3(4.5f, 2.5f, 1.5f);
	readonly float speed = 3f;
	
	Vector3 targetPosition;
	Vector3 lookPosition;
	Vector3 lookPositionLast;
	
	void Awake() { 
		Game.camera = this;
	} 
	
	void Start() {
	}
	
	public void Reset() {
		lookPosition = Game.blocky.transform.position;
		lookPositionLast = lookPosition;
		var angle = 0.0f;
		switch (Game.blocky.direction) {
			case "XP": angle = Mathf.PI*0.0f; break;
			case "ZN": angle = Mathf.PI*0.5f; break;
			case "XN": angle = Mathf.PI*1.0f; break;
			case "ZP": angle = Mathf.PI*1.5f; break;
		}
		Debug.Log(angle);
		transform.position = RotateY(resetPosition, angle) + lookPosition;
		targetPosition = RotateY(resetTargetPosition, angle) + lookPosition;
	}
	
	void Update() {
		
		{ // keep following target's position
			lookPosition = Game.blocky.transform.position;
			transform.LookAt(lookPosition);
			targetPosition += (lookPosition-lookPositionLast);
			lookPositionLast = lookPosition;
			transform.position += (targetPosition-transform.position)*0.1f;
		}
		
		// rotate camera angle when drag
		if ((!Game.workspace.canvas.enabled) && 
			(!Game.blocky.isFrozen()) && 
			((Input.GetMouseButton(0) && Input.mousePosition.y > 100) || 
			(Input.touchCount > 0 && Input.GetTouch(0).position.y > 100))) {
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
		
		// zoom +/-
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
		targetPosition -= (targetPosition-lookPosition)*0.2f;
	}
	
	public void ZoomOut() {
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