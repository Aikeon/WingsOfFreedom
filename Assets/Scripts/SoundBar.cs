using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundBar : MonoBehaviour
{
    private Slider slider;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        audioSource = GameManager.Instance.audioManager.sounds[0].source;
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = slider.value;
    }
}
