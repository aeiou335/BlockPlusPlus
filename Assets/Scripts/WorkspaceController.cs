using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorkspaceController : MonoBehaviour
{	
	void Awake() {
		Game.workspace = GetComponent<Canvas>();
		Game.workspace.enabled = false;
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Debug.Log("Input.GetKeyDown(p)");
			Game.workspace.enabled ^= true;
		}
	}
	
	public void ButtonSwitchClicked() {
		Game.workspace.enabled ^= true;
	}
	
}
