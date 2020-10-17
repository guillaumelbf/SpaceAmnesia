using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] SceneAsset gameScene = null;

    [Header("MenuScreens")]
    [SerializeField] GameObject titleScreen = null;
    [SerializeField] GameObject optionScreen = null;

    [Header("TitleScreen")]
    [SerializeField] Button playButton = null;
    [SerializeField] Button optionButton = null;
    [SerializeField] Button quitButton = null;

    [Header("OptionScreen")]
    [SerializeField] Slider masterVolume = null;
    //[SerializeField] Slider fxVolume = null;
    [SerializeField] Button backButton = null;
    [SerializeField] AudioMixer masterVolumeAudio = null;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(loadMainScene);
        optionButton.onClick.AddListener(showOptions);
        quitButton.onClick.AddListener(quitScene);

        backButton.onClick.AddListener(showTitleScreen);
        masterVolume.onValueChanged.AddListener(changeMasterValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void loadMainScene()
    {
        SceneManager.LoadScene(gameScene.name);
    }

    void showOptions()
    {
        titleScreen.SetActive(false);
        optionScreen.SetActive(true);
    }

    void showTitleScreen()
    {
        titleScreen.SetActive(true);
        optionScreen.SetActive(false);
    }

    void quitScene()
    {
        Application.Quit();
    }

    void changeMasterValue(float sliderValue)
    {
        masterVolumeAudio.SetFloat("MasterVolume",Mathf.Log10(sliderValue) * 20);
    }
}
