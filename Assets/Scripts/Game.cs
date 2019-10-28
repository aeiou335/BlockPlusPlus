using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game
{
	public static BlockyController blocky;
	public static CameraController camera;
	public static int levelNumber = 1;
	public GameManager gameManager;
	
	public static void ReloadLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		Debug.Log("ReloadLevel");
	}
	/*
	public static void NextLevel() {
		
		levelNumber++;
		if (levelNumber > 2) levelNumber = 1;
		SceneManager.LoadScene("Level0"+levelNumber);
		Debug.Log("NextLevel "+levelNumber);
		
		gameManager.CompleteLevel();
		
	}
	*/
	
}
