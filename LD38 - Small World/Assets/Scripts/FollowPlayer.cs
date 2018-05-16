using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	private Player player;
	private Transform playerT;
	public Vector3 offset;

	public bool bounds;
	public Vector3 minCamPos;
	public Vector3 maxCamPos;
	// Use this for initialization
	void Start () {

		player = GameObject.FindObjectOfType<Player> ();
		playerT = player.GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 pos = new Vector3 (playerT.position.x, Mathf.Lerp(transform.position.y, playerT.position.y, Time.deltaTime * 3), transform.position.z) + offset;

		transform.position = pos;
	
		if (bounds) {

			transform.position = new Vector3 (
				Mathf.Clamp (transform.position.x, minCamPos.x, maxCamPos.x),
				Mathf.Clamp(transform.position.y, minCamPos.y, maxCamPos.y),
				Mathf.Clamp(transform.position.z, minCamPos.z, maxCamPos.z));


		}
	}
}
