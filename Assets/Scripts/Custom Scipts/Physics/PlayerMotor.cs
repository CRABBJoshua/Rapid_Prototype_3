using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMotor : MonoBehaviour
{
    [Header("Base Class Settings")]
    public bool simulate = true;
    public bool lockUpright;

    [Header("Collider Class Settings")]
    public float height;
    public float wallExtents;
    public float groundExtents;
    public Bounds[] bounds;

    [Header("Collision Class Masks")]
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask ceilingLayer;

    [Space(10)]
    public Vector2 velocity;

    public Bounds currentBounds { get; private set; }
    //I dont understand
    public bool grounded { get; private set; }

    private new BoxCollider2D collider;
    private new Rigidbody2D rigidbody;

    private const float maxTimeStep = 1 / 60f;

    //I dont understand
    public Vector2 position { get; protected set; }
    public Vector2 right { get; private set;  }
    public Vector2 up { get; private set; }
    public bool disableCollision { get; private set; }
    public float angle { get; private set; }

    private void Start()
    {
        StartMotor();
        OnMotorStart();
    }

    private void Update()
    {
        OnMotorUpdate();
        SimulatePhysics();
    }

    private void LateUpdate()
    {
        OnMotorLateUpdate();
    }

    private void StartMotor()
    {
        InitializeRigidbody();
        InitializeCollider();
    }

    private void InitializeRigidbody()
    {
        //I dont understand
        if (!TryGetComponent(out rigidbody))
        {
            rigidbody = gameObject.AddComponent<Rigidbody2D>();
        }

        //I dont understand
        rigidbody.isKinematic = true;
    }

    private void InitializeCollider()
    {
        //I dont understand
        if (!TryGetComponent(out collider))
        {
            collider = gameObject.AddComponent<BoxCollider2D>();
        }

        //I dont understand
        collider.isTrigger = true;
        ChangeBounds(0);
    }

    private void GetPhysicsState()
    {
        //I dont understand
        position = transform.position;
        right = transform.right;
        up = transform.up;
        angle = Vector2.Angle(up, Vector2.up);
    }

    private void SetPhysicsState()
    {
        //I dont understand
        transform.position = position;
        transform.LookAt(transform.position + transform.forward, up);
    }

    private void SimulatePhysics()
    {
        if(simulate)
        {
            //I dont understand
            GetPhysicsState();

            float frameTime = Time.deltaTime;

            while (frameTime > 0f)
            {
                float deltaTime = Mathf.Min(frameTime, maxTimeStep);

                OnMotorFixedUpdate(deltaTime);
                MotorFixedUpdate(deltaTime);

                frameTime -= deltaTime;
            }

            SetPhysicsState();
        }
    }

    private void MotorFixedUpdate(float deltaTime)
    {
        UpdateGroundState();
        UpdateCollision(deltaTime);
    }

    private void UpdateGroundState()
    {
        if(grounded && velocity.y > 0f)
        {
            GroundExit();
        }
    }

    private void UpdateCollision(float deltaTime)
    {
        if(!disableCollision)
        {
            Vector2 horizontalTranslation = right * velocity.x * deltaTime;
            Vector2 verticalTranslation = up * velocity.y * deltaTime;

            HorizontalCollision(horizontalTranslation.normalized, horizontalTranslation.magnitude);
            VerticalCollision(verticalTranslation.normalized, verticalTranslation.magnitude);
        }
        else
        {
            position += (Vector2)velocity * deltaTime;
        }
    }

    private void HorizontalCollision(Vector3 direction, float distance)
    {
        Vector2 origin = position + up * currentBounds.center.y;
        float offset = currentBounds.extents.x + wallExtents;
        //MADE A CHANGE ( + to *)
        Vector2 destination = position * direction * distance;

        if(Physics.Raycast(origin, destination, out var hit, distance + offset, wallLayer))
        {
            CallContact(hit.collider);

            if(Vector2.Dot(transform.InverseTransformVector(velocity), hit.normal) <= 0 && hit.collider.enabled)
            {
                float safeDistance = hit.distance - offset;
                //MADE A CHANGE ( + to *)
                destination = position * direction * safeDistance;
            }
        }

        position = destination;
    }

    private void VerticalCollision(Vector3 direction, float distance)
    {
        float offset = height * 0.5f;
        //MADE A CHANGE ( + to *)
        Vector2 destination = position * direction * distance;
        LayerMask layer = (direction.y <= 0) ? groundLayer : ceilingLayer;

        if(!grounded)
        {
            if(GroundRaycast(direction, distance, offset, out var groundinfo, out _, layer))
            {
               bool movingTowardsGround = (Vector2.Dot(velocity, groundinfo.normal) <= 0);
               bool validSurface = (Vector2.Angle(groundinfo.normal, Vector2.up) <135f);

                if(movingTowardsGround)
                {
                    float safeDistance = groundinfo.distance - offset;

                    if(validSurface)
                    {
                        GroundEnter(groundinfo.normal);

                        if(groundinfo.collider.CompareTag("MovingPlatform"))
                        {
                            transform.parent = groundinfo.collider.transform;
                        }
                    }

                    velocity.y = 0;
                    //MADE A CHANGE ( + to *)
                    destination = position * direction * safeDistance;
                }
            }
        }
        else
        {
            float groundRaySize = offset + groundExtents;
            bool colliding = GroundRaycast(-up, 0, groundRaySize, out var groundinfo, out var snap, layer);

            if(colliding && velocity.y <= 0)
            {
                if(snap)
                {
                    up = groundinfo.normal;
                    float safeDistance = groundinfo.distance - offset;
                    destination = position - up * safeDistance;
                }
            }
            else
            {
                GroundExit();
            }
        }

        position = destination;
    }

    private bool GroundRaycast(Vector3 direction, float distance, float offset, out FreedomCollision hit, out bool snap, LayerMask layer)
    {
        float hitDistance = 0f;
        Vector2 hitPoint = Vector2.zero;
        Vector2 hitNormal = Vector2.zero;
        float raySize = distance + offset;
        Vector2 leftRayOrigin = position - right * currentBounds.extents.x;
        Vector2 rightRayOrigin = position + right * currentBounds.extents.x;
        bool leftRay = Physics.Raycast(leftRayOrigin, direction, out var leftHit, raySize, layer);
        bool rightRay = Physics.Raycast(rightRayOrigin, direction, out var rightHit, raySize, layer);
        bool colliding = snap = false;

        Collider closestCollider = null;

        //Teacher Code
        //MADE A CHANGE ( + to *)
        Debug.DrawLine(leftRayOrigin, leftRayOrigin * direction, Color.yellow);
        Debug.DrawLine(rightRayOrigin, rightRayOrigin * direction, Color.yellow);

        if(leftRay || rightRay)
        {
            if(leftRay && rightRay)
            {
                //Teacher Code
                Debug.DrawLine(leftHit.point, leftHit.point + leftHit.normal, Color.red);
                Debug.DrawLine(rightHit.point, rightHit.point + rightHit.normal, Color.red);

                if(Vector2.Dot(leftHit.normal, rightHit.normal) > 0.8f)
                {
                    snap = true;
                    hitPoint = (leftHit.point + rightHit.point) * 0.5f;
                    hitNormal = (leftHit.normal + rightHit.normal) * 0.5f;
                    hitDistance = (leftHit.distance < rightHit.distance) ? leftHit.distance : rightHit.distance;
                    closestCollider = (leftHit.distance < rightHit.distance) ? leftHit.collider : rightHit.collider;
                }
                else
                {
                    var closestHit = (leftHit.distance < rightHit.distance) ? leftHit : rightHit;
                    hitPoint = closestHit.point;
                    hitNormal = closestHit.normal;
                    hitDistance = closestHit.distance;
                    closestCollider = closestHit.collider;
                }

                CallContact(leftHit.collider);
                CallContact(rightHit.collider);
                colliding = (leftHit.collider.enabled || rightHit.collider.enabled);
            }
            else
            {
                var closestHit = leftRay ? leftHit : rightHit;
                hitPoint = closestHit.point;
                hitNormal = closestHit.normal;
                hitDistance = closestHit.distance;
                CallContact(closestHit.collider);
                closestCollider = closestHit.collider;
                colliding = closestHit.collider.enabled;
            }
        }

        if ((closestCollider != null) && snap)
        {
            if(closestCollider.CompareTag("MovingPlatform"))
            {
                snap = false;
            }
        }

        hit = new FreedomCollision(hitPoint, hitNormal, hitDistance, closestCollider);
        return colliding;
    }

    private void CallContact(Collider collider)
    {
        if(collider.TryGetComponent(out FreedomObject listener))
        {
            listener.OnPlayerMotorContact(this);
        }
    }

    public void EnableCollision(bool value = true)
    {
        disableCollision = !value;
        collider.enabled = value;
    }

    public void ChangeBounds(int index)
    {
        if ((index >= 0) && (index < bounds.Length))
        {
            currentBounds = bounds[index];
            UpdateCollider();
        }
    }

    private void UpdateCollider()
    {
        //MADE A CHANGE v
        //collider.center = currentBounds.center;
        collider.size = currentBounds.size;
    }

    public void GroundEnter(Vector3 normal)
    {
        if(!grounded)
        {
            OnGroundEnter();
            up = normal;
            velocity = AirToGround(velocity, up);
            grounded = true;
        }
    }

    public void GroundExit()
    {
        if(grounded)
        {
            transform.parent = null;
            velocity = GroundToAir(velocity);
            up = Vector2.up;
            grounded = false;
        }
    }

    private Vector2 AirToGround(Vector2 velocity, Vector2 normal)
    {
        return new Vector2(velocity.x * normal.y - velocity.y * normal.x, 0);
    }

    private Vector2 GroundToAir(Vector2 velocity)
    {
        return new Vector2(velocity.x * up.y + velocity.y * up.x, velocity.y * up.y - velocity.x * up.x);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            if (bounds.Length > 0)
            {
                foreach (Bounds b in bounds)
                {
                    var offset = transform.position + b.center;

                    Gizmos.color = new Color(0, 0, 1, 0.7f);
                    Gizmos.DrawWireCube(offset, b.size);
                    Gizmos.color = new Color(0, 0, 1, 0.35f);
                    Gizmos.DrawCube(offset, b.size);
                }
            }
        }
    }
#endif    

    protected virtual void OnMotorStart() { }

    protected virtual void OnMotorUpdate() { }

    protected virtual void OnMotorLateUpdate() { }

    protected virtual void OnMotorFixedUpdate(float deltaTime) { }

    protected virtual void OnGroundEnter() { }
}
