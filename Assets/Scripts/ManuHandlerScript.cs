using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManuHandlerScript : MonoBehaviour
{
    GameObject mainMenu;
    GameObject settingsMenu;
    // Start is called before the first frame update
    void Start()
    {
        //On start deactivate settings menu and activate main menu
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    void OnStartButtonPressed()
    {
        //After start button pressed load first game scene
    }

    void OnSettingsButtonPressed()
    {
        //After settings button pressed activate settingsMenu and deactivate mainMenu
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    
}
