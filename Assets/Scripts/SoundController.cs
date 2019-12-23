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
			"jump1", "jump2", "jump3", "win", "connect", 
			"delete", "play", "portal1", "portal2", "portal3"
		};
	}
	
	public void play(string sound) 
	{
		switch (sound)
		{
			case "JUMP": _play("jump"+(new System.Random()).Next(1, 4)); break;
			case "WIN": _play("win"); break;
			case "CONNECT": _play("connect"); break;
			case "DELETE": _play("delete"); break;
			case "PLAY": _play("play"); break;
			case "PORTAL": _play("portal"+(new System.Random()).Next(2, 4)); break;
			default: break;
		}
	}
	
	void _play(string name)
	{
		var index = System.Array.IndexOf(names, name);
		if (index >= 0) sources[index].Play();
	}
	
}
