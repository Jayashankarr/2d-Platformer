using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float skinWidth = 0.05f;

    [SerializeField]
    private int verticalRaycastCount = 2;

    [SerializeField]
    private int horizontalRaycastCount = 2;

    [SerializeField]
    private LayerMask collisionMask;

    private BoxCollider2D boxCollider;

    private RayCastStartPositions rayCastInfo;

    private float horizontalRaySpacing;

    private float verticalRaySpacing;

    private bool isGrounded;

    private HorizontalDirection currentHorizonalCollision;

    private VerticalDirection currentVerticalCollision;

    private bool isReady = false;

    public bool IsGrounded
    {
        get{return isGrounded;}
    }
    private void Awake()
    { 
        boxCollider = GetComponent<BoxCollider2D>();
        setUpRayCastBounds();
    }

    private void Start()
    {
        isReady = true;
    }

    private void Reset()
    {
        currentHorizonalCollision = HorizontalDirection.NONE;
        currentVerticalCollision = VerticalDirection.NONE;
    }

    private void setUpRayCastBounds()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand (skinWidth * -2);

        rayCastInfo.TopLeft = new Vector2(bounds.min.x , bounds.max.y);
        rayCastInfo.TopRight = new Vector2(bounds.max.x , bounds.max.y);
        rayCastInfo.BottomLeft = new Vector2(bounds.min.x , bounds.min.y);
        rayCastInfo.BottomRight = new Vector2(bounds.max.x , bounds.min.y);
        verticalRaySpacing = bounds.size.x / (verticalRaycastCount - 1);
        horizontalRaySpacing = bounds.size.y / (horizontalRaycastCount - 1);
    }

    private void Update()
    {
        if(currentVerticalCollision == VerticalDirection.DOWN)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void Move(Vector2 moveDelta)
    {
        if(!isReady)
        {
            return;
        }
        setUpRayCastBounds();
        Reset();
        updateCollisions(ref moveDelta);

        Debug.Log("horizontal move : " + moveDelta.x );

        Debug.Log("celocity : " + moveDelta);

        transform.Translate(moveDelta);

    }

    private void updateCollisions(ref Vector2 deltaPos)
    {
        updateVerticalCollision(ref deltaPos);
        updateHorizontalCollisions(ref deltaPos);
    }

    private void updateVerticalCollision(ref Vector2 deltaPos)
    {
        VerticalDirection direction = (deltaPos.y > 0)? VerticalDirection.UP:VerticalDirection.DOWN;
        float rayLength = Mathf.Abs(deltaPos.y) + skinWidth;


        for(int i = 0; i < verticalRaycastCount; i++)
        {
            Vector2 ray = (direction == VerticalDirection.UP)? rayCastInfo.TopLeft:rayCastInfo.BottomLeft;


            ray += Vector2.right  * (verticalRaySpacing * i + deltaPos.x);

            Debug.DrawRay(ray, Vector2.up * (int)direction,Color.red);

            RaycastHit2D hitInfo = Physics2D.Raycast(ray ,Vector2.up * (int)direction, rayLength , collisionMask);

            if (hitInfo.collider != null) 
            {
				deltaPos.y = (hitInfo.distance - skinWidth) * (int)direction;
				rayLength = hitInfo.distance;

                currentVerticalCollision = direction;
			}
        }
    }

    private void updateHorizontalCollisions(ref Vector2 deltaPos)
    {
        HorizontalDirection direction = (deltaPos.x > 0)? HorizontalDirection.RIGHT:HorizontalDirection.LEFT;

        Debug.Log("Horizontal collision : " + direction);
        float rayLength = Mathf.Abs(deltaPos.x) + skinWidth;


        for(int i = 0; i < horizontalRaycastCount; i++)
        {
            Vector2 ray = (direction == HorizontalDirection.RIGHT)? rayCastInfo.BottomRight:rayCastInfo.BottomLeft;

            ray += Vector2.up  * (horizontalRaySpacing * i);

            Debug.DrawRay(ray, Vector2.right * (int)direction,Color.red);

            RaycastHit2D hitInfo = Physics2D.Raycast(ray ,Vector2.right * (int)direction, rayLength , collisionMask);

            if (hitInfo.collider != null) 
            {
                 Debug.Log("Collision direction : " + direction );
				deltaPos.x = (hitInfo.distance - skinWidth) * (int)direction;
				rayLength = hitInfo.distance;

                currentHorizonalCollision = direction;
			}
        }
    }

    private struct RayCastStartPositions
    {
        public Vector2 TopLeft;

        public Vector2 TopRight;

        public Vector2 BottomLeft;

        public Vector2 BottomRight;
    }

    

    private enum VerticalDirection
    {
        NONE = 0,

        UP = 1,

        DOWN = -1
    }

    private enum HorizontalDirection
    {
        NONE = 0,

        RIGHT = 1,

        LEFT = -1
    }
}
