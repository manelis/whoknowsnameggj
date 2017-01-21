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

	// Use this for initialization
	void Start () {
		scorePlayer1 = scorePlayer2 = 0;

		playerOneString = "Player1\nScore: ";
		playerTwoString = "Player1\nScore: ";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayerOneScored()
	{
		playerOneText.text = playerOneString + (++scorePlayer1);
	}

	public void PlayerTwoScored()
	{
		playerTwoText.text = playerTwoString + (++scorePlayer2);
	}

}
