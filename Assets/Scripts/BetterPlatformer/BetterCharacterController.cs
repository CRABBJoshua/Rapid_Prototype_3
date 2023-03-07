using UnityEngine;
using System.Collections;


//--------------------------------------------
/*Better Character Controller Includes:
     - Fixed Update / Update Input seperation
     - Better grounding using a overlap box
     - Basic Multi Jump
 */
//--------------------------------------------
public class BetterCharacterController : MonoBehaviour
{
    protected bool facingRight = true;
    protected bool jumped;
    public int maxJumps;
    protected int currentjumpCount;

    public float speed = 5.0f;
    public float jumpForce = 1000;
    public Vector2 SpeedForce;
    public GameObject Portal_1;
    public GameObject Portal_2;
    public int PortalIndex = 0;
    Camera cam;

    private GameObject curPortal1;
    private GameObject curPortal2;

    private float horizInput;

    public bool grounded;

    public Rigidbody2D rb;

    public LayerMask groundedLayers;

    protected Collider2D charCollision;
    protected Vector2 playerSize, boxSize;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        charCollision = GetComponent<Collider2D>();
        playerSize = charCollision.bounds.extents;
        boxSize = new Vector2(playerSize.x, 0.05f);
    }

    void FixedUpdate()
    {
        //Box Overlap Ground Check
        Vector2 boxCenter = new Vector2(transform.position.x + charCollision.offset.x, transform.position.y + -(playerSize.y + boxSize.y - 0.01f) + charCollision.offset.y);
        grounded = Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundedLayers) != null;

        //Mathf.Clamp

        //forceToApply = Vector2.ClampMagnitude(forceToApply, 2.0f);

        //Move Character
        //rb.velocity = new Vector2(horizInput * speed * Time.fixedDeltaTime, rb.velocity.y);

        if (grounded)
        {
            if (Input.GetKey(KeyCode.LeftArrow) || (Input.GetKey(KeyCode.A)))
            {
                rb.AddForce(Vector2.left * SpeedForce, ForceMode2D.Force);
                SpeedForce = Vector2.ClampMagnitude(SpeedForce, 300);
            }
            if (Input.GetKey(KeyCode.RightArrow) || (Input.GetKey(KeyCode.D)))
            {
                rb.AddForce(Vector2.right * SpeedForce, ForceMode2D.Force);
                SpeedForce = Vector2.ClampMagnitude(SpeedForce, 300);
            }
        }

        //Jump
        if (jumped == true)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            SpeedForce = Vector2.ClampMagnitude(SpeedForce, 100);
            Debug.Log("Jumping!");

            jumped = false;
        }

        // Detect if character sprite needs flipping.
        if (horizInput > 0 && !facingRight)
        {
            FlipSprite();
        }
        else if (horizInput < 0 && facingRight)
        {
            FlipSprite();
        }
    }

    void Update()
    {
        if (grounded)
        {
            currentjumpCount = maxJumps;
        }

        //Input for jumping ***Multi Jumping***
        if (Input.GetButtonDown("Jump") && currentjumpCount > 1)
        {
            jumped = true;
            currentjumpCount--;
            Debug.Log("Should jump");
        }

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

        //Get Player input 
        horizInput = Input.GetAxis("Horizontal");

        
    }

    // Flip Character Sprite
    void FlipSprite()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    void Start()
    {
        cam = Camera.main;  
    }
}
