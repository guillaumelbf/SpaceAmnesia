﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private ChatBox _chatBox;
    
    Rigidbody2D mRigidbody;

    [SerializeField] float speed = 0;

    private Vector2 curVeclocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        _chatBox = GetComponent<ChatBox>();
        mRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            Dialog.PrintNewDialog("I can only talk to those who know who I am", 0.12f, true);
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
        if (_chatBox.isWriting || Dialog.isPrinting || Dialog.waitingAction)
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
        if (_chatBox.isWriting || Dialog.isPrinting || Dialog.waitingAction)
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
