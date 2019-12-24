using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockyController : MonoBehaviour 
{
	public Rigidbody rb;
	public string state; // { STOP, COOL, FRONT, BACK, LEFT, RIGHT, DEAD, FINISH, RESET, MENU }
	public int direction; // { XP, ZP, XN, ZN }
	public int characterNumber;
	
	readonly float timerDelay = 0.025f;
	readonly int[] directions = {0, 90, 180, 270};
	
	Vector3 resetPosition, resetAngle, resetScale, newPosition;
	int resetDirection;
	int countDown;
	int breathValue;
	Vector3 turnAxis;
	bool isRunning;
	float startHeight;
	bool isLanded;
	bool hasSent;
	
	void Awake() 
	{ 
		if (Game.characterNumber == characterNumber) Game.blocky = this;
	}
	
	void Start() 
	{
		InvokeRepeating("TimerTick", timerDelay, timerDelay);
		var pos = transform.position;
		var angle = transform.eulerAngles;
		var scale = transform.localScale;
		resetPosition = new Vector3(pos.x, pos.y, pos.z);
		resetAngle = new Vector3(angle.x, angle.y, angle.z);
		resetScale = new Vector3(scale.x, scale.y, scale.z);
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
		transform.localScale = resetScale;
		direction = resetDirection;
		rb.velocity = Vector3.zero;
		breathValue = 1;
		isRunning = false;
		hasSent = false;
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
		if (state != "SHRINK" || state != "ZOOM") Breath();
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
				if (!hasSent) 
				{
					DoorCollision();
					if (hasSent) break;
				}
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
				DoorCollision();
				break;
			case "TURN":
				if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, direction)) > 2) 
					transform.RotateAround(transform.position, turnAxis, timerDelay*200);
				if (--countDown < 0) isLanded = false;
				break;
			case "SHRINK":
				Correction();
				if (--countDown < 0) { SetState("ZOOM", 20); Game.sound.play("PORTAL"); }
				if (transform.localScale.x > 0.1f) 
					transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
				else transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
				break;
			case "ZOOM":
				Correction();
				if (--countDown < 0) SetState("COOL", 30); //NextCommand();
				if (transform.localScale.x < 0.5f) 
					transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
				else transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
				transform.position = newPosition;
				break;
			case "OPEN":
				if (--countDown < 0) SetState("COOL", 10);
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
		Debug.LogFormat("<color=blue>Number of puzzles = {0}.</color>", Game.commands.PuzzlesNumber());
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
		if (state != "STOP" && state != "ZOOM") return;
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
				Game.sound.play("COIN");
			}
		foreach (var diamond in Game.level.diamonds)
			if ((diamond.transform.position - transform.position).magnitude < 0.5 && 
				diamond.transform.localScale.magnitude > 1)
			{
				diamond.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
				SetState("WIN", 50);
				Game.sound.play("WIN");
			}
		foreach (var key in Game.level.keys)
			if ((key.transform.position - transform.position).magnitude < 0.5 && 
				key.transform.localScale.magnitude > 1)
			{
				key.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
				Game.level.Score("KEY");
				Game.sound.play("COIN");
			}
	}

	bool hasDoor()
	{
		Vector3 expectedDoorPosition = new Vector3(0.0f, 0.0f, 0.0f);
		Vector3 pos = transform.position;
		switch (direction) 
		{
			case   0: expectedDoorPosition = new Vector3(pos.x+1, pos.y, pos.z); break;
			case  90: expectedDoorPosition = new Vector3(pos.x, pos.y, pos.z-1); break;
			case 180: expectedDoorPosition = new Vector3(pos.x-1, pos.y, pos.z); break;
			case 270: expectedDoorPosition = new Vector3(pos.x, pos.y, pos.z+1); break;
		}
		foreach (var door in Game.level.doors)
		{
			if ((door.transform.position - expectedDoorPosition).magnitude < 0.5) 
			{
				door.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
				return true;
			}
		}
		return false;
	}
	void Open()
	{
		//rb.velocity = new Vector3(0f, 5f, 0f);
		if (Game.level.keyCount > 0 && hasDoor())
		{
			Game.level.Open("KEY");
		} 
		rb.velocity = new Vector3(0.0f, 3.0f, 0.0f);
		SetState("OPEN", 30);
		//Game.sound.play("JUMP");
	}

	void DoorCollision()
	{
		foreach (var portal in Game.level.portals)
		{
			if ((portal.transform.position - transform.position).magnitude < 0.5 ) 
			{
				Game.sound.play("PORTAL");
				hasSent = true;
			}
		}
		var old_pos = Game.blocky.transform.position;
		
		if (hasSent)
		{
			foreach (var portal in Game.level.portals)
			{
				//Debug.Log((portal.transform.position - transform.position).magnitude);	
				if ((portal.transform.position - transform.position).magnitude > 0.5 )
				{
					var new_pos = portal.transform.position;
					newPosition = new Vector3(new_pos.x, new_pos.y, new_pos.z);
					break;
				}
			}
			rb.velocity = new Vector3(0f, 0f, 0f);
			rb.angularVelocity = new Vector3(0f, 0f, 0f);
			SetState("SHRINK", 20);
		}
		
		/*
		foreach (var portal in Game.level.portals)
		{
			portal.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
		}
		*/
	}
	
	// Do the next command
	void NextCommand() 
	{
		var command = Game.commands.Next();
		hasSent = false;
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
			case "blocky_open_door": Open(); break;
			case "<start>": break;
			case "<stop>":
				isRunning = false;
				Debug.Log("End Of Commands");
				SetState("DEAD", 50);
				break;
			default: break;
		}
		Debug.Log(command);
		/*
		if (command == "blocky_move_forward" || command == "blocky_move_backward" ||
			command == "blocky_jump_forward" || command == "blocky_jump_backward" ||
			command == "blocky_move_left" || command == "blocky_move_right")
		{
			Debug.Log("isSent resets.");
			isSent = false;
		}
		*/
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
			/*
			case "Door":
				if (state == "MOVE" || state == "TURN") SetState("COOL", 10);
				break;
			*/
			default:
				Debug.Log("Collision = Others");
				SetState("DEAD", 50);
				break;
		}
	}
	
	/*
	void OnTriggerEnter(Collider collider) 
	{			
		if (collider.tag == "Door" && !isSent)
		{
			Debug.Log(collider.tag);
			DoorCollision();
			isSent = true;
		}
		//OnCollisionEnter(collision);
	}
	*/
}
