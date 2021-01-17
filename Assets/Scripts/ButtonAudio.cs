using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAudio : MonoBehaviour
{
    GameObject buttonGameobject;
    AudioSource buttonAudioSource;
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        buttonGameobject = this.gameObject;
        buttonAudioSource = buttonGameobject.GetComponent<AudioSource>();
        button = buttonGameobject.GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        buttonAudioSource.Play();
    }
 
    

}

