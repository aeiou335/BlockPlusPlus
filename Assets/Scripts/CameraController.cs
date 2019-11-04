using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {
	
	public float speed = 3f;
	public Vector3 targetPos;
	public Vector3 lookPos;
	public Vector3 lookPosLast;
	
	void Awake() { 
		Game.camera = this;
		transform.position = new Vector3(-5f, 5f, -5f);
		targetPos = new Vector3(1f, 1f, -1f);
	} 
	
	void Update() {
		
		// keep following target's position
		lookPos = Game.blocky.transform.position;
		transform.LookAt(lookPos);
		if (lookPosLast != null)
			targetPos += (lookPos-lookPosLast);
		lookPosLast = lookPos;
		transform.position += (targetPos-transform.position)*0.1f;
		
		// rotate camera angle when drag
		if ((!Game.workspace.enabled) && 
			((Input.GetMouseButton(0) && Input.mousePosition.y > 100) || 
			(Input.touchCount > 0 && Input.GetTouch(0).position.y > 100))) {
			float dx = Input.GetAxis("Mouse X") * 3f;
			float dy = Input.GetAxis("Mouse Y") * 3f;
			if (Input.touchCount > 0) {
				dx = Input.touches[0].deltaPosition.x * 0.2f;
				dy = Input.touches[0].deltaPosition.y * 0.2f;
			} //Vector3.zero;
			Vector3 angles = transform.eulerAngles;
			angles.z = 0;
			transform.eulerAngles = angles;
			transform.RotateAround(lookPos, Vector3.up, dx);
			if (!(
				(Mathf.Abs(Mathf.DeltaAngle(angles.x, 90)) < 10 && dy < 0) || 
				(Mathf.Abs(Mathf.DeltaAngle(angles.x, 0)) < 10 && dy > 0)))
				transform.RotateAround(lookPos, Camera.main.transform.right, -dy);
			targetPos = transform.position;
		}
		
		// zoom +/-
		if (Input.GetKeyDown(",")) ZoomIn();
		if (Input.GetKeyDown(".")) ZoomOut();
		
	}
	
	public void ZoomIn() {
		targetPos -= (targetPos-lookPos)*0.2f;
	}
	
	public void ZoomOut() {
		targetPos += (targetPos-lookPos)*0.2f;
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