using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private GameController game;
	// HOP
	public Vector2 hop;
	// JUMP
	public Vector2 jump;

	//STEP
	public Vector2 step;
	public float maxStep = 8f;
	bool grounded;


	public Transform startPos;
	public Transform endPos;


	private Rigidbody2D body;
	private Animator anim;
	// Use this for initialization
	void Start () {

		game = GameObject.FindObjectOfType<GameController> ();

		body = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		//SPAWN AT START
		if(transform.position.x >= endPos.position.x)
			{
				StartAgain ();
			}

		// DETECT INPUT
			if ((Input.GetKeyDown (KeyCode.Space)||Input.GetMouseButtonDown(0))&& grounded) {
			Jump ();
		}
			
	}
		
	void Jump()
	{
		if (body.velocity.y > 0){



			anim.Play ("jump");
			body.velocity = jump;
			grounded = false;
		}
	}
	void Hop()
	{

		anim.Play ("jump");
		body.velocity = hop;
	}
		
	void Die()
	{
		game.GameOver ();
		gameObject.SetActive (false);
	}

	void StartAgain()
	{
		if (hop.x < maxStep) {
			hop = hop + step;
			jump = jump + step * 2;
		}

		Vector2 pos = new Vector2 (startPos.position.x, transform.position.y);
		transform.position = pos;

		game.UpdateScore (30, "Around the Zoo", Color.green);

	}
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Ground" || (other.gameObject.tag == "Platform")) {

			grounded = true;
				
			Hop ();
		} 
			
		if (other.gameObject.tag == "Bullet") {
			Die ();
		}

	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Enemy") {

			if (game.hunted) {
				game.UpdateScore (150, "You crazy kangaroo!"); // THIS WILL BE DOUBLE
			} else {
				game.UpdateScore (50, "Revenge Kick!", Color.red);
			}

		}

	}
}
