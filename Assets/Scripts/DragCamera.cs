using UnityEngine;
using System.Collections;

public class DragCamera : MonoBehaviour {
	
	public float speed = 3f;
	
	void Update()
	{
		OrbitCamera();
	}

	public void OrbitCamera()
	{
		if (Input.GetMouseButton(0) || Input.touchCount > 0)
		{
			float x = Input.GetAxis("Mouse X") * 3f;
			float y = Input.GetAxis("Mouse Y") * 3f;
			if (Input.touchCount > 0) {
				x = Input.touches[0].deltaPosition.x * 0.2f;
				y = Input.touches[0].deltaPosition.y * 0.2f;
			}
			Vector3 target = Vector3.zero;
			Vector3 angles = transform.eulerAngles;
			angles.z = 0;
			transform.eulerAngles = angles;
			transform.RotateAround(target, Vector3.up, x);
			if (!(
				(Mathf.Abs(Mathf.DeltaAngle(angles.x,  90)) < 10 && y < 0) || 
				(Mathf.Abs(Mathf.DeltaAngle(angles.x, 270)) < 10 && y > 0)))
				transform.RotateAround(target, Camera.main.transform.right, -y);
			transform.LookAt(target);
			Debug.Log(angles);
		}
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