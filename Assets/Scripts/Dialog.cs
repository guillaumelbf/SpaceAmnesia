using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public struct NewDialog
{
    public string message;
    public float  speed;
    public short  visualHint;
};


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

    private static List<NewDialog> _dialogBuffer;

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
            _dialogBuffer = new List<NewDialog>();
        }
        else
            Destroy(this);
    }
        
    // Update is called once per frame
    void Update()
    {
        TryRefreshDialog();
        if (isPrinting)
            UpdatePrint();
        else if (waitingAction)
            UpdateWait();
    }

    public void PrintNewDialog(string msg, float dtLetters, short visualHint)
    {
        if (isPrinting || waitingAction || _chatBox.isWriting)
            return;
        
        _background.SetActive(true);
        _timeBetweenLetters = dtLetters;
        _messageToPrint = msg;
        _currMessage = "";
        isPrinting = true;
        if (visualHint == 0)
            _wrongInput.SetActive(true);
        else if (visualHint == 1)
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
            _dialogBuffer.RemoveAt(0);
        }
    }

    private void TryRefreshDialog()
    {
        if (waitingAction || isPrinting || _dialogBuffer.Count <= 0)
            return;
        
        PrintNewDialog(_dialogBuffer[0].message, _dialogBuffer[0].speed, _dialogBuffer[0].visualHint);
        
        
    }
    
    public static void AddDialogToBuffer(string msg, float speed, short visualHint)
    {
        NewDialog newDialog = new NewDialog();
        newDialog.message = msg;
        newDialog.speed = speed;
        newDialog.visualHint = visualHint;
        
        _dialogBuffer.Add(newDialog);
    }

}
