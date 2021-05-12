using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class CharacterController : MonoBehaviour
{
    public const float skinWidth = .015f;

    [SerializeField]
    private int verticalRaycastCount = 2;

    [SerializeField]
    private int horizontalRaycastCount = 2;

    [SerializeField]
    private float gravity = -0.0001f;

    [SerializeField]
    private LayerMask collisionMask;

    private BoxCollider2D collider;

    private RayCastPositions rayCastInfo;

    private float horizontalRaySpacing;

    private float verticalRaySpacing;

    private Vector2 velocity;
    
    private void Awake()
    { 
        collider = GetComponent<BoxCollider2D>();
        setUpRayCastBounds();
    }

    private void Start()
    {
        GameManager.Instance.Input.OnMoveBtn += onInput;
    }

    private void onInput(Vector2 velocity)
    {
        if(velocity.x > 0)
        {
            Debug.Log("press");
        }
        this.velocity = velocity *  0.1f;  
    }

    private void setUpRayCastBounds()
    {
        Bounds bounds = collider.bounds;

        rayCastInfo.TopLeft = new Vector2(bounds.min.x , bounds.max.y);
        rayCastInfo.TopRight = new Vector2(bounds.max.x , bounds.max.y);
        rayCastInfo.BottomLeft = new Vector2(bounds.min.x , bounds.min.y);
        rayCastInfo.BottomRight = new Vector2(bounds.max.x , bounds.min.y);
        verticalRaySpacing = bounds.size.x / (verticalRaycastCount - 1);
        horizontalRaySpacing = bounds.size.y / (horizontalRaycastCount - 1);
    }

    void Update()
    {
        
    }

    private void LateUpdate()
    {
        velocity.y +=  gravity;
        setUpRayCastBounds();
        updateVerticalCollision(ref velocity);
        transform.Translate(velocity);
    }

    private void updateCollisions(Vector2 deltaPos)
    {
        updateVerticalCollision(ref deltaPos);
        updateHorizontalCollisions(ref deltaPos);
    }

    private void updateVerticalCollision(ref Vector2 deltaPos)
    {
        VerticalDirection direction = (deltaPos.y > 0)? VerticalDirection.UP:VerticalDirection.DOWN;
        float rayLength = Mathf.Abs(deltaPos.y);


        for(int i = 0; i < verticalRaycastCount; i++)
        {
            Vector2 ray = (direction == VerticalDirection.UP)? rayCastInfo.TopLeft:rayCastInfo.BottomLeft;


            ray += Vector2.right  * (verticalRaySpacing * i);

            Debug.DrawRay(ray, Vector2.up * (int)direction,Color.red);

            RaycastHit2D hitInfo = Physics2D.Raycast(ray ,Vector2.up * (int)direction, rayLength , collisionMask);

            if (hitInfo.collider != null) 
            {
				deltaPos.y = (hitInfo.distance) * (int)direction;
				rayLength = hitInfo.distance;
			}
        }
    }

    private void updateHorizontalCollisions(ref Vector2 deltaPos)
    {
        HorizontalDirection direction = (deltaPos.x > 0)? HorizontalDirection.RIGHT:HorizontalDirection.LEFT;
        float rayLength = Mathf.Abs(deltaPos.x);


        for(int i = 0; i < verticalRaycastCount; i++)
        {
            Vector2 ray = (direction == HorizontalDirection.RIGHT)? rayCastInfo.TopLeft:rayCastInfo.BottomLeft;


            ray += Vector2.up  * (verticalRaySpacing * i);

            Debug.DrawRay(ray, Vector2.right * (int)direction,Color.red);

            RaycastHit2D hitInfo = Physics2D.Raycast(ray ,Vector2.right * (int)direction, rayLength , collisionMask);

            if (hitInfo.collider != null) 
            {
				deltaPos.y = (hitInfo.distance) * (int)direction;
				rayLength = hitInfo.distance;
			}
        }
    }

    private struct RayCastPositions
    {
        public Vector2 TopLeft;

        public Vector2 TopRight;

        public Vector2 BottomLeft;

        public Vector2 BottomRight;
    }

    private enum VerticalDirection
    {
        UP = 1,

        DOWN = -1
    }

    private enum HorizontalDirection
    {
        RIGHT = 1,

        LEFT = -1
    }
}
