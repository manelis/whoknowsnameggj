using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public string playername;

	Rigidbody2D rigidbodyPlayer;

	public int playerSpeed = 15;
	public int playerVerticalSpeed = 25;
	private float playerGravity = 3.5f;
	public float click_duration = 1;

	private float click_delta_time = 0;
	private float click_delta_time_total = 0;
	private float out_of_water_delta_time_total = 0;
	private bool clicked = false;

	private float constant_overwater_speed = 0;

	private bool previous_underwater = false;
	private bool underwater = false;
	private bool out_of_water = true;
	private bool floating = false;

	// Use this for initialization
	void Start () {
	
		rigidbodyPlayer = GetComponent<Rigidbody2D> ();

	}
	
	// Update is called once per frame
	void Update () {

		string horizontal_axis_name = "Horizontal";
		string fire_button_name = "Jump";

		if (playername == "player1")
		{
			horizontal_axis_name = "Horizontal";
			fire_button_name = "Jump";
		}
		else {
			horizontal_axis_name = "Horizontal2";
			fire_button_name = "Jump2";
		}

		float movementAxis = Input.GetAxis (horizontal_axis_name);
		bool fire_button_pressed = Input.GetButton (fire_button_name);

		if (clicked) {
			click_delta_time += Time.deltaTime;
		}

		if (click_delta_time > click_duration)
			click_delta_time = click_duration;

		RaycastHit2D raycastDown = Physics2D.Raycast (new Vector2 (transform.position.x, transform.position.y), new Vector2 (0, -1.0f),1000.0f, 1 << LayerMask.NameToLayer("MapWater"));
		RaycastHit2D raycastUp = Physics2D.Raycast (new Vector2 (transform.position.x, transform.position.y), new Vector2 (0, 1.0f),1000.0f, 1 << LayerMask.NameToLayer("MapWater"));

		underwater = raycastUp.distance > 0.2f;

		floating = raycastDown.distance < 0.4f && raycastUp.distance == 0 || raycastUp.distance < 0.4f && raycastDown.distance == 0;


		if (underwater) {
			//Debug.Log ("underwater");
			out_of_water = false;
			previous_underwater = true;
			click_delta_time_total += Time.deltaTime;
		}

		float underwater_float_speed = 50 * click_delta_time_total;

		if (out_of_water && raycastDown.distance < 0.2f && constant_overwater_speed < 0) {
			//Debug.Log ("in water");
			out_of_water = false;
			constant_overwater_speed = 0;
			out_of_water_delta_time_total = 0;
		}

		if (!underwater && (previous_underwater || !floating)) {
			//Debug.Log ("out of water");
			out_of_water = true;
			previous_underwater = false;
			constant_overwater_speed = underwater_float_speed;
			//Debug.Log (constant_overwater_speed);
		}

		if (out_of_water) {
			out_of_water_delta_time_total += Time.deltaTime;
		}

		//when you dive
		if (Input.GetButtonDown (fire_button_name) && !underwater) {
			click_delta_time_total = 0;
			clicked = true;
		}
		//when you release dive
		if (Input.GetButtonUp (fire_button_name)) {
			click_delta_time = 0;
			clicked = false;
		}

		if (!clicked)
			fire_button_pressed = false;

		float horizontal = movementAxis * Time.deltaTime * playerSpeed;
		float vertical = 0;


		float sea_y = 0;
		if (raycastDown.collider != null) {
			sea_y = raycastDown.point.y;
		} else if (raycastUp.collider != null) {
			sea_y = raycastUp.point.y;	
		}

		if(fire_button_pressed && !out_of_water) 
			vertical = -(click_duration - click_delta_time) * playerVerticalSpeed;
		if (underwater) {
			vertical += underwater_float_speed;
			vertical *= Time.deltaTime;
		} else
			vertical *= Time.deltaTime;
		if (out_of_water) {
			constant_overwater_speed -= playerGravity;

			vertical = constant_overwater_speed * Time.deltaTime;

			if (-vertical > raycastDown.distance)
				vertical = -raycastDown.distance ;
		}


		if(playername == "player1")
			Debug.Log (vertical);
	

		if (!out_of_water && vertical == 0 && floating)
			transform.position = new Vector3 (transform.position.x, sea_y, transform.position.z);
		else if (vertical < 0 || underwater) {
			transform.position = new Vector3 (transform.position.x, transform.position.y + vertical, transform.position.z);
		} else if (out_of_water) {
			transform.position = new Vector3 (transform.position.x, transform.position.y + vertical, transform.position.z);
		}

		transform.position = new Vector3 (transform.position.x + horizontal, transform.position.y, transform.position.z);

		///if (horizontal != 0) {


				//if (horizontal > 0) {
		//		rigidbodyPlayer.AddForce (new Vector2 (horizontal * 5, 0));
			
				//}
		//	}

	}

}
