﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private ChatBox _chatBox;
    private Dialog _dialogComp;
    
    Rigidbody2D mRigidbody;

    [SerializeField] float speed = 0;

    private Vector2 curVeclocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        _chatBox = GetComponent<ChatBox>();
        _dialogComp = GameObject.FindGameObjectWithTag("Dialog").GetComponent<Dialog>();
        mRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        mRigidbody.velocity = Vector2.zero;
        verticalMove(Input.GetAxisRaw("Vertical"));
        horizontalMove(Input.GetAxisRaw("Horizontal"));
        mRigidbody.velocity = mRigidbody.velocity.normalized * speed;
    }

    void verticalMove(float _direction)
    {
        if (_chatBox.isWriting || _dialogComp.isPrinting || _dialogComp.waitingAction)
            return;
        if(_direction.Equals(1))
        {
            mRigidbody.velocity = Vector2.SmoothDamp(mRigidbody.velocity, Vector2.up, ref curVeclocity,0.05f);
        }
        else if(_direction.Equals(-1))
        {
            mRigidbody.velocity = Vector2.SmoothDamp(mRigidbody.velocity, Vector2.down, ref curVeclocity, 0.05f);
        }
    }

    void horizontalMove(float _direction)
    {
        if (_chatBox.isWriting || _dialogComp.isPrinting || _dialogComp.waitingAction)
            return;
        if (_direction.Equals(1))
        {
            mRigidbody.velocity = Vector2.SmoothDamp(mRigidbody.velocity, Vector2.right, ref curVeclocity, 0.05f);
        }
        else if (_direction.Equals(-1))
        {
            mRigidbody.velocity = Vector2.SmoothDamp(mRigidbody.velocity, Vector2.left, ref curVeclocity, 0.05f);
        }
    }
}
