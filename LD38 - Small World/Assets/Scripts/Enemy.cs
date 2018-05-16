using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private GameController game;

	public GameObject targetContainer;
	public Target[] targets;
	public Transform playerTransform;
	private Transform selectedTarget;

	public Transform arm;
	public GameObject gun;
	public GameObject bullet;

	public float reactionTime = 3F;
	public Vector2 shootFreq; // X = MIN, Y == MAX

	[Range(0, 3)]
	public float drunkness = 2;
	public float bulletSpeed = 10F;
	public float bulletDecay = 3f;

	private float drunknessGen = 0f;

	private SpriteRenderer gunSprite;
	private SpriteRenderer armSprite;
	private SpriteRenderer muzzleFlash;
	public Animator lineOfFire;
	private float time;
	// Use this for initialization
	void Start () {

		game = GameObject.FindObjectOfType<GameController> ();

		time = Random.Range (-1, -5); // Starts randomly
		armSprite = arm.GetComponent<SpriteRenderer> ();

		SpriteRenderer[] array = gun.GetComponentsInChildren<SpriteRenderer> ();
		gunSprite = array [0];
		muzzleFlash = array [1];

		// SET UP TARGETS
		targets = targetContainer.GetComponentsInChildren<Target>();
		ChooseTarget ();
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;

		float randFreq;
		if (selectedTarget == playerTransform) {
			randFreq = Random.Range (shootFreq.x / 1.5F, shootFreq.y / 1.5F); // SHOOT MORE WHEN HUNTING
		} else {
			randFreq = Random.Range (shootFreq.x, shootFreq.y); // NORMAL RANGE FOR TARGETS
		}

		if (time > randFreq) 
		{
			ChooseTarget ();
			drunknessGen = Random.Range (-drunkness, drunkness);
			StartCoroutine (Aim ());
			time = 0;
		}
		PointAtTarget (selectedTarget);

	}
		
	List<Transform> upArray = new List<Transform>();
	void ChooseTarget()
	{
		upArray.Clear ();

		for (int i = 0; i < targets.Length; i++) {
			if (!targets [i].tippedOver) {
				upArray.Add (targets[i].transform);
			}
		}
			

		if (upArray.Count > 0) { // IF ANY TAGET AVAL

			int newTarget = Random.Range (0, upArray.Count);
			selectedTarget = upArray[newTarget].transform;
			game.Hunter (false);


		} else { // OR SHOOT PLAYER
			
			selectedTarget = playerTransform;
			game.Hunter (true);

		}
	}
		
	IEnumerator Aim()
	{
		lineOfFire.Play ("aim");
		yield return new WaitForSeconds (1.5f);
		FireBullet ();
	}
	void FireBullet()
	{
		muzzleFlash.enabled = true;
		GameObject clone = Instantiate (bullet,	gun.transform.position, gun.transform.rotation);
		clone.GetComponent<Rigidbody2D> ().velocity = bulletSpeed * transform.localScale.x * clone.transform.right;
		StartCoroutine (DestroyBullet (clone));
	}
	IEnumerator DestroyBullet(GameObject bulletClone)
	{
		yield return new WaitForSeconds (0.1F);
		muzzleFlash.enabled = false;
		yield return new WaitForSeconds(bulletDecay);
		Destroy (bulletClone);
	}

	void PointAtTarget(Transform target)
	{

		Vector3 vectorToTarget = target.position - transform.position;
		float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.AngleAxis(angle + drunknessGen , Vector3.forward);
		arm.rotation = Quaternion.Slerp(arm.rotation, q, Time.deltaTime * reactionTime);


		if ((angle < 90)&&(angle > -90)) {
			gunSprite.flipY = false;
			armSprite.flipY = false;
		} 
		else {
			gunSprite.flipY = true;
			armSprite.flipY = true;
		}
	}
}
