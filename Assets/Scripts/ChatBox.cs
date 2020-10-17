using System.Collections;
using System.Collections.Generic;
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
    private TextMesh   _chatBoxTextPopUp;

    private SpriteRenderer _chatBoxBackground;
    public Sprite thinkingBackgroundSprite;
    public Sprite talkingBackgroundSprite;

    [SerializeField] public float maxTalkTime;
    private float _currTalkTime;

    // Start is called before the first frame update
    void Start()
    {
        isWriting = false;
        isTalking = false;
        _currTalkTime = maxTalkTime;

        /*
         *  Setup chatbox components and objects in the script to avoid
         *     catching them at each frame
         */
        
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("ChatBox"))
            {
                _chatBox = child.gameObject;
                break;
            }
        }

        if (_chatBox == null)
            return;
        _chatBox = transform.GetChild(0).gameObject;
        _chatBoxBox = transform.GetChild(0).GetChild(0).gameObject;
        _chatBoxBackground = _chatBoxBox.GetComponent<SpriteRenderer>();
        _chatBoxTextPopUp = transform.GetChild(0).GetComponent<TextMesh>();
        _chatBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
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
            msgChatBox += Input.inputString;
            _chatBoxBox.transform.localScale =
                new Vector3((_chatBoxTextPopUp.GetComponent<Renderer>().bounds.size.x + 1) * 75, 50, 1); 
            _chatBoxTextPopUp.text = msgChatBox;
            if (Input.GetKeyDown(KeyCode.Backspace) && msgChatBox.Length > 1)
                msgChatBox = msgChatBox.Remove(msgChatBox.Length - 2);
        }

    }
}
