﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelController : MonoBehaviour
{	
	void Awake() {
	}
	
	void Update() {
	}
	
	public void ButtonZoomInClicked() {
		Game.camera.ZoomIn();
	}
	
	public void ButtonZoomOutClicked() {
		Game.camera.ZoomOut();
	}
	
	public void ButtonSwitchClicked() {
		Game.workspace.canvas.enabled ^= true;
		//Invoke("LoadUGUIDemo", 1);
		//Invoke("LoadLevel01", 10);
	}
	
	/*

	public void ButtonUpClicked() {
		Game.blocky.MoveForward();
	}
	
	public void ButtonDownClicked() {
		Game.blocky.MoveBackward();
	}
	
	public void ButtonLeftClicked() {
		Game.blocky.TurnLeft();
	}
	
	public void ButtonRightClicked() {
		Game.blocky.TurnRight();
	}
	
    private void LoadUGUIDemo()
    {
        SceneManager.LoadScene("UGUIDemo");
    }
    
    private void LoadLevel01()
    {
        SceneManager.LoadScene("Level01");
    }
	*/
}
