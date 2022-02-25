using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{
	private Rigidbody2D _rb;
	private bool _moved = false;
	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
	}
	void Start()
	{
		_rb.useFullKinematicContacts = true;
		var _offset = (Mgr.RandNextFloat - .5f) * 4f;
		Debug.Log("Rand Offset: " + _offset);
		_rb.MovePosition(new Vector2(_rb.position.x, _offset));
		_rb.velocity = new Vector2(-9f, 0);
	}

	// Update is called once per frame
	void Update()
	{
		if (transform.position.x <= -5f)
		{
			Destroy(gameObject);
		}

	}

	private void FixedUpdate()
	{
	}
}
