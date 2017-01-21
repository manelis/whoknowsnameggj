using UnityEngine;
using System.Collections;

public class PlayerNew : MonoBehaviour {

	public enum playerState {floating, underwaterMovingDown, underwaterMovingUp, outsideMovingUp, outsideFaling};

	private playerState state = playerState.outsideFaling;

	public string playername;

	Rigidbody2D rigidbodyPlayer;

	//constants
	private float horizontalPlayerSpeed = 15;
	private float raycastRadius = 0.4f;
	private float gravity = 3.0f;
	private float buttonBaseIncrementSpeed = 0.8f; // also means that after this value in seconds stops affecting
	private float baseIncrementSpeedMultiplier = 20; //value multiplied to the speed when submerging
	private float goBackUpSpeedMultipler = 4;
	private float minMovingUpSpeed = 0.4f;

	//variables
	private float verticalSpeed = 0.0f;
	private float timeSinceSubmerssion = 0.0f;
	private float timeSinceLettingGo = 0.0f;

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

		RaycastHit2D raycastDown = Physics2D.Raycast (new Vector2 (transform.position.x, transform.position.y), new Vector2 (0, -1.0f),1000.0f, 1 << LayerMask.NameToLayer("MapWater"));
		RaycastHit2D raycastUp = Physics2D.Raycast (new Vector2 (transform.position.x, transform.position.y), new Vector2 (0, 1.0f),1000.0f, 1 << LayerMask.NameToLayer("MapWater"));

		float movementAxis = Input.GetAxis (horizontal_axis_name);
		bool buttonPressed = Input.GetButton (fire_button_name);
		bool buttonDownThisFrame = Input.GetButtonDown (fire_button_name);
		bool buttonUpThisFrame = Input.GetButtonUp (fire_button_name);
		bool nearSurface = raycastDown.distance < raycastRadius && raycastUp.distance == 0 || raycastUp.distance < raycastRadius && raycastDown.distance == 0;

		float sea_y = 0;
		if (raycastDown.collider != null) {
			sea_y = raycastDown.point.y;
		} else if (raycastUp.collider != null) {
			sea_y = raycastUp.point.y;	
		}

		float horizontal_increment = movementAxis * horizontalPlayerSpeed;

		//state machine
		if (state == playerState.floating) {

			transform.position = new Vector3 (transform.position.x, sea_y, transform.position.z);

			if (buttonDownThisFrame) {
				state = playerState.underwaterMovingDown;
			
				timeSinceSubmerssion = 0.0f;
			}

			if (raycastDown.distance > raycastRadius) { //came out of the water with speed
				state = playerState.outsideFaling;

				verticalSpeed = 0;
			}
		} 
		else if (state == playerState.underwaterMovingDown) {

			timeSinceSubmerssion += Time.deltaTime;

			transform.position += new Vector3 (0, -(buttonBaseIncrementSpeed - timeSinceSubmerssion)* baseIncrementSpeedMultiplier * Time.deltaTime);

			if (buttonUpThisFrame || buttonBaseIncrementSpeed < timeSinceSubmerssion) {

				state = playerState.underwaterMovingUp;

				timeSinceLettingGo = 0.0f;
				verticalSpeed = minMovingUpSpeed;
			}
		} 
		else if (state == playerState.underwaterMovingUp) {

			timeSinceLettingGo += Time.deltaTime;

			float speed = timeSinceLettingGo;
			//if (speed < minMovingUpSpeed)
			//	speed = minMovingUpSpeed;

			verticalSpeed += speed * goBackUpSpeedMultipler * Time.deltaTime;

			transform.position += new Vector3 (0, verticalSpeed, 0);

			if (nearSurface) {
				state = playerState.outsideMovingUp;
				verticalSpeed *= 1.2f;
			}
		}
		else if (state == playerState.outsideMovingUp) {

			verticalSpeed -= gravity * Time.deltaTime;

			transform.position += new Vector3 (0, verticalSpeed, 0);


			if (verticalSpeed < 0) {
				state = playerState.outsideFaling;
			}
		}
		else if (state == playerState.outsideFaling) {

			verticalSpeed -= gravity;
			float fallingIncrement = verticalSpeed * Time.deltaTime;
			if (fallingIncrement > raycastDown.distance)
				fallingIncrement = raycastDown.distance;

			transform.position += new Vector3 (0, fallingIncrement, 0);

			if (raycastDown.distance < raycastRadius) {
				state = playerState.floating;
			}
		}
			
		//apply player input
		transform.position += new Vector3 (horizontal_increment * Time.deltaTime, 0, 0);
	}

	public float getSpeed(){
		return verticalSpeed;
	}
}
