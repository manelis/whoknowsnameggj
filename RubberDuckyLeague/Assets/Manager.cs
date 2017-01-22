﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

	int scorePlayer1;
	int scorePlayer2;

	public Text playerOneText;
	public Text playerTwoText;
	public Text playerScoredText;

	string playerOneString;
	string playerTwoString;

	public GameObject ball;
	public GameObject player1;
	public GameObject player2;

	private int maxScore = 5;
	private bool playerWon = false;

	Vector3 ballInitialPosition;
	Vector3 player1InitialPosition;
	Vector3 player2InitialPosition;

	float timeWhenScored;
	public float resetTime = 2.0f;
	bool scored;

	Color yellowColor = new Color(255f/255f, 221f/255f,85f/255f,255f/255f);
	Color greenColor = new Color(94f/255f,205f/255f,84f/255f,255f/255f);



	// Use this for initialization
	void Start () {
		scorePlayer1 = scorePlayer2 = 0;

		playerOneString = "";
		playerTwoString = "";


		ballInitialPosition = ball.transform.position;
		player1InitialPosition = player1.transform.position;
		player2InitialPosition = player2.transform.position;

		scored = false;

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.R)) 
		{
			ResetPositions ();
		}

		if (scored && (Time.time - timeWhenScored) >= resetTime && !playerWon) {
			ResetPositions ();
		}

		if (playerWon && Input.GetButtonDown("Submit")) {
			playerWon = false;
			ResetScore ();
			ResetPositions ();
		}
	}

	public void PlayerOneScored()
	{
		if (scored)
			return;

		playerOneText.text = playerOneString + (++scorePlayer1);
		playerScoredText.text = "Yellow scored!";
		playerScoredText.color = yellowColor;
		Score ();
	}

	public void PlayerTwoScored()
	{
		if (scored)
			return;
		
		playerTwoText.text = playerTwoString + (++scorePlayer2);
		playerScoredText.text = "Green scored!";
		playerScoredText.color = greenColor;
		Score ();

	}

	void Score(){

		checkWinningCondition ();

		playerScoredText.enabled = true;
		scored = true;
		timeWhenScored = Time.time;
	}

	void checkWinningCondition()
	{
		if (scorePlayer1 >= maxScore) 
		{
			playerWon = true;
			playerScoredText.text = "Yellow Wins!";
		}
		else if(scorePlayer2 >= maxScore)
		{
			playerScoredText.text = "Green Wins!";
			playerWon = true;
		}
	}

	void ResetScore()
	{
		scorePlayer2 = 0;
		scorePlayer1 = 0;
		playerTwoText.text = playerTwoString + scorePlayer2;
		playerOneText.text = playerOneString + scorePlayer1;
	}

	void ResetPositions()
	{
		ball.transform.position = ballInitialPosition;
		player1.transform.position = player1InitialPosition;
		player2.transform.position = player2InitialPosition;

		player1.GetComponent<PlayerNew> ().resetState ();
		player2.GetComponent<PlayerNew> ().resetState ();

		ball.GetComponent<Ball> ().resetState ();
		scored = false;

		playerScoredText.enabled = false;
	}

}
