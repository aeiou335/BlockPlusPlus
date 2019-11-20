using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DiamondController : MonoBehaviour {
	
	void Awake() { 
	} 
	
	void Start() {
	}
	
	void Update() {
		transform.Rotate(Vector3.up*100*Time.deltaTime, Space.Self);
	}
}
