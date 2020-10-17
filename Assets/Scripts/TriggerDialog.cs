using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialog : MonoBehaviour
{
    public string dialogText;
    public float timeBetweenLetters;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetButton("Interact"))
        {
            string tempDialog = "";
            tempDialog += dialogText;
            tempDialog = tempDialog.Replace("\\n", "\n");
            Dialog.PrintNewDialog(tempDialog, timeBetweenLetters, true);
        }
    }
}
