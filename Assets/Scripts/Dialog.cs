using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    private string _messageToPrint;
    private string _currMessage;
    
    private float _timeBetweenLetters;
    private float _currTimeBetweenLetters;

    public float freezeTimeAfterPrint;
    private float _currFreezeTime;
    
    public bool isPrinting;
    public bool waitingAction;

    private GameObject _background;
    private TMP_Text _dialog;

    private ChatBox _chatBox;
    // Start is called before the first frame update
    void Start()
    {
        _background = transform.GetChild(0).gameObject;
        _dialog = _background.transform.GetChild(0).GetComponent<TMP_Text>();
        isPrinting = false;
        waitingAction = false;
        _background.SetActive(false);
        _chatBox = GameObject.FindGameObjectWithTag("Player").GetComponent<ChatBox>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            PrintNewDialog("Salutations, humain.\nComment allez-vous?", 0.05f);
        
        
        if (isPrinting)
            UpdatePrint();
        else if (waitingAction)
            UpdateWait();
    }

    public void PrintNewDialog(string msg, float dtLetters)
    {
        if (isPrinting || waitingAction || _chatBox.isWriting)
            return;
        
        _background.SetActive(true);
        _timeBetweenLetters = dtLetters;
        _messageToPrint = msg;
        _currMessage = "";
        isPrinting = true;
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
        }
    }
}
