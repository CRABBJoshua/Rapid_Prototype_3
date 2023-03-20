using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class ExampleController : MonoBehaviour
{
	/*TODO LIST:
	 * Snaping
	 * Movement
	*/
	
	#region inputs
	[SerializeField] private string m_MoveInput;
	[SerializeField] private string m_JumpInput;

	public float m_InMove;
	private float m_InMovePrev; //set this value every frame to the old move input then check the input in apply motion to see if they have different signs
	private bool m_InJump;
	#endregion

	#region Grounded Checks
	private bool m_Grounded;
	private bool m_GroundedCheckActive;
	#endregion

	#region Timers
	private float m_MovingTimer;
	private float m_JumpingTimer;
	private float m_RayCastTimer = 20;
	private float m_HoverTimer = 10;
	#endregion

	[SerializeField] private PlayerStats m_Stats;

	private Rigidbody2D m_RB;
	[SerializeField] private AnimationCurve m_MomentumLUT;
	[SerializeField] private AnimationCurve m_SnapLUT;
	[SerializeField] private float time;
	[SerializeField] private LayerMask m_Layer;
	[SerializeField] private LayerMask m_WhatIsGround;
	[SerializeField] private GameObject Player;
	[SerializeField] private AnimationCurve m_SpeedFactor;
	
	private float m_JumpDelay = 1f;
	private float m_SpeedDelay = 1f;
	private float m_ForceDown = 10f;
	private float m_HoverForce = 10f;
	public Vector2 m_VelocityReset;

	public int maxHealth = 100;
	public int currentHealth;

	public CS_HealthBar healthBar;

	void Start()
	{
		currentHealth = maxHealth;	
		healthBar.SetMaxHealth(maxHealth);
	}

	private void Awake()
	{
		m_RB = GetComponent<Rigidbody2D>();
		m_GroundedCheckActive = true;
		m_JumpingTimer = 0f;
		m_MovingTimer = 0f;
	}

	void Update()
	{
		GetInputs();

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SceneManager.LoadScene(1);
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			TakeDamage(20);
		}

		if (currentHealth <= 0)
		{
			Death();
		}
	}

	void LoadLevel()
	{
		Application.LoadLevel("BetterPlatformer");
	}

	void Death()
	{
		Destroy(Player);
		LoadLevel();
	}

	public void TakeDamage(int Damage)
	{
		currentHealth -= Damage;
		healthBar.SetHealth(currentHealth);
	}

	private void FixedUpdate()
	{
		if (m_GroundedCheckActive) { GroundedCheck(); }
		ApplyMotion();
		Jump();
	}

	private void GetInputs()
	{
		m_InMovePrev = m_InMove;
		m_InMove = Input.GetAxis(m_MoveInput);
		m_InJump = Input.GetButton(m_JumpInput);
	}

	private void ApplyMotion()
	{
		//Update the mmoving timer value based on time
		if (m_InMove != 0f && Mathf.Sign(m_InMove) == Mathf.Sign(m_InMovePrev))
		{
			m_MovingTimer += Time.fixedDeltaTime;
		}
		else
		{
			m_MovingTimer = 0f;
		}
		//get the speed from the LUT and apply it to velocity

		float xSpeed = m_MomentumLUT.Evaluate(m_MovingTimer / m_Stats.timeToMaxSpeed) * m_Stats.maxSpeed * ((m_InMove > 0f) ? 1f : -1f);

		float SpeedFactor = m_SpeedFactor.Evaluate(Vector2.Dot((Vector2.right * xSpeed).normalized, m_RB.velocity.normalized));

		m_RB.velocity = new Vector2(xSpeed * SpeedFactor, m_RB.velocity.y);
		m_RB.velocity = Vector2.ClampMagnitude(m_RB.velocity, m_Stats.maxSpeed);

		//Stops spped on the direction change
		//if(m_InMove == 1)
		//{
		//	m_RB.velocity = new Vector2(xSpeed, m_RB.velocity.y);
		//	m_RB.velocity = Vector2.ClampMagnitude(m_RB.velocity, m_Stats.maxSpeed);
		//	if (m_InMove != 1)
		//	{
		//		StartCoroutine(C_StopSpeed());
		//	}

		//}
		//else if(m_InMove == -1)
		//{
		//	m_RB.velocity = new Vector2(xSpeed, m_RB.velocity.y);
		//	m_RB.velocity = Vector2.ClampMagnitude(m_RB.velocity, m_Stats.maxSpeed);
		//	if (m_InMove != -1)
		//	{
		//		Debug.Log("WORK PLEASE");
		//		StartCoroutine(C_StopSpeed());
		//	}

		//}

		//if (m_InMove == 0 || m_InMove != 1 || m_InMove == -1)
		//{
		//	Debug.Log("WORK PLEASE");
		//	StartCoroutine(C_StopSpeed());
		//}
	}

	private void Jump()
	{
		if (!m_InJump) { return; }

		if (m_Grounded)
		{
			m_RB.AddForce(Vector2.up * m_Stats.JumpForce, ForceMode2D.Impulse);
			m_Grounded = false;
			m_GroundedCheckActive = false;
			StartCoroutine(C_DelayGroundedCheck());
		}
		else if (m_JumpingTimer < m_HoverTimer)
		{
			//hover FIX THIS
			m_RB.AddForce(Vector2.up * m_HoverForce, ForceMode2D.Force);
			m_JumpingTimer += Time.fixedDeltaTime;
		}
	}

	private void GroundedCheck()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down * m_RayCastTimer, m_Layer);

		m_Grounded = (hit != null);

		if(!m_Grounded) { return; }

		m_JumpingTimer = 0f;
		m_RB.AddForce(Vector2.down * m_ForceDown, ForceMode2D.Force);
	}

	private IEnumerator C_DelayGroundedCheck()
	{
		yield return new WaitForSeconds(m_JumpDelay);
		m_GroundedCheckActive = true;
	}

	private IEnumerator C_StopSpeed()
	{
		yield return new WaitForSeconds(m_SpeedDelay);
		m_RB.velocity = Vector2.ClampMagnitude(m_RB.velocity, 1);
	}

	//private void SurfaceAllignment()
	//{
	//	Ray m_Ray = new Ray(transform.position, -transform.up);
	//	RaycastHit info = new RaycastHit();

	//	if(Physics.Raycast(m_Ray, out info, m_WhatIsGround))
	//	{
	//		Debug.Log(m_Ray);
	//		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Vector2.up, info.normal), m_SnapLUT.Evaluate(time));
	//	}
	//}
}
