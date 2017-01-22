using UnityEngine;
using System.Collections;

public class PlayerNew : MonoBehaviour {

	public enum playerState {floating, underwaterMovingDown, underwaterMovingUp, outsideMovingUp, outsideFaling};

	private playerState state = playerState.outsideFaling;

	public string playername;

    public ParticleSystem splash;

	Rigidbody2D rigidbodyPlayer;

	//constants
	private float horizontalPlayerSpeed;
	private float raycastRadius;
	private float gravity;
	private float buttonBaseIncrementSpeed; // also means that after this value in seconds stops affecting
	private float baseIncrementSpeedMultiplier; //value multiplied to the speed when submerging
	private float goBackUpSpeedMultipler;
	private float minMovingUpSpeed;
	private float powerupForceDistance = 8.0f;

	private float collisionDisableTime;

	//variables
	private float verticalSpeed ;
	private float timeSinceSubmerssion;
	private float timeSinceLettingGo;
	private float timeSinceCollisionDisable;
	private bool isCollisionsEnabled;

	private float timeSinceShockwave;
	private float shockwaveDisplayTime;

	private int lastside;

	private Vector3 previousPosition;

	private ShockwavePowerup powerup;

	private Manager manager;

	private GameObject shockwaveEffect;

	public void resetState(){

		horizontalPlayerSpeed = 15;
		raycastRadius = 0.4f;
		gravity = 3.0f;
		buttonBaseIncrementSpeed = 0.7f; // also means that after this value in seconds stops affecting
		baseIncrementSpeedMultiplier = 30; //value multiplied to the speed when submerging
		goBackUpSpeedMultipler = 4;
		minMovingUpSpeed = 0.4f;

		collisionDisableTime = 1.0f;

		//variables
		verticalSpeed = 0.0f;
		timeSinceSubmerssion = 0.0f;
		timeSinceLettingGo = 0.0f;
		timeSinceCollisionDisable = 0.0f;
		isCollisionsEnabled = true;
		timeSinceShockwave = 0.0f;
		shockwaveDisplayTime = 0.3f;

		lastside = 1;
		if (playername == "player2")
			lastside = -1;

		previousPosition = new Vector3(0,0,0);

		state = playerState.outsideFaling;
		powerup = null;

		manager = GameObject.Find ("GameManager").GetComponent<Manager> ();
		shockwaveEffect = transform.FindChild ("ShockwaveEffect").gameObject;
	}


	// Use this for initialization
	void Start () {
		resetState ();
		rigidbodyPlayer = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		string horizontal_axis_name = "";
		string fire_button_name = "";
		string shockwave_button_name = "";

		if (playername == "player1")
		{
			horizontal_axis_name = "Horizontal";
			fire_button_name = "Jump";
			shockwave_button_name = "Shockwave";
		}
		else {
			horizontal_axis_name = "Horizontal2";
			fire_button_name = "Jump2";
			shockwave_button_name = "Shockwave2";
		}

		RaycastHit2D raycastDown = Physics2D.Raycast (new Vector2 (transform.position.x, transform.position.y), new Vector2 (0, -1.0f),1000.0f, 1 << LayerMask.NameToLayer("MapWater"));
		RaycastHit2D raycastUp = Physics2D.Raycast (new Vector2 (transform.position.x, transform.position.y), new Vector2 (0, 1.0f),1000.0f, 1 << LayerMask.NameToLayer("MapWater"));
	
		float movementAxis = Input.GetAxis (horizontal_axis_name);
		bool buttonPressed = Input.GetButton (fire_button_name);
		bool buttonDownThisFrame = Input.GetButtonDown (fire_button_name);
		bool buttonUpThisFrame = Input.GetButtonUp (fire_button_name);
		bool shockwavePressed = Input.GetButton (shockwave_button_name);
		bool nearSurface = raycastDown.distance < raycastRadius && raycastUp.distance == 0 || raycastUp.distance < raycastRadius && raycastDown.distance == 0;

		float sea_y = 0;
		if (raycastDown.collider != null) {
			sea_y = raycastDown.point.y;
		} else if (raycastUp.collider != null) {
			sea_y = raycastUp.point.y;	
		}

		float horizontal_increment = movementAxis * horizontalPlayerSpeed;

		previousPosition = transform.position;

		//state machine
		if (state == playerState.floating) {

			transform.position = new Vector3 (transform.position.x, sea_y, transform.position.z);

			if (buttonPressed) {
				state = playerState.underwaterMovingDown;
			
				verticalSpeed = 0;
				timeSinceSubmerssion = 0.0f;
			}

			if (raycastDown.distance > raycastRadius) { //came out of the water with speed
				state = playerState.outsideFaling;

				verticalSpeed = 0;
			}
		} 
		else if (state == playerState.underwaterMovingDown) {

			timeSinceSubmerssion += Time.fixedDeltaTime;

			transform.position += new Vector3 (0, -(buttonBaseIncrementSpeed - timeSinceSubmerssion)* baseIncrementSpeedMultiplier * Time.fixedDeltaTime);

			if (!buttonPressed || buttonBaseIncrementSpeed < timeSinceSubmerssion) {

				state = playerState.underwaterMovingUp;

				timeSinceLettingGo = 0.0f;
				verticalSpeed = minMovingUpSpeed;
			}
		} 
		else if (state == playerState.underwaterMovingUp) {

			timeSinceLettingGo += Time.fixedDeltaTime;

			float speed = timeSinceLettingGo;
			//if (speed < minMovingUpSpeed)
			//	speed = minMovingUpSpeed;

			verticalSpeed += speed * goBackUpSpeedMultipler * Time.fixedDeltaTime;

			transform.position += new Vector3 (0, verticalSpeed, 0);

			if (nearSurface) {
				state = playerState.outsideMovingUp;
				verticalSpeed *= 1.8f;
			}
		}
		else if (state == playerState.outsideMovingUp) {

			verticalSpeed -= gravity * Time.fixedDeltaTime;

			transform.position += new Vector3 (0, verticalSpeed, 0);


			if (verticalSpeed < 0) {
				state = playerState.outsideFaling;
			}
		}
		else if (state == playerState.outsideFaling) {

			verticalSpeed -= gravity;
			float fallingIncrement = verticalSpeed * Time.fixedDeltaTime;
			if (fallingIncrement > raycastDown.distance)
				fallingIncrement = raycastDown.distance;

			transform.position += new Vector3 (0, fallingIncrement, 0);

			if (raycastDown.distance < raycastRadius) {
                ParticleSystem nice = Instantiate(splash, transform.position+(new Vector3(0,1,0)), transform.rotation) as ParticleSystem;
                nice.Play();
                Destroy(nice.gameObject, 1);
				state = playerState.floating;
			}
		}
			
		//shockwave
		if (powerup != null && shockwavePressed) {
			useShockwave ();
		}
		timeSinceShockwave += Time.deltaTime;
		if (timeSinceShockwave > shockwaveDisplayTime)
			shockwaveEffect.SetActive (false);


		//apply player input
		int wallCollision = getWallCollision();
		if (horizontal_increment > 0 && wallCollision > 0)
			horizontal_increment = 0;
		if (horizontal_increment < 0 && wallCollision < 0)
			horizontal_increment = 0;		

		transform.position += new Vector3 (horizontal_increment * Time.fixedDeltaTime, 0, 0);

		if (horizontal_increment > 0)
			lastside = 1;
		else if (horizontal_increment < 0)
			lastside = -1;

		Vector3 directionVector = transform.position - previousPosition;
		if (lastside == -1) {
			//directionVector = -directionVector;
			this.GetComponent<SpriteRenderer> ().flipY = true;
		}
		else
			this.GetComponent<SpriteRenderer> ().flipY = false;

		if (Mathf.Abs(horizontal_increment) < 0.2f && state == playerState.floating) {
			directionVector = new Vector3 (lastside, 0, 0);
			if(lastside == -1)
				this.GetComponent<SpriteRenderer> ().flipY = false;
					
		}

		transform.right = directionVector;

		timeSinceCollisionDisable += Time.fixedDeltaTime;
		if (timeSinceCollisionDisable >= collisionDisableTime)
			isCollisionsEnabled = true;
	}
		
	public float getSpeed(){
		return (transform.position - previousPosition).magnitude;
	}

	public Vector3 getDirection(){
		return transform.position - previousPosition;
	}

	public int getWallCollision(){

		RaycastHit2D raycastLeft = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(-1.0f, 0), 1000.0f, 1 << LayerMask.NameToLayer("Obstacle"));
		RaycastHit2D raycastRight = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(1.0f, 0), 1000.0f, 1 << LayerMask.NameToLayer("Obstacle"));

		int side_blocked = 0;
		if (raycastLeft.distance < 0.8f) side_blocked = -1;
		if (raycastRight.distance < 0.8f) side_blocked = 1;

		return side_blocked;
	}

	public bool collisionsEnabled(){
		return isCollisionsEnabled;
	}

	public void disableCollisions(){
		timeSinceCollisionDisable = 0.0f;
		//isCollisionsEnabled = false;
	}

	public void addPowerUp(ShockwavePowerup pickup){

		powerup = pickup;

		if (playername == "player1") {
			manager.player1Shockwave.enabled = true;
			manager.player2Shockwave.enabled = false;
		} else {
			manager.player1Shockwave.enabled = false;
			manager.player2Shockwave.enabled = true;
		}
	}

	public void useShockwave(){
		timeSinceShockwave = 0.0f; 
		powerup = null;
		manager.shockWaveUsed ();

		if (playername == "player1") {
			manager.player1Shockwave.enabled = false;
		} else {
			manager.player2Shockwave.enabled = false;
		}

		shockwaveEffect.SetActive (true);

		GameObject ball = GameObject.Find ("ball");

		if (ball != null) {
			float distance = (ball.transform.position - transform.position).magnitude;
			Vector3 direction = (ball.transform.position - transform.position).normalized;
			if (distance < powerupForceDistance) {

				float force = 10 * (powerupForceDistance - distance);
				Debug.Log (force);


				ball.GetComponent<Rigidbody2D>().AddForce (direction * force,ForceMode2D.Impulse);
				disableCollisions ();
			}

		}

	}
}


