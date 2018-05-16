using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

	private GameController game;
	public bool tippedOver;

	private Animator animTarget;
	// Use this for initialization
	void Start()
	{
		game = GameObject.FindObjectOfType<GameController> ();
		animTarget = GetComponentInChildren<Animator> ();


	}
	void OnTriggerEnter2D(Collider2D other)
	{

		if (other.gameObject.tag == "Bullet") {

			if (!tippedOver) {
				other.gameObject.GetComponent<Bullet> ().Die ();
				tippedOver = true;
				animTarget.SetBool ("alive", false);
			}
		}

		if (other.gameObject.tag == "Player"){

			if (tippedOver) {
				
				tippedOver = false;		
				game.UpdateScore (35, "Target Resurection");
				animTarget.SetBool ("alive", true);
			} else {
				game.UpdateScore (20, "Risky Business", Color.red);
			}

		}
	}
}
