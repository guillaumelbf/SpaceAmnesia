using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    private static string _messageToPrint;
    private static string _currMessage;
    
    private static float _timeBetweenLetters;
    private static float _currTimeBetweenLetters;

    public static float freezeTimeAfterPrint;
    private static float _currFreezeTime;
    
    public static bool isPrinting;
    public static bool waitingAction;

    private static GameObject _background;
    private static TMP_Text _dialog;

    private static GameObject _wrongInput;
    private static GameObject _rightInput;

    private static ChatBox _chatBox;

    private static Dialog _instance;


    private void Awake()
    {
        if (_instance == null)
        {

            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            
            _background = transform.GetChild(0).gameObject;
            _dialog = _background.transform.GetChild(0).GetComponent<TMP_Text>();
            _wrongInput = _background.transform.GetChild(1).gameObject;
            _rightInput = _background.transform.GetChild(2).gameObject;
            isPrinting = false;
            waitingAction = false;
            _background.SetActive(false);
            _wrongInput.SetActive(false);
            _rightInput.SetActive(false);
            _chatBox = GameObject.FindGameObjectWithTag("Player").GetComponent<ChatBox>();
        }
        else
            Destroy(this);
    }
        
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            PrintNewDialog("Salutations, humain.\nComment allez-vous?", 0.05f, false);
        
        
        if (isPrinting)
            UpdatePrint();
        else if (waitingAction)
            UpdateWait();
    }

    public static void PrintNewDialog(string msg, float dtLetters, bool badInput)
    {
        if (isPrinting || waitingAction || _chatBox.isWriting)
            return;
        
        _background.SetActive(true);
        _timeBetweenLetters = dtLetters;
        _messageToPrint = msg;
        _currMessage = "";
        isPrinting = true;
        if (badInput)
            _wrongInput.SetActive(true);
        else
            _rightInput.SetActive(true);
    }

    public void UpdatePrint()
    {
        if (_currMessage.Length == _messageToPrint.Length)
        {
            isPrinting = false;
            waitingAction = true;
        }
        else
        {
            _currTimeBetweenLetters -= Time.deltaTime;
            if (Input.GetKey(KeyCode.Return))
                _currTimeBetweenLetters -= 4.0f * Time.deltaTime;
            if (_currTimeBetweenLetters <= 0.0f)
            {
                _currMessage += _messageToPrint[_currMessage.Length];
                _currTimeBetweenLetters = _timeBetweenLetters;
                _currFreezeTime = freezeTimeAfterPrint;
            }
        }

        _dialog.text = _currMessage;
    }

    public void UpdateWait()
    {
        _currFreezeTime -= Time.deltaTime;
        if (_currFreezeTime <= 0.0f && Input.anyKeyDown)
        {
            waitingAction = false;
            _background.SetActive(false);
            _rightInput.SetActive(false);
            _wrongInput.SetActive(false);
        }
    }

}
