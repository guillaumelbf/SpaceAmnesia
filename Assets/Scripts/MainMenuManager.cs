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

    [Header("FadeScreen")]
    [SerializeField] Image fadeOutImage = null;
    [SerializeField] float fadeOutTime = 0;
    [SerializeField] float fadeInTime = 0;
    float maxFadeInTime = 0;
    float maxFadeOutTime = 0;

    bool menuIn = true;
    bool menuOut = false;


    // Start is called before the first frame update
    void Start()
    {
        // Init master volume with the slider
        masterVolumeAudio.SetFloat("MasterVolume",-50);

        maxFadeInTime = fadeInTime;
        maxFadeOutTime = fadeOutTime;

        playButton.onClick.AddListener(loadMainScene);
        optionButton.onClick.AddListener(showOptions);
        quitButton.onClick.AddListener(quitScene);

        backButton.onClick.AddListener(showTitleScreen);
        masterVolume.onValueChanged.AddListener(changeMasterValue);
    }

    // Update is called once per frame
    void Update()
    {
        if(menuIn)
        {
            fadeInTime -= Time.deltaTime;
            float currentVol;
            masterVolumeAudio.GetFloat("MasterVolume", out currentVol);

            fadeOutImage.color = new Color(0f, 0f, 0f, fadeInTime / maxFadeInTime); // Fade in screen
            masterVolumeAudio.SetFloat("MasterVolume", Mathf.Lerp(currentVol, sliderToVolume(masterVolume.value), fadeInTime)); // Fade in sound

            if (fadeInTime <= 0)
            {
                fadeOutImage.gameObject.SetActive(false);
                menuIn = false;
            }
        }

        if(menuOut)
        {
            fadeOutTime -= Time.deltaTime;
            float currentVol;
            masterVolumeAudio.GetFloat("MasterVolume",out currentVol);

            fadeOutImage.color = new Color(0f,0f,0f, (1 - (fadeOutTime / maxFadeOutTime))); // Fade out screen
            masterVolumeAudio.SetFloat("MasterVolume", Mathf.Lerp(currentVol, -50, 0.001f)); // Fade out sound

            if (fadeOutTime <= 0)
                SceneManager.LoadScene(gameScene.name);
        }
    }

    void loadMainScene()
    {
        menuOut = true;
        fadeOutImage.gameObject.SetActive(true);
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
        masterVolumeAudio.SetFloat("MasterVolume",sliderToVolume(sliderValue));
    }

    float sliderToVolume(float sliderValue)
    {
        return Mathf.Log10(sliderValue) * 20;
    }
}
