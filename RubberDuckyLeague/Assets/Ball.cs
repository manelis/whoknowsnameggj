﻿using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	Rigidbody2D self_rigidbody;

	// Use this for initialization
	void Start () {
		self_rigidbody = gameObject.GetComponent<Rigidbody2D> ();
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		PlayerNew player = collision.gameObject.GetComponent<PlayerNew> ();

		if (player != null) {
			Vector3 direction = transform.position - collision.gameObject.transform.position;
			self_rigidbody.constraints = RigidbodyConstraints2D.None;
			self_rigidbody.AddForce (new Vector2 (direction.x, direction.y) * 100 * player.getSpeed());
		}
		else if(collision.gameObject.layer == LayerMask.NameToLayer("MapWater")){
			self_rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name.Equals ("BasketLeft")) {
			GameObject.Find ("GameManager").GetComponent<Manager>().PlayerTwoScored();

		} 
		else
		{
			GameObject.Find ("GameManager").GetComponent<Manager>().PlayerOneScored();
		}

	}

	// Update is called once per frame
	void Update () {

		RaycastHit2D raycastDown = Physics2D.Raycast (new Vector2 (transform.position.x, transform.position.y), new Vector2 (0, -1.0f),1000.0f, 1 << LayerMask.NameToLayer("MapWater"));
	}
}
