using UnityEngine;

public class Player : MonoBehaviour 
{

    [SerializeField]
	private float maxJumpHeight = 4;

    [SerializeField]
	private float minJumpHeight = 1;

    [SerializeField]
	private float timeToJumpApex = .4f;

    [SerializeField]
	private CharacterController controller;

    [SerializeField]
	private float moveSpeed = 6;

	private float smoothTimeOnAir = .2f;

	private float smoothTimeOnGround = .1f;

	private float gravity;

	private float maxJumpVelocity;

	private float minJumpVelocity;

	private Vector3 velocity;

	private float velocityXSmoothing;

	private Vector2 horizontalMoveInput;

	private void Start() 
    {
        GameManager.Instance.Input.OnMoveBtn += onMoveInput;
        GameManager.Instance.Input.OnJumpBtnTapped += onJumpInput;

		controller = GetComponent<CharacterController> ();

		initializeValues();
	}

    private void initializeValues()
    {
        gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
    }

	private void Update() 
    {
		calculateVelocity ();

        if(controller != null)
        {
		    controller.Move (velocity * Time.deltaTime);
        }
	}

	private void onMoveInput (Vector2 input) 
    {
		horizontalMoveInput = input;
	}

	private void onJumpInput(bool isPressedDown) 
    {
        if(isPressedDown)
        {
            velocity.y = maxJumpVelocity;
        }
        else if(velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
	}

	private void calculateVelocity() 
    {
		float targetVelocityX = horizontalMoveInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.IsGrounded)?smoothTimeOnGround:smoothTimeOnAir);
		velocity.y += (gravity * Time.deltaTime);

        Debug.Log("celocity : " + velocity);
	}
}
