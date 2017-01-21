using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

	int scorePlayer1;
	int scorePlayer2;

	public Text playerOneText;
	public Text playerTwoText;

	string playerOneString;
	string playerTwoString;

	public GameObject ball;
	public GameObject player1;
	public GameObject player2;

	Vector3 ballInitialPosition;
	Vector3 player1InitialPosition;
	Vector3 player2InitialPosition;

	float timeWhenScored;
	public float resetTime = 2.0f;
	bool scored;


	// Use this for initialization
	void Start () {
		scorePlayer1 = scorePlayer2 = 0;

		playerOneString = "Player1\nScore: ";
		playerTwoString = "Player1\nScore: ";


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
		};


		if (scored && (Time.time - timeWhenScored) >= resetTime) {
			ResetPositions ();
		}
	}

	public void PlayerOneScored()
	{
		playerOneText.text = playerOneString + (++scorePlayer1);
		Score ();
	}

	public void PlayerTwoScored()
	{
		playerTwoText.text = playerTwoString + (++scorePlayer2);
		Score ();

	}

	void Score(){
		if (scored)
			return;

		scored = true;
		timeWhenScored = Time.time;
	}

	void ResetPositions()
	{
		ball.transform.position = ballInitialPosition;
		player1.transform.position = player1InitialPosition;
		player2.transform.position = player2InitialPosition;

		player1.GetComponent<PlayerNew> ().resetState ();
		player2.GetComponent<PlayerNew> ().resetState ();

		//TODO: rotate players to face each other
		scored = false;
	}

}
