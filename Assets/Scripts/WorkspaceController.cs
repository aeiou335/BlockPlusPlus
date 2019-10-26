using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkspaceController : MonoBehaviour
{	
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
	
	public void ButtonZoomInClicked() {
		Game.camera.ZoomIn();
	}
	
	public void ButtonZoomOutClicked() {
		Game.camera.ZoomOut();
	}
}
