using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuOptions : MonoBehaviour
{
    public GameObject mainMenu;
    public int playerControllerNumber;

    public GameObject[] sliderSelectionBoxes;
    public Slider activeSlider;

    public AudioMixer audioMixer;

    public float timeBeforeInput = .5f;
    public float changePerTick = 2;

    private float time = 0;
    private int index = 0;

    private void Start()
    {
        activeSlider = sliderSelectionBoxes[index].GetComponentInParent<Slider>();
    }

    private void Update()
    {
        float UIHorAxis = Input.GetAxis("UIHorizontal" + playerControllerNumber);
        float UIVerAxis = Input.GetAxis("UIVertical" + playerControllerNumber);

        if (UIHorAxis != 0)
        {
            time += Time.deltaTime;
            if(time > timeBeforeInput)
            {
                if (UIHorAxis > 0)
                {
                    activeSlider.value += changePerTick;
                }
                else
                {
                    activeSlider.value -= changePerTick;
                }

                time = 0;
            }
        }
        else if (UIVerAxis != 0)
        {
            time += Time.deltaTime;
            if(time > .25f)
            {
                if(UIVerAxis > 0)
                {
                    sliderSelectionBoxes[index].SetActive(false);
                    index++;
                    if(index == sliderSelectionBoxes.Length)
                    {
                        index = 0;
                    }                    
                    sliderSelectionBoxes[index].SetActive(true);
                }
                else if(UIVerAxis < 0)
                {
                    sliderSelectionBoxes[index].SetActive(false);

                    if(index == 0)
                    {
                        index = sliderSelectionBoxes.Length - 1;
                    }
                    else
                    {
                        index--;
                    }
                    sliderSelectionBoxes[index].SetActive(true);
                }
                activeSlider = sliderSelectionBoxes[index].GetComponentInParent<Slider>();
                time = 0;
            }
        }


        if (Input.GetButtonDown("Special" + playerControllerNumber))
        {
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }    
    public void SetBackgroundVolume(float volume)
    {
        audioMixer.SetFloat("BackgroundVolume", volume);
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }


}
