using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
	
	AudioSource[] sources;
	string[] names;
	
	void Awake()
	{
		Game.sound = this;
		sources = GetComponents<AudioSource>();
		names = new string[] 
		{
			"jump1", "jump2", "jump3", 
			"win", "connect", "delete", "play", 
			"portal1", "portal2", "portal3",
			"click1", "click2", "click3", 
			"coin1", "coin2", "coin3", "door"
		};
	}
	
	public void play(string sound) 
	{
		int random23 = (new System.Random()).Next(2, 4);
		int random123 = (new System.Random()).Next(1, 4);
		switch (sound)
		{
			case "JUMP": _play("jump" + random123); break;
			case "WIN": _play("win"); break;
			case "CONNECT": _play("connect"); break;
			case "DELETE": _play("delete"); break;
			case "PLAY": _play("play"); break;
			case "PORTAL": _play("portal" + random23); break;
			case "CLICK": _play("click" + random123); break;
			case "COIN": _play("coin" + random123); break;
			case "DOOR": _play("door"); break;
			default: break;
		}
	}
	
	void _play(string name)
	{
		var index = System.Array.IndexOf(names, name);
		if (index >= 0) sources[index].Play();
		Debug.Log("sound.play " + name + " " + index);
	}
	
}
