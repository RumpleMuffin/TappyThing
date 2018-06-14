using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMnager : MonoBehaviour {

	public delegate void GameDelegate();
	public static event GameDelegate OnGameStarted;
	public static event GameDelegate OnGameOverConfirmed;

	public static GameMnager Instance;

	public GameObject startPage;
	public GameObject gameOverPage;
	public GameObject countDownPage;
	public Text scoreText;
	public GameObject player;

	enum PageState
	{
		None,
		Start,
		GameOver,
		Countdown
	}

	int score = 0;
	bool gameOver = true;

	public bool GameOver { get { return gameOver; } }
	public int Score { get { return score; } }

	void Awake()
	{
		Instance = this;
	}

	private void OnEnable()
	{
		CountDown.OnCountdownFinished += OnCountdownFinished;
		TapController.OnPlayerDied += OnPlayerDied;
		TapController.OnPlayerScored += OnPlayerScored;
	}

	private void OnDisable()
	{
		CountDown.OnCountdownFinished -= OnCountdownFinished;
		TapController.OnPlayerScored -= OnPlayerScored;
		TapController.OnPlayerDied -= OnPlayerDied;
	}

	void OnCountdownFinished()
	{
		SetPageState(PageState.None);
		OnGameStarted(); //sent to tap controller
		score = 0;
		gameOver = false;
	}

	void OnPlayerDied()
	{
		gameOver = true;
		int savedScore = PlayerPrefs.GetInt("HighScore");
		if(score > savedScore)
		{
			PlayerPrefs.SetInt("HighScore", score);
		}
		SetPageState(PageState.GameOver);
	}

	void OnPlayerScored()
	{
		score++;
		scoreText.text = score.ToString();
	}

	void SetPageState(PageState state)
	{
		switch (state)
		{
			case PageState.None:
				startPage.SetActive(false);
				gameOverPage.SetActive(false);
				countDownPage.SetActive(false);
				break;

			case PageState.Start:
				startPage.SetActive(true);
				gameOverPage.SetActive(false);
				countDownPage.SetActive(false);
				break;

			case PageState.GameOver:
				startPage.SetActive(false);
				gameOverPage.SetActive(true);
				countDownPage.SetActive(false);
				break;

			case PageState.Countdown:
				startPage.SetActive(false);
				gameOverPage.SetActive(false);
				countDownPage.SetActive(true);
				break;
		}

		
	}

	public void ConfirmGameOver()
	{
		//activated when replay
		OnGameOverConfirmed();//event sent to tap controller
		scoreText.text = "0";
		SetPageState(PageState.Start);
	}

	public void StartGame()
	{
		//activated when play button hit at begining
		SetPageState(PageState.Countdown);
		Vector3 playerPos = player.GetComponent<TapController>().startPos;
		player.transform.position = playerPos;
	}
}
