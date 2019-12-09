using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorkspaceController : MonoBehaviour
{
	public Canvas canvas;
	
	List<string> commands = new List<string>();

	void Awake() {
		Game.workspace = this;
		canvas = GetComponent<Canvas>();
		canvas.enabled = false;
	}
	
	void Update() {
	}
	
	public void ClearCommands() {
		commands.Clear();
	}
	
	public void AddCommand(string command) {
		commands.Add(command);
	}
	
	public List<string> GetCommands() {
		return commands;
	}
	
}
