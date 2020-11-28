using UnityEngine;

public class Player : MonoBehaviour {
	public float moveSpeed = 7;
	public float smoothMoveTime = .1f;
	public float turnSpeed = 8;

	float angle;
	float smoothInputMagnitude;
	float smoothMoveVelocity;
	public Vector3 velocity;
	public bool isColliding;
	bool disabled;
	Vector2 savepoint;

	new Rigidbody rigidbody;

	void Start() {
		rigidbody = GetComponent<Rigidbody>();
		PointPlatform.OnPlayerWin += Disable;
		PointPlatform.OnGainPoint += UpdateSavepoint;
		Guard.OnPlayerSpotted += Spotted;
	}

	void Update() {
		// Receive player input or disable movement
		Vector3 inputDirection = Vector3.zero;
		if (!disabled) {
			inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
		}
		
		// Calculate multiplier to smooth velocity
		float inputMagnitude = inputDirection.magnitude;
		smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);

		// Calculate interpolated value
		float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
		angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);

		velocity = transform.forward * moveSpeed * smoothInputMagnitude;
	}

	void FixedUpdate() {
		// Move and rotate the rigidBody over fixed time so calculations are correct
		rigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
		rigidbody.MovePosition(rigidbody.position + velocity * Time.deltaTime);
	}

	void UpdateSavepoint(PointPlatform pointPlatform) {
		savepoint = new Vector2(pointPlatform.transform.position.x, pointPlatform.transform.position.z);
	}

	void Spotted() {
		if(GameObject.FindObjectOfType<GameUI>().isHardModeOn) {
			Disable();
		} else {
			ReturnToSavepoint();
		}
    }

	void Disable() {
		disabled = true;
    }

	void ReturnToSavepoint() {
		transform.position = new Vector3(savepoint.x, transform.position.y, savepoint.y);
    }

	private void OnTriggerEnter(Collider other) {
		savepoint = new Vector2(other.transform.position.x, other.transform.position.z);
	}


	void OnDestroy() {
		PointPlatform.OnPlayerWin -= Disable;
		PointPlatform.OnGainPoint -= UpdateSavepoint;
		Guard.OnPlayerSpotted -= Spotted;
	}
}
