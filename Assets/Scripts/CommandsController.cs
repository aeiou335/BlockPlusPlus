using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommandsController : MonoBehaviour
{	
	List<string> commands = new List<string>();
	List<int> puzzles = new List<int>();
	int index;

	void Awake() 
	{
		Game.commands = this;
	}
	
	// clear all commands
	public void Clear() 
	{
		commands.Clear();
		puzzles.Clear();
	}
	
	// add command
	public void Add(string command) 
	{
		commands.Add(command);
	}
	
	// reset, ready for run
	public void Reset()
	{
		index = 0;
	}
	
	// get next command
	public string Next() 
	{
		if (index >= commands.Count) 
			return "<stop>";
		return commands[index++];
	}
	
	// add puzzle
	public void AddPuzzle(int puzzle) 
	{
		if (puzzles.IndexOf(puzzle) == -1)
			puzzles.Add(puzzle);
	}
	
	// get number of puzzles 
	public int PuzzlesNumber() 
	{
		return puzzles.Count;
	}
	
}
