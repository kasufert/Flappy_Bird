using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{
	public const float PIPE_SPEED = 2f;
	public const float TWICE_MAX_OFFSET = 1.6f;
	private static float s_LastOffset = 0f;
	private Rigidbody2D _rb;
	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
		BirdScript.BirdDeath += OnBirdDeath;
		Mgr.RestartGame += OnRestartGame;
	}
	private void OnRestartGame()
	{
		if (gameObject != null)
			Destroy(gameObject);
	}
	private void OnDestroy()
	{
		BirdScript.BirdDeath -= OnBirdDeath;
		Mgr.RestartGame -= OnRestartGame;
	}
	void Start()
	{
		_rb.useFullKinematicContacts = true;
		float offset = GenerateNewOffset();
		Debug.Log("Rand Offset: " + offset);
		s_LastOffset = Mathf.Clamp(offset, -.2f, .2f);
		_rb.MovePosition(new Vector2(_rb.position.x, offset));
		_rb.velocity = new Vector2(-PIPE_SPEED, 0);

		static float GenerateNewOffset()
		{
			float nextF = Mgr.RandNextFloat;
			float offset = (nextF - .5f + s_LastOffset) * TWICE_MAX_OFFSET;
			Debug.Log("Rand Next Float: " + nextF);
			return offset;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (transform.position.x <= -2f)
		{
			Destroy(gameObject);
		}

	}
	private void OnBirdDeath()
	{
		if (_rb != null)
			_rb.velocity = Vector2.zero;
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Mgr.IncScore();
	}
}
