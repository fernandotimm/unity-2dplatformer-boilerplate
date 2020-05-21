using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float jumpForce = 1000f; // Amount of force for the player jump
	[Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f; // Fine tunning for smoothing out the movements
	[SerializeField] private LayerMask groundSelection; // A mask that contains the layers that should behave as ground for the player
	[SerializeField] private Transform groundDetector; // A GameObject inside the Player marking where to check if it's grounded

	const float groundedRadius = .2f; // The radius of the circle to detect if the player is grounded
	private bool isGrounded; // Returns if the player is grounded or not
	
	private Rigidbody2D rigidbody2D;
	private bool isFacingRight = true; // Returns the side the player is currently facing
	private Vector3 velocity = Vector3.zero;

	private void Awake() {
		rigidbody2D = GetComponent<Rigidbody2D>();

	}

	private void FixedUpdate() {
		bool wasGrounded = isGrounded;
		isGrounded = false;

		// The player is grounded if the circle of gameObject ground-check hits an object (defined as Layer set on groundSelection) behaving as ground for the player
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundDetector.position, groundedRadius, groundSelection);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject) {
				isGrounded = true;
			}
		}
	}


	public void Move(float move, bool jump) {
		//only control the player if it is grounded
		if (isGrounded) {
			// Move the player based on the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, rigidbody2D.velocity.y);
			// ...and then smoothing it out and applying it to the player
			rigidbody2D.velocity = Vector3.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);

			// If the player is moving right and is facing left...
			if (move > 0 && !isFacingRight)
			{
				// ... flip the player
				Flip();
			}
			// Otherwise, if the player is moving left and is facing right...
			else if (move < 0 && isFacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player jumps...
		if (isGrounded && jump)
		{
			// Set a vertical force to it
			isGrounded = false;
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
		}
	}


	private void Flip() {
		// Switch the way the player is facing
		isFacingRight = !isFacingRight;

		// Invert the x local scale of the player by multiplying by -1
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}