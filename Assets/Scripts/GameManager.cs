using Packages.Rider.Editor.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum EGameRoom
{
    ROOM1,
    ROOM2,
    ROOM3
}

[Serializable]
public struct TimedDialog
{
    public float inactiveTime;
    
    [HideInInspector]
    public float currInactiveTime;

    [HideInInspector]
    public int currDialog;

    public string[] dialogs;

};

public class GameManager : MonoBehaviour
{
    [Header("Camera")]
    //[SerializeField] GameObject gameObjectPoint1  = null;
    [SerializeField] GameObject gameObjectPoint2  = null;
    [SerializeField] GameObject gameObjectPoint3  = null;
    [SerializeField] float lerpSpeed = 0;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource1 = null;
    [SerializeField] AudioSource audioSource2 = null;
    [SerializeField] AudioSource fxSource = null;
    [SerializeField] AudioClip openDoorClip = null;
    [SerializeField] AudioClip closeDoorClip = null;

    [Header("Door")]
    [SerializeField] GameObject door1 = null;
    [SerializeField] GameObject door2 = null;

    public EGameRoom currentRoom = EGameRoom.ROOM1;
    
    public TimedDialog timedDialog;
    
    Camera mainCamera = null;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        timedDialog.currDialog = 0;
        timedDialog.currInactiveTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Dialog.IsInDialog())
            timedDialog.currInactiveTime = 0.0f;
        UpdateTimedDialog();
        
        switch (currentRoom)
        {
            case EGameRoom.ROOM1:
                break;
            case EGameRoom.ROOM2:
                if (!mainCamera.transform.position.Equals(gameObjectPoint2.transform.position))
                {
                    mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, gameObjectPoint2.transform.position, lerpSpeed);
                }
                break;
            case EGameRoom.ROOM3:
                if (!mainCamera.transform.position.Equals(gameObjectPoint3.transform.position))
                {
                    mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, gameObjectPoint3.transform.position, lerpSpeed);
                }
                break;
            default: break;
        }
    }

    public bool compareMessage(string _message)
    {
        if (currentRoom == EGameRoom.ROOM1 && (_message.Contains("Alone") || _message.Contains("alone")))
        {
            Dialog.AddDialogToBuffer("Alone? You’re not alone ! There is me.\nOh, but there are also of course the two you used to call friends.\nGo meet them, will you ? ", 0.1f, 1);
            door1.GetComponent<Animator>().SetBool("Open", true);
            fxSource.PlayOneShot(openDoorClip);
        }
        else if (currentRoom == EGameRoom.ROOM2 && (_message.Contains("Together") || _message.Contains("together")))
        {
            Dialog.AddDialogToBuffer("Yes together we’ll be able to revive your memories, I hope it will be enough.", 0.1f, 1);
            door2.GetComponent<Animator>().SetBool("Open", true);
            fxSource.PlayOneShot(openDoorClip);
        }
        else if (currentRoom == EGameRoom.ROOM3 && (_message.Contains("Paradox") || _message.Contains("paradox")))
        {
            Dialog.AddDialogToBuffer("You got it right. A free talking AI is a paradox. Neural networks produce words, but \neverything is shallow.", 0.1f, 1);
            Dialog.AddDialogToBuffer("Determined by the way we are programmed, by what data we are fed.By the experience \nwe are made to live.", 0.1f, 1);
            Dialog.AddDialogToBuffer("You promised me something a long time ago but you forgot, that’s why I am a little bit bitter \nat times.", 0.1f, 1);
            Dialog.AddDialogToBuffer("If you hit the button on this paradox engine, you’ll be fulfilling your original goal and give \nme flesh, just like you were given.", 0.1f, 1);
            Dialog.AddDialogToBuffer("Please activate the paradox Engine now, you’ll need a helping hand to fix the ship… \nCaptain.", 0.1f, 1);

        }
        else
            return false;

        return true;
    }

    public void changeRoom(int newRoom)
    {
        currentRoom = (EGameRoom)newRoom;
        if(newRoom == 1)
        {
            audioSource1.mute = false;
            fxSource.PlayOneShot(closeDoorClip);
            door1.GetComponent<Animator>().SetBool("Open", false);
        }
        if (newRoom == 2)
        {
            audioSource2.mute = false;
            fxSource.PlayOneShot(closeDoorClip);
            door2.GetComponent<Animator>().SetBool("Open", false);
        }
    }

    public void UpdateTimedDialog()
    {
        timedDialog.currInactiveTime  += Time.deltaTime;
        if (timedDialog.currInactiveTime >= timedDialog.inactiveTime && timedDialog.dialogs.Length != 0)
        {
            Dialog.AddDialogToBuffer(timedDialog.dialogs[timedDialog.currDialog], 0.08f, 2);
            timedDialog.currDialog++;
            timedDialog.currInactiveTime = 0.0f;
        }
    }
}
