using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockyController : MonoBehaviour 
{
	public Rigidbody rb;
	public string state; // { STOP, COOL, FRONT, BACK, LEFT, RIGHT, DEAD, FINISH, RESET, MENU }
	public int direction; // { XP, ZP, XN, ZN }
	
	readonly float timerDelay = 0.025f;
	readonly int[] directions = {0, 90, 180, 270};
	
	Vector3 resetPosition, resetAngle;
	int resetDirection;
	int countDown;
	int breathValue;
	Vector3 turnAxis;
	bool commandRunning;
	int commandIndex;
	int diamondNumber;
	float startHeight;
	string standOn;
	
	void Awake() 
	{ 
		Game.blocky = this;
	}
	
	void Start() 
	{
		rb = GetComponent<Rigidbody>();
		InvokeRepeating("TimerTick", timerDelay, timerDelay);
		var pos = transform.position;
		var angle = transform.eulerAngles;
		resetPosition = new Vector3(pos.x, pos.y, pos.z);
		resetAngle = new Vector3(angle.x, angle.y, angle.z);
		foreach (var d in directions)
			if (Mathf.Abs(Mathf.DeltaAngle(angle.y, d)) < 45)
				resetDirection = d;
		if (SceneManager.GetActiveScene().name == "Menu") {
			breathValue = 1;
			SetState("MENU", 0);
		} else {
			Game.blocky.Reset();
			Game.camera.Reset();
		}
	}
	
	public void Reset() 
	{
		transform.position = resetPosition;
		transform.eulerAngles = resetAngle;
		direction = resetDirection;
		rb.velocity = Vector3.zero;
		breathValue = 1;
		commandRunning = false;
		diamondNumber = 0;
		SetState("RESET", countDown);
	}
	
	void Update() 
	{
		if (isFrozen()) return;
		if (transform.position.y < -10) SetState("DEAD", 0);
	}
	
	// Called every timer ticks
	void TimerTick() 
	{
		{ // Breath
			if (transform.localScale.x > 0.5+0.015) breathValue = -1;
			if (transform.localScale.x < 0.5-0.015) breathValue = +1;
			float delta = timerDelay*0.1f;
			Vector3 deltaVector = new Vector3(delta, -delta, delta);
			transform.localScale += deltaVector*breathValue;
		}
		switch (state) 
		{
			case "RESET":
				if (--countDown < 0) SetState("STOP", 0);
				break;
			case "FINISH":
				if (--countDown < 0) { SetState("END", 0); Game.level.CompleteLevel(); }
				break;
			case "DEAD":
				if (--countDown < 0) { SetState("END", 0); Game.level.FailLevel(); }
				break;
			case "STOP":
				Correction();
				if (commandRunning) NextCommand();
				break;
			case "COOL":
				Correction();
				if (--countDown < 0) SetState("STOP", 0);
				break;
			case "MOVE":
				if (transform.position.y < startHeight) 
					rb.velocity = new Vector3(0, rb.velocity.y, 0);
				//if (rb.velocity.magnitude < 0.01 && rb.angularVelocity.magnitude < 0.01) 
				//	SetState("COOL", 15);
				break;
			case "TURN":
				if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, direction)) > 2) 
					transform.RotateAround(transform.position, turnAxis, timerDelay*200);
				break;
			default:
				break;
		}
	}
	
	// is frozen?
	public bool isFrozen() 
	{
		return (state == "RESET" || state == "MENU");
	}
	
	// is ended?
	public bool isEnded() 
	{
		return (state == "END" || state == "DEAD" || state == "FINISH");
	}
	
	// Start running command
	public void CommandStart() 
	{
		commandIndex = 0;
		commandRunning = true;
	}
	
	// Set state
	void SetState(string _state, int _countDown) 
	{
		Debug.Log(_state);
		state = _state;
		countDown = _countDown;
	}
	
	// Move forward/backward 1 block
	void Move(string dir) 
	{
		if (state != "STOP") return;
		var sign = (dir == "FORWARD"? 1: -1);
		var vx = 1.2f; var vy = 4f;
		switch (direction) 
		{
			case   0: rb.velocity = new Vector3(vx*sign, vy, 0f); break;
			case  90: rb.velocity = new Vector3(0f, vy, -vx*sign); break;
			case 180: rb.velocity = new Vector3(-vx*sign, vy, 0f); break;
			case 270: rb.velocity = new Vector3(0f, vy, vx*sign); break;
		}
		SetState("MOVE", 0);
		startHeight = transform.position.y;
		Game.sound.play("JUMP");
	}
	
	// Turn left/right 90 degrees
	void Turn(string dir) 
	{
		if (state != "STOP") return;
		SetState("TURN", 0);
		rb.velocity = new Vector3(0f, 2.6f, 0f);
		direction = (direction + (dir == "LEFT"? 270: 90)) % 360;
		turnAxis = (dir == "LEFT"? Vector3.down: Vector3.up);
		Game.sound.play("JUMP");
	}
	
	// Correct position and rotation value
	void Correction() 
	{ 
		Vector3 pos = transform.position;
		Vector3 eul = transform.eulerAngles;
		float dx = (pos.x < 0? -0.5f: +0.5f);
		float dz = (pos.z < 0? -0.5f: +0.5f);
		var targetPos = new Vector3((int)pos.x+dx, pos.y, (int)pos.z+dz);
		transform.position += (targetPos-transform.position)*0.1f;
		//transform.position = new Vector3((int)pos.x+dx, pos.y, (int)pos.z+dz);
		var angleY = 0f; 
		foreach (int angle in directions)
			if (Mathf.Abs(Mathf.DeltaAngle(eul.y, angle)) < 45) angleY = angle;
		/*var angleZ = 0f;
		if (standOn == "SLOPE") {
			if (Mathf.Abs(Mathf.DeltaAngle(eul.z, 45)) < 45) angleZ = 45;
			if (Mathf.Abs(Mathf.DeltaAngle(eul.z, 315)) < 45) angleZ = 315;
		}*/
		transform.eulerAngles = new Vector3(eul.x, angleY, eul.z);
		rb.velocity = new Vector3(0f, 0f, 0f);
		rb.angularVelocity = new Vector3(0f, 0f, 0f);
	}
	
	// Do the next command
	void NextCommand() 
	{
		var commands = Game.workspace.GetCommands();
		if (commandIndex >= commands.Count) 
		{
			commandRunning = false;
			SetState("DEAD", 50);
			return;
		}
		switch (commands[commandIndex]) 
		{
			case "blocky_move_forward": Move("FORWARD"); break;
			case "blocky_move_backward": Move("BACKWARD"); break;
			case "blocky_turn_left": Turn("LEFT"); break;
			case "blocky_turn_right": Turn("RIGHT"); break;
			default: break;
		}
		commandIndex += 1;
	}

	// On Collision
	void OnCollisionEnter(Collision collisionInfo) 
	{
		if (isFrozen() || isEnded()) return;
		Debug.Log("OnCollisionEnter = "+collisionInfo.collider.tag);
		switch (collisionInfo.collider.tag) 
		{
			case "Ground":
				standOn = "GROUND";
				if (state == "MOVE") SetState("COOL", 10);
				if (state == "TURN") SetState("COOL", 10);
				break;
			case "Stair":
				standOn = "STAIR";
				if (state == "MOVE") SetState("COOL", 10);
				if (state == "TURN") SetState("COOL", 10);
				break;
			case "Flag":
				SetState("FINISH", 50);
				Game.sound.play("WIN");
				break;
			/*
			case "Diamond":
				diamondNumber += 1;
				Game.level.diamondNumber.text = "Diamond: "+diamondNumber;
				Debug.Log("Diamond Touched");
				break;
			*/
			default:
				SetState("DEAD", 50);
				break;
		}
	}
	
}
