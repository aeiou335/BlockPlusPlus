using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommandsController : MonoBehaviour
{	
	List<string> list = new List<string>();
	int index;

	void Awake() 
	{
		Game.commands = this;
	}
	
	// clear all commands
	public void Clear() 
	{
		list.Clear();
	}
	
	// clear add commands
	public void Add(string command) 
	{
		list.Add(command);
	}
	
	public void Reset()
	{
		index = 0;
	}
	
	public string Next() 
	{
		if (index >= list.Count) 
			return "<stop>";
		return list[index++];
	}
	
}
