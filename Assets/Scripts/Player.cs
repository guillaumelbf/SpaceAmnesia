using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rigidbody;

    [SerializeField] float speed = 0;

    private Vector2 curVeclocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = Vector2.zero;
        verticalMove(Input.GetAxisRaw("Vertical"));
        horizontalMove(Input.GetAxisRaw("Horizontal"));
        rigidbody.velocity = rigidbody.velocity.normalized * speed;
    }

    void verticalMove(float _direction)
    {
        if(_direction.Equals(1))
        {
            rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, Vector2.up, ref curVeclocity,0.05f);
        }
        else if(_direction.Equals(-1))
        {
            rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, Vector2.down, ref curVeclocity, 0.05f);
        }
    }

    void horizontalMove(float _direction)
    {
        if (_direction.Equals(1))
        {
            rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, Vector2.right, ref curVeclocity, 0.05f);
        }
        else if (_direction.Equals(-1))
        {
            rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, Vector2.left, ref curVeclocity, 0.05f);
        }
    }
}
