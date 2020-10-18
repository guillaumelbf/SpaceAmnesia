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

    public void changeRoom(int newRoom)
    {
        currentRoom = (EGameRoom)newRoom;
        if(newRoom == 1)
            audioSource1.mute = false;
        if (newRoom == 2)
            audioSource2.mute = false;
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
