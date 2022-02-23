using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BirdScript : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 _vel;
    private const float GRAV = 9.81f;
    private const float JUMP_SPEED = 4f;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (!Mgr.Paused)
		{
            UpdateMovement();
		}

        void UpdateMovement()
		{
            var delTime = Time.deltaTime;
            _vel.y -= GRAV * delTime;

            if (Input.GetKeyDown(KeyCode.Space))
			{
                _vel.y = JUMP_SPEED;
			}

            _rb.position += _vel * delTime;

		}
    }
}
