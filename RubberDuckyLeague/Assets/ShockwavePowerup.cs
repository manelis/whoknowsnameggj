using UnityEngine;
using System.Collections;

public class ShockwavePowerup : MonoBehaviour {

	public Manager gameManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		PlayerNew player = other.gameObject.GetComponent<PlayerNew> ();

		if (player != null) {
			player.addPowerUp (this);

			this.GetComponent<SpriteRenderer> ().enabled = false;
			this.GetComponent<CircleCollider2D> ().enabled = false;
		}
	}
}
