using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockyController : MonoBehaviour {
	
	public Rigidbody rb;
	public string state; // { STOP, COOL, FRONT, BACK, LEFT, RIGHT, DEAD, FINISH, RESET, MENU }
	public string direction; // { XP, ZP, XN, ZN }
	
	readonly float timerDelay = 0.025f;
	
	Vector3 resetPosition, resetAngle;
	string resetDirection;
	int countDown;
	int breathValue;
	int turnTarget;
	Vector3 turnAxis;
	bool commandRunning;
	int commandIndex;
	
	void Awake() { 
		Game.blocky = this;
	}
	
	void Start() {
		rb = GetComponent<Rigidbody>();
		InvokeRepeating("TimerTick", timerDelay, timerDelay);
		var pos = transform.position;
		var angle = transform.eulerAngles;
		resetPosition = new Vector3(pos.x, pos.y, pos.z);
		resetAngle = new Vector3(angle.x, angle.y, angle.z);
		if (Mathf.Abs(Mathf.DeltaAngle(angle.y,   0)) < 45) resetDirection = "XP";
		if (Mathf.Abs(Mathf.DeltaAngle(angle.y,  90)) < 45) resetDirection = "ZN";
		if (Mathf.Abs(Mathf.DeltaAngle(angle.y, 180)) < 45) resetDirection = "XN";
		if (Mathf.Abs(Mathf.DeltaAngle(angle.y, 270)) < 45) resetDirection = "ZP";
		if (SceneManager.GetActiveScene().name == "Menu") {
			breathValue = 1;
			state = "MENU";
		} else {
			Reset();
			Game.camera.Reset();
		}
	}
	
	public void Reset() {
		transform.position = resetPosition;
		transform.eulerAngles = resetAngle;
		direction = resetDirection;
		rb.velocity = Vector3.zero;
		breathValue = 1;
		commandRunning = false;
		countDown = 50;
		state = "RESET";
	}
	
	void Update() {
		if (isFrozen()) return;
		if (transform.position.y < -10) {
			state = "DEAD";
		}
	}
	
	// Called every timer ticks
	void TimerTick() {
		{ // Breath
			if (transform.localScale.x > 0.5+0.01) breathValue = -1;
			if (transform.localScale.x < 0.5-0.01) breathValue = +1;
			float delta = timerDelay*0.1f;
			Vector3 deltaVector = new Vector3(delta, -delta, delta);
			transform.localScale += deltaVector*breathValue;
		}
		switch (state) {
			case "RESET":
				if (--countDown < 0) state = "STOP";
				break;
			case "FINISH":
				if (--countDown < 0) Game.NextLevel();
				break;
			case "DEAD":
				if (--countDown < 0) Game.ReloadLevel();
				break;
			case "STOP":
				Correction();
				if (commandRunning) NextCommand();
				break;
			case "COOL":
				Correction();
				if (--countDown < 0) state = "STOP";
				break;
			case "FRONT":
			case "BACK":
				if (rb.velocity.magnitude < 0.01) {
					countDown = 5;
					state = "COOL";
				}
				break;
			case "LEFT":
			case "RIGHT":
				transform.RotateAround(transform.position, turnAxis, timerDelay*200);
				if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, turnTarget)) < 2) {
					countDown = 5;
					state = "COOL";
				}
				break;
		}
	}
	
	// is frozen?
	public bool isFrozen() {
		return (state == "RESET" || state == "MENU");
	}
	
	// Start running command
	public void CommandStart() {
		commandIndex = 0;
		commandRunning = true;
	}
	
	// Move forward 1 block
	void MoveForward() {
		if (state != "STOP") return;
		state = "FRONT";
		switch (direction) {
			case "XP": rb.velocity = new Vector3(2f, 2.4f, 0f); break;
			case "ZP": rb.velocity = new Vector3(0f, 2.4f, 2f); break;
			case "XN": rb.velocity = new Vector3(-2f, 2.4f, 0f); break;
			case "ZN": rb.velocity = new Vector3(0f, 2.4f, -2f); break;
		}
	}
	
	// Move Backward 1 block
	void MoveBackward() {
		if (state != "STOP") return;
		state = "BACK";
		switch (direction) {
			case "XP": rb.velocity = new Vector3(-2f, 2.4f, 0f); break;
			case "ZP": rb.velocity = new Vector3(0f, 2.4f, -2f); break;
			case "XN": rb.velocity = new Vector3(2f, 2.4f, 0f); break;
			case "ZN": rb.velocity = new Vector3(0f, 2.4f, 2f); break;
		}
	}
	
	// Turn left 90 degrees
	void TurnLeft() {
		if (state != "STOP") return;
		state = "LEFT";
		turnAxis = Vector3.down;
		rb.velocity = new Vector3(0f, 2.6f, 0f);
		switch (direction) {
			case "XP": direction = "ZP"; turnTarget = 270; break;
			case "ZP": direction = "XN"; turnTarget = 180; break;
			case "XN": direction = "ZN"; turnTarget =  90; break;
			case "ZN": direction = "XP"; turnTarget = 360; break;
		}
	}
	
	// Turn right 90 degrees
	void TurnRight() {
		if (state != "STOP") return;
		state = "RIGHT";
		turnAxis = Vector3.up;
		rb.velocity = new Vector3(0f, 2.6f, 0f);
		switch (direction) {
			case "XP": direction = "ZN"; turnTarget =  90; break;
			case "ZP": direction = "XP"; turnTarget = 360; break;
			case "XN": direction = "ZP"; turnTarget = 270; break;
			case "ZN": direction = "XN"; turnTarget = 180; break;
		}
	}
	
	// Correct position and rotation
	void Correction() { 
		Vector3 pos = transform.position;
		Vector3 eul = transform.eulerAngles;
		float dx = (pos.x < 0? -0.5f: +0.5f);
		float dz = (pos.z < 0? -0.5f: +0.5f);
		var targetPos = new Vector3((int)pos.x+dx, pos.y, (int)pos.z+dz);
		transform.position += (targetPos-transform.position)*0.1f;
		int[] angles = { 90, 180, 270, 360 };
		foreach (int angle in angles)
			if (Mathf.Abs(Mathf.DeltaAngle(eul.y, angle)) < 45)
				transform.eulerAngles = new Vector3(0, angle, 0);
	}
	
	// Do next command
	void NextCommand() {
		var commands = Game.workspace.GetCommands();
		if (commandIndex >= commands.Count) {
			commandRunning = false;
			countDown = 50;
			state = "DEAD";
			return;
		}
		switch (commands[commandIndex]) {
			case "blocky_move_forward": MoveForward(); break;
			case "blocky_move_backward": MoveBackward(); break;
			case "blocky_turn_left": TurnLeft(); break;
			case "blocky_turn_right": TurnRight(); break;
			default: break;
		}
		commandIndex += 1;
	}

	// On Collision
	void OnCollisionEnter(Collision collisionInfo) {
		if (isFrozen()) return;
		//Debug.Log("tag = "+collisionInfo.collider.tag);
		switch (collisionInfo.collider.tag) {
			case "Ground":
				break;
			case "Flag":
				if (state != "DEAD") {
					countDown = 50;
					state = "FINISH";
					Debug.Log("FINISH");
				}
				break;
			default:
				if (state != "FINISH") {
					countDown = 50;
					state = "DEAD";
					Debug.Log("DEAD");
				}
				break;
		}
	}
	
}
