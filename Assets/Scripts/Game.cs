using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game
{
	public static BlockyController blocky;
	public static CameraController camera;
	public static WorkspaceController workspace;
	public static LevelController level;
	public static int levelNumber = 1;
	
	public static void ReloadLevel() {
		blocky.Reset();
		camera.Reset();
	}
	
	public static void NextLevel() {
		Debug.Log("Game.NextLevel()");
		levelNumber += 1;
		level.CompleteLevel();
		/*
		levelNumber++;
		if (levelNumber > 2) levelNumber = 1;
		SceneManager.LoadScene("Level0"+levelNumber);
		Debug.Log("NextLevel "+levelNumber);
		*/
	}
}
