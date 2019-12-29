using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game
{
	public static BlockyController blocky;
	public static CameraController camera;
	public static CommandsController commands;
	public static LevelController level;
	public static SoundController sound;
	public static int chapterNumber = 1;
	public static int levelNumber = 1;
	public static int characterNumber = 0;
	public readonly static bool forVideo = true;
}
