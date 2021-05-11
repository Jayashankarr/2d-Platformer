using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class CharacterController : MonoBehaviour
{
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
    
    void Awake()
    { 
        collider = GetComponent<BoxCollider2D>();
        setUpRayCastBounds();
    }

    private void setUpRayCastBounds()
    {
        Bounds bounds = collider.bounds;

        rayCastInfo.TopLeft = new Vector2(bounds.max.x , bounds.max.y);
        rayCastInfo.TopRight = new Vector2(bounds.min.x , bounds.max.y);
        rayCastInfo.BottomLeft = new Vector2(bounds.min.x , bounds.min.y);
        rayCastInfo.BottomRight = new Vector2(bounds.max.x , bounds.min.y);
        verticalRaySpacing = bounds.size.y / verticalRaycastCount;
        horizontalRaySpacing = bounds.size.x / horizontalRaycastCount;
    }

    void Update()
    {
        
    }

    private void LateUpdate()
    {
        Vector2 velocity = Vector2.up * gravity;
        setUpRayCastBounds();
        updateVerticalCollision(ref velocity);
        transform.Translate(velocity);
    }

    private void updateCollisions(Vector2 deltaPos)
    {
        updateVerticalCollision(ref deltaPos);
        updateHorizontalCollisions();
    }

    private void updateVerticalCollision(ref Vector2 deltaPos)
    {
        VerticalDirection direction = (deltaPos.y > 0)? VerticalDirection.UP:VerticalDirection.DOWN;


        for(int i = 0; i < verticalRaycastCount; i++)
        {
            Vector2 ray = (direction == VerticalDirection.UP)? rayCastInfo.TopLeft:rayCastInfo.BottomLeft;


            ray += Vector2.up * (verticalRaySpacing * i);

            Debug.DrawRay(ray, Vector2.up * (int)direction,Color.red);

            RaycastHit2D hitInfo = Physics2D.Raycast(ray ,Vector2.up * (int)direction, 1f , collisionMask);

            if(hitInfo.collider != null && hitInfo.distance < 0.1f)
            {
                deltaPos.y = 0f;
            }
        }
    }

    private void updateHorizontalCollisions()
    {

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
