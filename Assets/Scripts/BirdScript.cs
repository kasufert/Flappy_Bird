using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BirdScript : MonoBehaviour
{
	private Rigidbody2D _rb;
	private Vector2 _vel;
	private const float GRAV = 20f;
	private const float JUMP_SPEED = 4.1f;
	private const float TERMINAL_VELOCITY = 5f;
	private bool _isDead = false;
	public static event Action BirdDeath;
	// Start is called before the first frame update
	void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
		_rb.position = Vector2.zero;
		_vel = Vector2.zero;
		_isDead = false;
		_rb.isKinematic = true;
		_rb.useFullKinematicContacts = true;
		Mgr.RestartGame += Start;
	}

	// Update is called once per frame
	void Update()
	{
		if (!Mgr.Paused)
		{
			UpdateVelocity();
			if (_rb.position.y > 2f || _rb.position.y < -2f)
			{
				BirdDeath();
			}
		}

		void UpdateVelocity()
		{
			var delTime = Time.deltaTime;
			_vel.y -= GRAV * delTime;

			if (Input.GetKeyDown(KeyCode.Space) && !_isDead)
			{
				_vel.y = JUMP_SPEED;
			}

			if (_vel.y < -TERMINAL_VELOCITY)
			{
				_vel.y = -TERMINAL_VELOCITY;
			}

			_rb.velocity = _vel;
			if (_vel.y >= -3f)
			{
				_rb.rotation = 40f;
			}
			else
			{
				_rb.rotation = _vel.y * (130f / 6f) + (130f / 1.5f);
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		_isDead = true;
		_rb.velocity = Vector2.zero;
		BirdDeath();
		Debug.Log("Collided");
	}
}
