using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public string playername;

	Rigidbody2D rigidbodyPlayer;

	public int playerSpeed = 15;
	public int playerVerticalSpeed = 30;
	public float click_duration = 1;

	private float click_delta_time = 0;
	private bool clicked = false;

	// Use this for initialization
	void Start () {
	
		rigidbodyPlayer = GetComponent<Rigidbody2D> ();

	}
	
	// Update is called once per frame
	void Update () {

		float movementAxis = 0;
		bool fire_button_pressed = false;

		if (clicked)
			click_delta_time += Time.deltaTime;

		if (click_delta_time > click_duration)
			click_delta_time = click_duration;

		if (Input.GetButtonDown ("Jump"))
			clicked = true;
		if (Input.GetButtonUp ("Jump")) {
			click_delta_time = 0;
			clicked = false;
		}
		
		if (playername == "player1")
		{
			movementAxis = Input.GetAxis ("Horizontal");
			fire_button_pressed = Input.GetButton ("Jump");
		}
		else {
			movementAxis = Input.GetAxis ("Horizontal2");
			fire_button_pressed = Input.GetButton ("Jump2");
		}

		float horizontal = movementAxis * Time.deltaTime * playerSpeed;
		float vertical = 0;

		if(fire_button_pressed) 
			vertical = -(click_duration - click_delta_time) * playerVerticalSpeed;

		RaycastHit2D raycastDown = Physics2D.Raycast (new Vector2 (transform.position.x, transform.position.y), new Vector2 (0, -1.0f),1000.0f, 1 << LayerMask.NameToLayer("MapWater"));
		RaycastHit2D raycastUp = Physics2D.Raycast (new Vector2 (transform.position.x, transform.position.y), new Vector2 (0, 1.0f),1000.0f, 1 << LayerMask.NameToLayer("MapWater"));

		bool underwater = raycastUp.distance > 0f;

		float sea_y = 0;
		if (raycastDown.collider != null) {
			sea_y = raycastDown.point.y;
		} else if (raycastUp.collider != null) {
			sea_y = raycastUp.point.y;	
		}

		if (underwater)
			vertical += 10;
		Debug.Log (vertical);
		if (vertical == 0 && (raycastDown.distance < 0.4f && raycastUp.distance == 0 || raycastUp.distance < 0.4f && raycastDown.distance == 0))
			transform.position = new Vector3 (transform.position.x + horizontal, sea_y, transform.position.z);
		else if(vertical < 0 || underwater){
			transform.position = new Vector3 (transform.position.x + horizontal, transform.position.y + vertical*Time.deltaTime, transform.position.z);
		}

		///if (horizontal != 0) {


				//if (horizontal > 0) {
		//		rigidbodyPlayer.AddForce (new Vector2 (horizontal * 5, 0));
			
				//}
		//	}

	}

}
