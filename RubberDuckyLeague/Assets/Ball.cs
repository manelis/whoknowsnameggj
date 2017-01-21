using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	Rigidbody2D self_rigidbody;

	// Use this for initialization
	void Start () {
		self_rigidbody = gameObject.GetComponent<Rigidbody2D> ();
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.GetComponent<Player> () != null) {
			Vector3 direction = transform.position - collision.gameObject.transform.position;
			self_rigidbody.constraints = RigidbodyConstraints2D.None;
			self_rigidbody.AddForce (new Vector2 (direction.x, direction.y) * 500);
		}
		else if(collision.gameObject.layer == LayerMask.NameToLayer("MapWater")){
			self_rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
		}
	}

	// Update is called once per frame
	void Update () {

		RaycastHit2D raycastDown = Physics2D.Raycast (new Vector2 (transform.position.x, transform.position.y), new Vector2 (0, -1.0f),1000.0f, 1 << LayerMask.NameToLayer("MapWater"));
	}
}
