using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandlerScript : MonoBehaviour
{
    [SerializeField]GameObject mainMenu;
    [SerializeField]GameObject settingsMenu;
    AudioSource menuButtonsAudio;
    [SerializeField]Slider volumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        //On start deactivate settings menu and activate main menu
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        menuButtonsAudio = GetComponent<AudioSource>();
    }

    public void OnStartButtonPressed()
    {
        //After start button pressed load first game scene]
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void OnSettingsButtonPressed()
    {
        //After settings button pressed activate settingsMenu and deactivate mainMenu
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    public void OnBackButtonPressed()
    {
        //After settings button pressed activate settingsMenu and deactivate mainMenu
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
    public void OnExitButtonPressed()
    {
        //After exit button is pressed exit game
        Application.Quit();
    }
    public void AudioOnButtonClicked()
    {
        //Plays "click" sound after button is pressed
        menuButtonsAudio.Play();
    }

   public void ChangeVol() {
        // Adjust audiolistener volume to the settings menus slider value
        AudioListener.volume = volumeSlider.value;
    }

}
