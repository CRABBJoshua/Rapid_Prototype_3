using UnityEngine;
using System.Collections;

/*TODOLIST:
 * Spawn projectile
 */
public class BetterCharacterController : PlayerMotor
{
    protected bool facingRight = true;
    protected bool jumped;
    public int maxJumps;
    protected int currentjumpCount;

    public float speed = 5.0f;
    public float jumpForce = 1000;
    public Vector2 SpeedForce;
    public GameObject Portal_1;
    public GameObject Portal_1_Projectile;
    public GameObject Portal_2_Projectile;
    public GameObject Portal_2;
    public int PortalIndex = 0;
    Camera cam;

    private GameObject curPortal1;
    private GameObject curPortal2;

    private float horizInput;

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

    void Update()
    {

		if (Input.GetMouseButtonDown(0))
		{
			//start slow mo
			Time.timeScale = 0.05f;
		}

		if (Input.GetMouseButtonUp(0))
		{
			//reset time scale
			//spawn portal
			Time.timeScale = 1.0f;
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
		}

		if (Input.GetMouseButtonDown(1))
		{
			//start slow mo
			Time.timeScale = 0.05f;
		}

		if (Input.GetMouseButtonUp(1))
		{
			//reset time scale
			//spawn portal
			Time.timeScale = 1.0f;
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
		}

        //Get Player input 
        horizInput = Input.GetAxis("Horizontal");

        
    }

    void Start()
    {
        cam = Camera.main;  
    }
}
