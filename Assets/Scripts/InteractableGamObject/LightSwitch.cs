using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public enum TurnOffLightOptions { TurnOffInstant, DontTurnOff, TurnOffWithFlickAll, TurnOffWithFlick1By1, FadeInNOut };

public class LightSwitch : MonoBehaviour, ItemInterface
{

    public GameObject[] lights;

    public float flickTimer;
    private float flickTimerDecreasing;
    public float flickerOutTroTime = 2f;
    float flickerOutTroCounter = 0f;
    public bool canBeTurnedOff;

    private bool lightsOn = false;
    private bool flickAllLightsOneByOne = false;
    private bool fadeInNOut = false;


    public TurnOffLightOptions TurnOffOptions;

    public float lightUpTimeInSeconds;
    private float upTimeTimer = 0;
    private bool flickAllLights = false;

    void ItemInterface.ItemEnteractedWith()
    {
        SwitchClicked();
    }

    void Start()
    {
        flickTimerDecreasing = flickTimer;
    }


    bool calledTurnOffLights = false;
    void Update()
    {
        if (lightsOn)
        {

            if (upTimeTimer >= lightUpTimeInSeconds && !calledTurnOffLights){
                calledTurnOffLights = true;
                TurnOfLights();
            }

            if (flickAllLights)
                FlickAllLights();
            

            else if (flickAllLightsOneByOne)
                FlickAllLightsOneByOne();

            else if (fadeInNOut)
            {
                FadeInAndOut();
            }

            upTimeTimer += Time.deltaTime;
        }
      
    }

    bool lit = true;
    float timer = 0f;
    private void FlickAllLights()
    {
        flickerOutTroCounter += Time.deltaTime;
        timer += Time.deltaTime;
        decreasingFlickerTime();

        if (flickerOutTroCounter >= flickerOutTroTime){
            ResetLights();
            return;
        }

        if (timer >= flickTimerDecreasing)
        {
            foreach (GameObject l in lights){
                l.GetComponent<UnityEngine.Light>().intensity = (lit) ? 0 : 1;             
            }
            lit = !lit;
            timer = 0;
        }
    }

    int lightCounter = 0;
    float switchToNewLight = 0;
    private void FlickAllLightsOneByOne()
    {

        switchToNewLight += Time.deltaTime;
        timer += Time.deltaTime;  

        if (timer >= flickTimer)
        {    
            lights[lightCounter].GetComponent<UnityEngine.Light>().intensity = (lit) ? 0 : 1;          
            lit = !lit;
            timer = 0;
        }

        if(switchToNewLight >= flickerOutTroTime)
        {
            lightCounter++;
            switchToNewLight = 0f;
        }

        if(lightCounter == lights.Length)
        {
            lightCounter = 0;
            ResetLights();
        }
    }

    float decreasePercentage = .01f;
    float ints;
    private void FadeInAndOut()
    {

        timer += Time.deltaTime;
        
            if (timer >= flickerOutTroTime * 0.01){
                foreach (GameObject l in lights)
                {
                UnityEngine.Light light = l.GetComponent<UnityEngine.Light>();

                light.intensity -=  decreasePercentage;

                    ints = light.intensity;
                }
                timer = 0f;
                if(ints <= 0)
                {
                    timer = 0f;
                    fadeInNOut = false;
                    ResetLights();
                }

            }
    }


    float decreasingFlickerTimer = 0f;
    private void decreasingFlickerTime()
    {
        if (decreasingFlickerTimer >= flickerOutTroTime * 0.1){
            flickTimerDecreasing -= (flickTimer * 0.11f);
            decreasingFlickerTimer = 0f;
        }

        decreasingFlickerTimer += Time.deltaTime;
    }

    public void SwitchClicked()
    {
        if (lightsOn && canBeTurnedOff){
            ResetLights();            
        }
        else if (!lightsOn){
            foreach (GameObject l in lights)
            {
                l.GetComponent<UnityEngine.Light>().intensity = 1;
            }
            lightsOn = true;
        }
    }

    public void TurnOfLights(){
        if (TurnOffOptions == TurnOffLightOptions.TurnOffInstant){
            ResetLights();
        }

        else if(TurnOffOptions == TurnOffLightOptions.DontTurnOff){
            upTimeTimer = 0;
            return;
        }

        else if(TurnOffOptions == TurnOffLightOptions.TurnOffWithFlickAll){
            flickAllLights = true;
        }

        else if(TurnOffOptions == TurnOffLightOptions.TurnOffWithFlick1By1)
        {
            flickAllLightsOneByOne = true;
        }
        else if(TurnOffOptions == TurnOffLightOptions.FadeInNOut)
        {
            fadeInNOut = true;
        }
    }

    private void TurnOfLightsInstant()
    {
        foreach (GameObject l in lights)
        {
            l.GetComponent<UnityEngine.Light>().intensity = 0;
        }
    }

    public void ResetLights()
    {
        TurnOfLightsInstant();
        lightsOn = false;
        upTimeTimer = 0;
        flickAllLights = false;
        flickAllLightsOneByOne = false;
        flickerOutTroCounter = 0f;
        decreasingFlickerTimer = 0f;
        flickTimerDecreasing = flickTimer;
        calledTurnOffLights = false;
    }
}
