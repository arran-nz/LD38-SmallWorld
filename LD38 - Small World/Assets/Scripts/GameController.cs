using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject UIParent;
	public GameObject scoreDescOBJ;
	public Text scoreText;


	public Animator alertText;
	public Animator huntedOVLY;
	private Animator textAnim;
	private string recentMsg;
	private int score;

	private int scoreMulti = 1;
	public Color MULTIPLIERColor;

	public bool hunted;


	 // MENU STUFF

	public Animator menu;
	public Animator bgMain;
	public Text hScoreTXT;
	public Text pScoreTXT;

	// SOUND
	public Image muteButtonIMG;
	public Sprite unmuteSprite;
	public Sprite muteSprite;
	// Use this for initialization
	void Start () {

		Time.timeScale = 0f;

		float[] prevScores = SaveLoadManager.LoadScore ();


		hScoreTXT.text = prevScores [0].ToString ();
		pScoreTXT.text = prevScores [1].ToString ();
	}
	void Update()
	{
		// DETECT INPUT
		if (Input.GetKeyDown (KeyCode.Space) && Time.timeScale == 0) {
			StartGame ();
		}
	}
	public void MuteToggle()
	{
		if (GetComponent<AudioSource> ().isPlaying) {
			GetComponent<AudioSource> ().Stop ();
			muteButtonIMG.sprite = muteSprite;
		} else {GetComponent<AudioSource> ().Play ();
			muteButtonIMG.sprite = unmuteSprite;
		}

	}
	public void StartGame()
	{
		Time.timeScale = 1f;
		menu.Play ("close");
		bgMain.Play ("mainBG");
	}
	public void GameOver()
	{
		UpdateScore(0, "Roo has been put down", Color.black);
		float[] prevScores = SaveLoadManager.LoadScore ();

		if (score > prevScores[0]) {
			prevScores [0] = score; // HIGH SCORE
		}

		prevScores [1] = score; // PREV SCORE

		SaveLoadManager.SaveScore (prevScores);
		StartCoroutine (RestartGame ());
			
	}
	IEnumerator RestartGame()
	{
		yield return new WaitForSeconds (2F);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	// ------------------- UPDATE SCORE OVERLOADS

	public void UpdateScore(int value, string desc, Color col)
	{
		score += value * scoreMulti;
		scoreText.text = ("SCORE " + score);

		Quaternion rot = new Quaternion (0, 0, 0, 0);
		Vector3 pos = new Vector3 (0, 0);
		GameObject scoreDesc = Instantiate (scoreDescOBJ, pos,rot,UIParent.transform);
		scoreDesc.transform.localPosition = pos;
		scoreDesc.GetComponentInChildren<Text> ().text = (desc + " +" + (value*scoreMulti).ToString ());

		if (scoreMulti > 1) {
			scoreDesc.GetComponentInChildren<Text> ().color = MULTIPLIERColor;
		} else {
			scoreDesc.GetComponentInChildren<Text> ().color = col;
		}

		Destroy (scoreDesc, 3);

	}

	public void UpdateScore(int value, string desc)
	{
		score += value * scoreMulti;
		scoreText.text = ("SCORE " + score);

		Quaternion rot = new Quaternion (0, 0, 0, 0);
		Vector3 pos = new Vector3 (0, 0);
		GameObject scoreDesc = Instantiate (scoreDescOBJ, pos,rot,UIParent.transform);
		scoreDesc.transform.localPosition = pos;
		scoreDesc.GetComponentInChildren<Text> ().text = (desc + " +" + (value*scoreMulti).ToString ());

		if (scoreMulti > 1) {
			scoreDesc.GetComponentInChildren<Text> ().color = MULTIPLIERColor;
		}
		Destroy (scoreDesc, 3);
	}

	// -------------------

	public void Hunter(bool prey)
	{
		hunted = prey;
		alertText.SetBool ("show", prey);
		huntedOVLY.SetBool ("on", prey);

		if (prey) {
			scoreMulti = 2;//d
		} 
		else
		{
			scoreMulti = 1;
		}
			
			

	}
}
