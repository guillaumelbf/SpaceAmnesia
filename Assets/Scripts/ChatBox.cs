using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour
{
    public bool isWriting;
    public bool isTalking;
    public string msgChatBox;
    public GameObject chatBox;
    public GameObject chatBoxBox;
    public TextMesh chatBoxTextPopUp;

    [SerializeField] public float maxTalkTime;
    private float currTalkTime;

    // Start is called before the first frame update
    void Start()
    {
        chatBox = transform.GetChild(0).gameObject;
        chatBoxBox = transform.GetChild(0).GetChild(0).gameObject;
        chatBoxTextPopUp = transform.GetChild(0).GetComponent<TextMesh>();
        chatBox.SetActive(false);
        isWriting = false;
        currTalkTime = maxTalkTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWriting && !isTalking && Input.GetKeyDown(KeyCode.Return))
        {
            chatBox.SetActive(true);
            msgChatBox = "";
            isWriting = true;
        }
        else if (isWriting && Input.GetKeyDown(KeyCode.Return))
        {
            chatBox.SetActive(false);
            Debug.Log(msgChatBox);
            isWriting = false;
            isTalking = true;
        }
        /*
        if (isTalking)
        {
            currTalkTime -= Time.deltaTime;
            if (currTalkTime <= 0.0f)
                isTalking = false
        }*/
        if (isWriting)
        {
            msgChatBox += Input.inputString;
            chatBoxBox.transform.localScale =
                new Vector3((chatBoxTextPopUp.GetComponent<Renderer>().bounds.size.x + 1) * 75, 50, 1); 
            chatBoxTextPopUp.text = msgChatBox;
            if (Input.GetKeyDown(KeyCode.Backspace) && msgChatBox.Length > 1)
                msgChatBox = msgChatBox.Remove(msgChatBox.Length - 2);
        }

    }
}
