using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ExampleController : MonoBehaviour
{
	#region inputs
	[SerializeField] private string m_MoveInput;
	[SerializeField] private string m_JumpInput;

	private float m_InMove;
	private bool m_InJump;
	#endregion

	[SerializeField] private PlayerStats m_Stats;

	private Rigidbody2D m_RB;
	[SerializeField] private AnimationCurve m_MomentumLUT;
	private float m_MovingTimer;
	private float m_JumpingTimer;

	private bool m_Grounded;
	private bool m_GroundedCheckActive;

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
	}

	private void FixedUpdate()
	{
		if (m_GroundedCheckActive) { GroundedCheck(); }
		ApplyMotion();
		Jump();
		GetComponent<Rigidbody2D>().AddForce(Physics.gravity, ForceMode2D.Force);
	}

	private void GetInputs()
	{
		m_InMove = Input.GetAxis(m_MoveInput);
		m_InJump = Input.GetButton(m_JumpInput);
	}

	private void ApplyMotion()
	{
		//Update the mmoving timer value based on time
		if (m_InMove != 0f)
		{
			m_MovingTimer += Time.fixedDeltaTime;
		}
		else
		{
			m_MovingTimer = 0f;
		}
		//get the speed from the LUT and apply it to velocity
		m_RB.velocity = Vector2.right * m_MomentumLUT.Evaluate(m_MovingTimer / m_Stats.timeToMaxSpeed) * m_Stats.maxSpeed * ((m_InMove > 0f) ? 1f : -1f);
	}

	private void Jump()
	{
		if (!m_InJump) { return; }

		if (m_Grounded)
		{
			//yeet FIX THIS
			m_RB.AddForce(Vector2.up * m_Stats.JumpForce, ForceMode2D.Impulse);
			m_Grounded = false;
			m_GroundedCheckActive = false;
			StartCoroutine(C_DelayGroundedCheck());
		}
		else if (m_JumpingTimer < 76234f) //FIX THIS
		{
			//hover FIX THIS
			m_RB.AddForce(Vector2.up * 10, ForceMode2D.Force);
			m_JumpingTimer += Time.fixedDeltaTime;
		}
	}

	private void GroundedCheck()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down * 39f); //FIX THIS to not use hard coded values and possibly a layermask

		m_Grounded = (hit != null);

		if(!m_Grounded) { return; }

		m_JumpingTimer = 0f;
		m_RB.AddForce(Vector2.down * 10f, ForceMode2D.Force); //FIX THIS
	}

	private IEnumerator C_DelayGroundedCheck()
	{
		yield return new WaitForSeconds(0.5f); //FIX THIS
		m_GroundedCheckActive = true;
	}
}