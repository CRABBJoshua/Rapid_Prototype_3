using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEngine.ParticleSystem;


public class Player : PlayerMotor
{
    [Header("Base Settings")]
    public bool disableInput;
    public bool disableSkinRotation;
    public bool disableCameraFollow;

    [Header("Base Components")]
    public PlayerInput input;

    [Header("Base Scriptables")]
    public PlayerStats stats;

    private int horizontalSpeedHash;
    private int animationSpeedHash;
    private int stateHash;
    private int groundedHash;

    public bool attacking { get; set; }
    public bool lookingDown { get; set; }
    public bool lookingUp { get; set; }
    public bool halfGravity { get; set; }
    public bool invincible { get; set; }

    public float invincibleTimer { get; set; }
    public int direction { get; private set; }

    [Header("Portals")]
    public GameObject Portal_1;
    public GameObject Portal_2;
    public int PortalIndex = 0;

    Camera cam;

    private GameObject curPortal1;
    private GameObject curPortal2;


    protected override void OnMotorFixedUpdate(float deltaTime)
    {
        if (!disableInput)
        {
            input.InputUpdate();
            input.UnlockHorizontalControl(deltaTime);
        }

        ClampVelocity();
        ClampToStageBounds();
    }

    private void ClampVelocity()
    {
        velocity = Vector3.ClampMagnitude(velocity, stats.maxSpeed);
    }

    private void ClampToStageBounds()
    {
        var stageManager = StageManager.Instance;

        if (stageManager && !disableCollision)
        {
            Vector2 nextPosition = position;

            if ((nextPosition.x - currentBounds.extents.x - wallExtents) < stageManager.bounds.xMin)
            {
                float safeDistance = stageManager.bounds.xMin + currentBounds.extents.x;
                nextPosition.x = Mathf.Max(nextPosition.x, safeDistance);
                velocity.x = Mathf.Max(velocity.x, 0);
            }
            else if ((nextPosition.x + currentBounds.extents.x + wallExtents) > stageManager.bounds.xMax)
            {
                float safeDistance = stageManager.bounds.xMax - currentBounds.extents.x;
                nextPosition.x = Mathf.Min(nextPosition.x, safeDistance);
                velocity.x = Mathf.Min(velocity.x, 0);
            }

            if ((nextPosition.y - height * 0.5f) < stageManager.bounds.yMin)
            {
                float safeDistance = stageManager.bounds.yMin - height * 0.5f;
                nextPosition.y = Mathf.Max(nextPosition.y, safeDistance);
            }

            position = nextPosition;
        }
    }

    public void UpdateDirection(float direction)
    {
        if (direction != 0)
        {
            this.direction = (direction > 0) ? 1 : -1;
        }
    }

    public void HandleSlopeFactor(float deltaTime)
    {
        if (grounded)
        {
            if (!attacking)
            {
                velocity.x += up.x * stats.slope * deltaTime;
            }
            else
            {
                bool downHill = (Mathf.Sign(velocity.x) == Mathf.Sign(up.x));
                float slope = downHill ? stats.slopeRollDown : stats.slopeRollUp;
                velocity.x += up.x * slope * deltaTime;
            }
        }
    }

    public void HandleAcceleration(float deltaTime)
    {
        float acceleration = grounded ? stats.acceleration : stats.airAcceleration;

        if (input.right && (velocity.x < stats.topSpeed))
        {
            velocity.x += acceleration * deltaTime;
            velocity.x = Mathf.Min(velocity.x, stats.topSpeed);
        }
        else if (input.left && (velocity.x > -stats.topSpeed))
        {
            velocity.x -= acceleration * deltaTime;
            velocity.x = Mathf.Max(velocity.x, -stats.topSpeed);
        }
    }

    public void HandleDeceleration(float deltaTime)
    {
        if (grounded)
        {
            float deceleration = attacking ? stats.rollDeceleration : stats.deceleration;

            if (input.right && (velocity.x < 0))
            {
                velocity.x += deceleration * deltaTime;

                if (velocity.x >= 0)
                {
                    velocity.x = stats.turnSpeed;
                }
            }
            else if (input.left && (velocity.x > 0))
            {
                velocity.x -= deceleration * deltaTime;

                if (velocity.x <= 0)
                {
                    velocity.x = -stats.turnSpeed;
                }
            }
        }
    }

    public void HandleFriction(float deltaTime)
    {
        if (grounded && (attacking || (input.horizontal == 0)))
        {
            float friction = attacking ? stats.rollFriction : stats.friction;
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, friction * deltaTime);
        }
    }

    public void HandleGravity(float deltaTime)
    {
        if (!grounded)
        {
            float gravity = halfGravity ? (stats.gravity * 0.5f) : stats.gravity;
            velocity.y -= gravity * deltaTime;
        }
    }

    public void HandleJump()
    {
        if (grounded)
        {
            velocity.y = stats.maxJumpHeight;
        }
    }

    public void HandleFall()
    {
        if (grounded)
        {
            if ((Mathf.Abs(velocity.x) < stats.minSpeedToSlide) && (angle >= stats.minAngleToSlide))
            {
                if (angle >= stats.minAngleToFall)
                {
                    GroundExit();
                }

                input.LockHorizontalControl(stats.controlLockTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ForwardTrigger"))
        {
            groundLayer |= (1 << 11);
            groundLayer &= ~(1 << 10);
            wallLayer |= (1 << 11);
            wallLayer &= ~(1 << 10);
        }
        else if (other.CompareTag("BackwardTrigger"))
        {
            groundLayer |= (1 << 10);
            groundLayer &= ~(1 << 11);
            wallLayer |= (1 << 10);
            wallLayer &= ~(1 << 11);
        }
    }

    private void update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 cursorPos = cam.ScreenToWorldPoint(Input.mousePosition);

            if (Portal_1 != null)
            {
                Destroy(curPortal1);
            }

            curPortal1 = Instantiate(Portal_1, new Vector2(cursorPos.x, cursorPos.y), Quaternion.identity);

            curPortal1.GetComponent<Portal>().Portal1 = curPortal1;
            curPortal1.GetComponent<Portal>().Portal2 = curPortal2;

            if (curPortal2 != null)
            {
                curPortal2.GetComponent<Portal>().Portal1 = curPortal1;
                curPortal2.GetComponent<Portal>().Portal2 = curPortal2;
            }



            //PortalIndex = (PortalIndex + 1) % 2;
            //Debug.Log(PortalIndex);

        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 cursorPos = cam.ScreenToWorldPoint(Input.mousePosition);

            if (Portal_2 != null)
            {
                Destroy(curPortal2);
            }

            curPortal2 = Instantiate(Portal_2, new Vector2(cursorPos.x, cursorPos.y), Quaternion.identity);

            curPortal2.GetComponent<Portal>().Portal1 = curPortal1;
            curPortal2.GetComponent<Portal>().Portal2 = curPortal2;

            if (curPortal1 != null)
            {
                curPortal1.GetComponent<Portal>().Portal1 = curPortal1;
                curPortal1.GetComponent<Portal>().Portal2 = curPortal2;
            }

            //PortalIndex = (PortalIndex + 1) % 2;
            //Debug.Log(PortalIndex);
        }
    }
}

