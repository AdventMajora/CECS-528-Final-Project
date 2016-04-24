using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public int level = 1;
	public float maxLevelTime = 60f;
	public float levelStartTime = 2f;
	public float baseSpawnTime = 0.5f;
	public GameObject pauseMenu;

	private float timeLeft;
	private float timeElapsed = 0f;
	private bool gamePlaying = false;
	private GameObject levelText;
	private GameObject RedWinner;
	private GameObject BlueWinner;
	private GameObject TieWinner;
	private Text timeText;
	private thrower spawner;
	private GameObject endGameView;

	private Vector3[] originalPlayerPositions;

	// Use this for initialization
	void Start () 
	{
		spawner = GameObject.Find ("Thrower").GetComponent<thrower>();
		spawner.stopSpawning ();
		timeText = GameObject.Find("TimeText").GetComponent<Text>();
		levelText = GameObject.Find ("LevelText");
		levelText.SetActive (false);
		RedWinner = GameObject.Find ("Red_Winner");
		RedWinner.SetActive (false);
		BlueWinner = GameObject.Find ("Blue_Winner");
		BlueWinner.SetActive (false);
		TieWinner = GameObject.Find ("Tie_Winner");
		TieWinner.SetActive (false);

		endGameView = GameObject.Find ("EndGameText");
		endGameView.SetActive (false);

		beginLevel ();
	}

	void beginLevel()
	{
		// Destroy all thrown objects if they exist
		foreach (Transform child in GameObject.Find("Thrower").transform)
		{
			Destroy (child.gameObject);
		}

		// Reset time left
		timeLeft = maxLevelTime;

		// Set init time text
		timeText.text = "Time Left: " + Mathf.Round(timeLeft);

		// Activate object thrower after delay
		Invoke ("activateSpawner", levelStartTime);

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			pauseLevel();
		}
		if (gamePlaying)
		{
			spawner.spawnTime = (float)baseSpawnTime / (Mathf.Log ( (timeElapsed / 2) + 2));
			updateTime();
			if(timeLeft <= 0)
			{
				gamePlaying = false;
				endGame (0);
			}
		}
	}
	
	public void pauseLevel() {
		pauseMenu.SetActive(true);
		Time.timeScale = 0;
	}
	
	public void resumeLevel() {
		Time.timeScale = 1;
	}
	
	public void quit() {
		Application.Quit();
	}

	void finishLevel ()
	{
		// Stop throwing objects
		spawner.stopSpawning ();

		level++;
		beginLevel ();
	}

	private void updateTime()
	{
		timeLeft -= Time.deltaTime;
		timeElapsed += Time.deltaTime;

		// Display new time text
		timeText.text = "Time Left: " + timeLeft.ToString("F2");
	}

	// Update rate of objects thrown based on level
	public void activateSpawner()
	{
		// Display level text
		levelText.SetActive (true);
		Invoke ("disableLevelText", 2);
		gamePlaying = true;
		spawner.spawnTime = baseSpawnTime;
		spawner.beginSpawning ();
		// Should start timer as well
		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();
	}

	public void disableLevelText()
	{
		levelText.SetActive (false);
	}

	public void endGame(int winnerNumber)
	{
		CancelInvoke ();

		// Show new level text saying that you won or something.
		spawner.stopSpawning ();
		gamePlaying = false;

		endGameView.SetActive (true);
		if (winnerNumber != 0) {
			//GameObject.Find ("EndGameText").GetComponent<Text> ().text = "WINNER\nPlayer " + winnerNumber;
			if (winnerNumber == 1) {
				RedWinner.SetActive(true);
			} else {
				BlueWinner.SetActive(true);
			}
		} else {
			//GameObject.Find ("EndGameText").GetComponent<Text> ().text = "TIE";
			TieWinner.SetActive(true);
		}
		return;
	}

	public void restartGame ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		return;
	}

	public bool isGamePlaying() {
		return gamePlaying;
	}

	public void goToMainMenu() {
		print ("HAI");
		SceneManager.LoadScene ("opening");
	}
}