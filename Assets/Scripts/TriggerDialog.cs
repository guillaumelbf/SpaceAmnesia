using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialog : MonoBehaviour
{
    [SerializeField]
    public NewDialog[] Dialogs;

    private ChatBox _player;
    //public string dialogText;
    //public float timeBetweenLetters;

    private bool _isIn;
    // Start is called before the first frame update
    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<ChatBox>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isIn && Input.GetButtonDown("Interact") && !Dialog.isPrinting && 
            !Dialog.waitingAction && !_player.isWriting)
        {
            
            foreach (NewDialog newDiag in Dialogs)
            {
                _isIn = false;
                string tempDialog = newDiag.message;
                tempDialog = tempDialog.Replace("\\n", "\n");
                Dialog.AddDialogToBuffer(tempDialog, newDiag.speed, newDiag.visualHint);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _isIn = true;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        _isIn = false;
    }
    
}
