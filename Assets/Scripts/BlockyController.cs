using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockyController : MonoBehaviour {
	
	public enum State { STOP, COOL, FRONT, BACK, LEFT, RIGHT, DEAD, FINISH, RESET, MENU };
	public enum Direction { XP, ZP, XN, ZN };
	public Rigidbody rb;
	public GameManager gameManager;
	public State state;
	
	float timerDelay = 0.025f;
	int breathValue;
	int turnTarget;
	int countDown;
	Vector3 rotateAxis;
	Direction direction;
	Vector3 targetPos;
	Vector3 targetAngle;
	bool isRunning;
	int commandIndex;
	
	void Awake() { 
		Game.blocky = this;
	}
	
	void Start() {
		rb = GetComponent<Rigidbody>();
		InvokeRepeating("TimerTick", timerDelay, timerDelay);
		if (SceneManager.GetActiveScene().name == "Menu")
			ResetForMenu();
		else Reset();
	}
	
	public void Reset() {
		transform.position = new Vector3(0.5f, 0.25f, 0.5f);
		transform.eulerAngles = new Vector3(0, 0, 0);
		rb.velocity = new Vector3(0, 0, 0);
		breathValue = 1;
		countDown = 50;
		state = State.RESET;
		direction = Direction.XP;
		isRunning = false;
	}
	
	public void ResetForMenu() {
		breathValue = 1;
		state = State.MENU;
	}
	
	public bool isFrozen() {
		return (state == State.RESET || state == State.MENU);
	}
	
	void Update() {
		if (isFrozen()) return;
		if (state == State.STOP) {
			if (Input.GetKeyDown("up"))
				MoveForward();
			if (Input.GetKeyDown("down"))
				MoveBackward();
			if (Input.GetKeyDown("left"))
				TurnLeft();
			if (Input.GetKeyDown("right"))
				TurnRight();
		}
		if (transform.position.y < -10) {
			state = State.DEAD;
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
			case State.RESET:
				{ // Count down
					countDown -= 1;
					if (countDown < 0) {
						state = State.STOP;
					}
				}
				break;
			case State.FINISH:
				{ // Count down
					countDown -= 1;
					if (countDown < 0) Game.manager.CompleteLevel();
				}
				break;
			case State.DEAD:
				{ // Count down
					countDown -= 1;
					if (countDown < 0) Game.ReloadLevel(); //Game.manager.Restart();
				}
				break;
			case State.STOP:
				Correction();
				if (isRunning) RunCommand();
				break;
			case State.COOL:
				Correction();
				{ // Count down
					countDown -= 1;
					if (countDown < 0) state = State.STOP;
				}
				break;
			case State.FRONT:
			case State.BACK:
				if (rb.velocity.magnitude < 0.01) {
					countDown = 5;
					state = State.COOL;
				}
				break;
			case State.LEFT:
			case State.RIGHT:
				transform.RotateAround(transform.position, rotateAxis, timerDelay*200);
				if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, turnTarget)) < 2) {
					countDown = 5;
					state = State.COOL;
				}
				break;
		}
	}
	
	public void StartRunning() {
		commandIndex = 0;
		isRunning = true;
	}
	
	void RunCommand() {
		var commands = Game.workspace.GetCommands();
		if (commandIndex >= commands.Count) {
			isRunning = false;
			countDown = 50;
			state = State.DEAD;
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
	
	// Move forward 1 block
	public void MoveForward() {
		if (state != State.STOP) return;
		state = State.FRONT;
		switch (direction) {
			case Direction.XP: rb.velocity = new Vector3(2f, 2.4f, 0f); break;
			case Direction.ZP: rb.velocity = new Vector3(0f, 2.4f, 2f); break;
			case Direction.XN: rb.velocity = new Vector3(-2f, 2.4f, 0f); break;
			case Direction.ZN: rb.velocity = new Vector3(0f, 2.4f, -2f); break;
		}
	}
	
	// Move Backward 1 block
	public void MoveBackward() {
		if (state != State.STOP) return;
		state = State.BACK;
		switch (direction) {
			case Direction.XP: rb.velocity = new Vector3(-2f, 2.4f, 0f); break;
			case Direction.ZP: rb.velocity = new Vector3(0f, 2.4f, -2f); break;
			case Direction.XN: rb.velocity = new Vector3(2f, 2.4f, 0f); break;
			case Direction.ZN: rb.velocity = new Vector3(0f, 2.4f, 2f); break;
		}
	}
	
	// Turn left 90 degrees
	public void TurnLeft() {
		if (state != State.STOP) return;
		state = State.LEFT;
		rotateAxis = Vector3.down;
		rb.velocity = new Vector3(0f, 2.6f, 0f);
		switch (direction) {
			case Direction.XP: direction = Direction.ZP; turnTarget = 270; break;
			case Direction.ZP: direction = Direction.XN; turnTarget = 180; break;
			case Direction.XN: direction = Direction.ZN; turnTarget =  90; break;
			case Direction.ZN: direction = Direction.XP; turnTarget = 360; break;
		}
	}
	
	// Turn right 90 degrees
	public void TurnRight() {
		if (state != State.STOP) return;
		state = State.RIGHT;
		rotateAxis = Vector3.up;
		rb.velocity = new Vector3(0f, 2.6f, 0f);
		switch (direction) {
			case Direction.XP: direction = Direction.ZN; turnTarget =  90; break;
			case Direction.ZP: direction = Direction.XP; turnTarget = 360; break;
			case Direction.XN: direction = Direction.ZP; turnTarget = 270; break;
			case Direction.ZN: direction = Direction.XN; turnTarget = 180; break;
		}
	}
	
	// Correct position and rotation
	void Correction() { 
		Vector3 pos = transform.position;
		Vector3 eul = transform.eulerAngles;
		float dx = (pos.x < 0? -0.5f: +0.5f);
		float dz = (pos.z < 0? -0.5f: +0.5f);
		targetPos = new Vector3((int)pos.x+dx, pos.y, (int)pos.z+dz);
		transform.position += (targetPos-transform.position)*0.1f;
		int[] angles = { 90, 180, 270, 360 };
		foreach (int angle in angles)
			if (Mathf.Abs(Mathf.DeltaAngle(eul.y, angle)) < 45)
				transform.eulerAngles = new Vector3(0, angle, 0);
	}

	// On Collision
	void OnCollisionEnter(Collision collisionInfo) {
		if (isFrozen()) return;
		//Debug.Log("tag = "+collisionInfo.collider.tag);
		switch (collisionInfo.collider.tag) {
			case "Ground":
				break;
			case "Flag":
				if (state != State.DEAD) {
					countDown = 50;
					state = State.FINISH;
					Debug.Log("FINISH");
				}
				break;
			default:
				if (state != State.FINISH) {
					countDown = 50;
					state = State.DEAD;
					Debug.Log("DEAD");
				}
				break;
		}
	}
	
}
