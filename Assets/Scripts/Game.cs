using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game
{
	public static BlockyController blocky;
	public static CameraController camera;
	public static WorkspaceController workspace;
	public static GameManager gameManager;
	//public static int levelNumber = 1;
	
	public static void ReloadLevel() {
		blocky.Reset();
		camera.Reset();
	}
	
	public static void NextLevel() {
		gameManager.CompleteLevel();
		/*
		levelNumber++;
		if (levelNumber > 2) levelNumber = 1;
		SceneManager.LoadScene("Level0"+levelNumber);
		Debug.Log("NextLevel "+levelNumber);
		*/
	}
}
