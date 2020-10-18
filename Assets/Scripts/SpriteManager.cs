using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] List<SpriteRenderer> spriteList = null;

    [Header("Player")]
    [SerializeField] GameObject player = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        foreach(SpriteRenderer sprite in spriteList)
        {
            if(sprite.transform.position.y <= player.transform.position.y)
                sprite.gameObject.SetActive(true);
            else
                sprite.gameObject.SetActive(false);
        }
    }
}
