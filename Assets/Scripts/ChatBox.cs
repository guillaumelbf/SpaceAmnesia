using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour
{
    [HideInInspector]
    public bool isWriting;
    
    [HideInInspector]
    public bool isTalking;
    
    [HideInInspector]
    public string msgChatBox;
  
    private GameObject _chatBox;
    private GameObject _chatBoxBox;
    private TMP_Text   _chatBoxTextPopUp;

    private Renderer _textRenderer;
    
    private SpriteRenderer _chatBoxBackground;
    public Sprite thinkingBackgroundSprite;
    public Sprite talkingBackgroundSprite;

    [SerializeField] public float maxTalkTime;
    private float _currTalkTime;

    private Dialog _dialog;

    // Start is called before the first frame update
    void Start()
    {
        isWriting = false;
        isTalking = false;
        _currTalkTime = maxTalkTime;

        _dialog = GameObject.FindGameObjectWithTag("Dialog").GetComponent<Dialog>();

        /*
         *  Setup chatbox components and objects in the script to avoid
         *     catching them at each frame
         */
        
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("ChatBox"))
            {
                _chatBox = child.GetChild(0).gameObject;
                break;
            }
        }
        
        if (_chatBox == null)
            return;
        _chatBoxBox = _chatBox.transform.GetChild(0).gameObject;
        _chatBoxBackground = _chatBoxBox.GetComponent<SpriteRenderer>();
        _chatBoxTextPopUp = _chatBox.transform.GetComponent<TMP_Text>();
        _chatBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_dialog.isPrinting || _dialog.waitingAction)
            return;
        if (!isWriting && !isTalking && Input.GetKeyDown(KeyCode.Return))
        {
            _chatBox.SetActive(true);
            _chatBoxBackground.sprite = thinkingBackgroundSprite;
            msgChatBox = "";
            isWriting = true;
        }
        else if (isWriting && Input.GetKeyDown(KeyCode.Return))
        {
            _chatBoxBackground.sprite = talkingBackgroundSprite;
            Debug.Log(msgChatBox);
            isWriting = false;
            isTalking = true;
            _currTalkTime = maxTalkTime;
        }
        else if(isWriting && Input.GetKeyDown(KeyCode.Escape))
        {
            _chatBox.SetActive(false);
            msgChatBox = "";
            isWriting = false;
            isTalking = false;
        }

        if (isTalking)
        {
            _currTalkTime -= Time.deltaTime;
            if (_currTalkTime <= 0.0f)
            {
                isTalking = false;
                _chatBox.SetActive(false);

            }
        }
        if (isWriting)
        {
            if (!Input.GetKey(KeyCode.Backspace))
                msgChatBox += Input.inputString;
            else if (Input.GetKeyDown(KeyCode.Backspace) && msgChatBox.Length > 1)
                msgChatBox = msgChatBox.Remove(msgChatBox.Length - 1);
            _chatBoxTextPopUp.text = msgChatBox;
            _chatBoxBackground.size = new Vector2((_chatBoxTextPopUp.bounds.size.x * 0.01f) + 0.02f, 0.2f);
            
        }

    }
}
