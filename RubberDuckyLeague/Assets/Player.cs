using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public string playername;

	Rigidbody2D rigidbodyPlayer;

	public int playerSpeed = 15;

	// Use this for initialization
	void Start () {
	
		rigidbodyPlayer = GetComponent<Rigidbody2D> ();

	}
	
	// Update is called once per frame
	void Update () {
	
		RaycastHit2D raycastDown = Physics2D.Raycast (new Vector2 (transform.position.x, transform.position.y), new Vector2 (0, -1.0f),1000.0f, 1 << LayerMask.NameToLayer("MapWater"));
		RaycastHit2D raycastUp = Physics2D.Raycast (new Vector2 (transform.position.x, transform.position.y), new Vector2 (0, 1.0f),1000.0f, 1 << LayerMask.NameToLayer("MapWater"));

		float sea_y = 0;
		if (raycastDown.collider != null) {
			sea_y = raycastDown.point.y;
		} else if (raycastUp.collider != null) {
			sea_y = raycastUp.point.y;	
		}

		transform.position = new Vector3(transform.position.x, sea_y, transform.position.z);


		float movementAxis = 0;

		if (playername == "player1")
			movementAxis = Input.GetAxis ("Horizontal");
		else {
			movementAxis = Input.GetAxis ("Horizontal2");
		}

		float horizontal = movementAxis * Time.deltaTime * playerSpeed;
		transform.position = transform.position += new Vector3 (horizontal, 0, 0);

		///if (horizontal != 0) {


				//if (horizontal > 0) {
		//		rigidbodyPlayer.AddForce (new Vector2 (horizontal * 5, 0));
			
				//}
		//	}

	}

}
