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
	public static SoundController sound;
	public static int levelNumber = 1;
	
	/*
	
	public static void ReloadLevel() {
		level.EndGame();
	}
	
	public static void NextLevel() {
		Debug.Log("Game.NextLevel()");
		levelNumber += 1;
		level.CompleteLevel();
	}
	*/
}
