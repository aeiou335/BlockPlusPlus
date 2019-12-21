﻿using System.Collections;
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
	bool isRunning;
	float startHeight;
	bool isLanded;
	
	void Awake() 
	{ 
		Game.blocky = this;
	}
	
	void Start() 
	{
		InvokeRepeating("TimerTick", timerDelay, timerDelay);
		var pos = transform.position;
		var angle = transform.eulerAngles;
		resetPosition = new Vector3(pos.x, pos.y, pos.z);
		resetAngle = new Vector3(angle.x, angle.y, angle.z);
		foreach (var d in directions)
			if (Mathf.Abs(Mathf.DeltaAngle(angle.y, d)) < 45)
				resetDirection = d;
		Reset();
	}
	
	public void Reset() 
	{
		if (SceneManager.GetActiveScene().name == "Menu") 
		{
			breathValue = 1;
			SetState("MENU", 0);
			return;
		}
		rb = GetComponent<Rigidbody>();
		transform.position = resetPosition;
		transform.eulerAngles = resetAngle;
		direction = resetDirection;
		rb.velocity = Vector3.zero;
		breathValue = 1;
		isRunning = false;
		isLanded = true;
		SetState("RESET", 50);
	}
	
	void Update() 
	{
		if (isFrozen()) return;
		if (transform.position.y < -10) 
		{
			Debug.Log("position.y < -10");
			SetState("DEAD", 0);
		}
	}
	
	// Called every timer ticks
	void TimerTick() 
	{
		Breath();
		switch (state) 
		{
			case "RESET":
				if (--countDown < 0) SetState("STOP", 0);
				break;
			case "WIN":
				if (--countDown < 0) { SetState("END", 0); Game.level.CompleteLevel(); }
				break;
			case "DEAD":
				if (--countDown < 0) { SetState("END", 0); Game.level.FailLevel(); }
				break;
			case "STOP":
				Correction();
				if (isRunning) NextCommand();
				break;
			case "COOL":
				 Correction();
				if (--countDown < 0) SetState("STOP", 0);
				break;
			case "MOVE":
				CheckCollision();
				if (transform.position.y < startHeight) 
					rb.velocity = new Vector3(0, rb.velocity.y, 0);
				if (--countDown < 0) isLanded = false;
				break;
			case "SEND":
				DoorCorrection();
				break;
			case "TURN":
				if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, direction)) > 2) 
					transform.RotateAround(transform.position, turnAxis, timerDelay*200);
				if (--countDown < 0) isLanded = false;
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
		return (state == "END" || state == "DEAD" || state == "WIN");
	}
	
	// Start running commands
	public void Run() 
	{
		if (isFrozen() || isEnded()) return;
		isRunning = true;
	}
	
	// Set state
	void SetState(string _state, int _countDown) 
	{
		Debug.Log(_state);
		state = _state;
		countDown = _countDown;
	}
	
	// Move/Jump forward/backward 1 block
	void Move(string dir, string mode) 
	{
		if (state != "STOP") return;
		var signFront = 0;
		var signLeft = 0;
		if (dir == "FORWARD" || dir == "BACKWARD") 
		{
			signFront = (dir == "FORWARD"? 1: -1);
		} else {
			signLeft = (dir == "LEFT" ? 1 : -1);
		}
		//var sign = (dir == "FORWARD"? 1: -1);
		var vx = (mode == "JUMP"? 1.3f: 2.0f); 
		var vy = (mode == "JUMP"? 4.5f: 2.4f);
		switch (direction) 
		{
			case   0: rb.velocity = new Vector3(vx*signFront, vy, vx*signLeft); break;
			case  90: rb.velocity = new Vector3(vx*signLeft, vy, -vx*signFront); break;
			case 180: rb.velocity = new Vector3(-vx*signFront, vy, -vx*signLeft); break;
			case 270: rb.velocity = new Vector3(-vx*signLeft, vy, vx*signFront); break;
		}
		SetState("MOVE", 5);
		startHeight = transform.position.y;
		Game.sound.play("JUMP");
	}
	
	// Turn left/right 90 degrees
	void Turn(string dir) 
	{
		if (state != "STOP") return;
		SetState("TURN", 5);
		rb.velocity = new Vector3(0f, 2.6f, 0f);
		direction = (direction + (dir == "LEFT"? 270: 90)) % 360;
		turnAxis = (dir == "LEFT"? Vector3.down: Vector3.up);
		Game.sound.play("JUMP");
	}
	
	// Breath animation
	void Breath()
	{
		if (transform.localScale.x > 0.5+0.015) breathValue = -1;
		if (transform.localScale.x < 0.5-0.015) breathValue = +1;
		float delta = timerDelay*0.1f;
		Vector3 deltaVector = new Vector3(delta, -delta, delta);
		transform.localScale += deltaVector*breathValue;
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
		var angleY = 0f; 
		foreach (int angle in directions)
			if (Mathf.Abs(Mathf.DeltaAngle(eul.y, angle)) < 45) angleY = angle;
		transform.eulerAngles = new Vector3(eul.x, angleY, eul.z);
		rb.velocity = new Vector3(0f, 0f, 0f);
		rb.angularVelocity = new Vector3(0f, 0f, 0f);
	}
	
	// Check Collision for coin,diamond
	void CheckCollision()
	{
		foreach (var coin in Game.level.coins)
			if ((coin.transform.position - transform.position).magnitude < 0.5 && 
				coin.transform.localScale.magnitude > 1)
			{
				coin.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
				Game.level.Score("COIN");
			}
		foreach (var diamond in Game.level.diamonds)
			if ((diamond.transform.position - transform.position).magnitude < 0.5 && 
				diamond.transform.localScale.magnitude > 1)
			{
				diamond.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
				SetState("WIN", 50);
				Game.sound.play("WIN");
			}
	}

	void DoorCorrection()
	{
		Debug.Log(Game.level.portals[0].transform.position);
		Debug.Log(Game.level.portals[1].transform.position);
		//var new_pos = Game.level.portals[0].transform.position;
		var old_pos = Game.blocky.transform.position;
		foreach (var portal in Game.level.portals)
		{
			Debug.Log((portal.transform.position - transform.position).magnitude);
			if ((portal.transform.position - transform.position).magnitude > 0.5 )
			{
				var new_pos = portal.transform.position;
				transform.position = new Vector3(new_pos.x, old_pos.y, new_pos.z);
				break;
			}
		}		
		Correction();
	}
	
	// Do the next command
	void NextCommand() 
	{
		var command = Game.commands.Next();
		switch (command) 
		{
			case "blocky_move_forward":  Move("FORWARD", "MOVE"); break;
			case "blocky_move_backward": Move("BACKWARD", "MOVE"); break;
			case "blocky_turn_left":     Turn("LEFT"); break;
			case "blocky_turn_right":    Turn("RIGHT"); break;
			case "blocky_jump_forward":  Move("FORWARD", "JUMP"); break;
			case "blocky_jump_backward": Move("BACKWARD", "JUMP"); break;
			case "blocky_move_left": Move("LEFT", "MOVE"); break;
			case "blocky_move_right": Move("RIGHT", "MOVE"); break;
			case "<start>": break;
			case "<stop>":
				isRunning = false;
				SetState("DEAD", 50);
				break;
			default: break;
		}
	}

	// On Collision
	void OnCollisionEnter(Collision collision) 
	{
		if (isFrozen() || isEnded()) return;
		//Debug.Log("Collision = " + collision.collider.tag);
		switch (collision.collider.tag) 
		{
			case "Ground":
			case "Stair":
				if ((state == "MOVE" || state == "TURN") && !isLanded)
				{
					Debug.Log("Collision = Ground/Stair");
					isLanded = true;
					SetState("COOL", 10);
				}
				break;
			case "Flag": // will be replaced by diamond
				Debug.Log("Collision = Flag");
				SetState("WIN", 50);
				Game.sound.play("WIN");
				break;
			case "Door":
				if (state == "MOVE" || state == "TURN") SetState("COOL", 10);
				break;
			default:
				Debug.Log("Collision = Others");
				SetState("DEAD", 50);
				break;
		}
	}
	
	
	void OnTriggerEnter(Collider collider) 
	{			
		//Debug.Log(collider.tag);
		if (collider.tag == "Door")
		{
			DoorCorrection();
		}
		//OnCollisionEnter(collision);
	}
	
}
