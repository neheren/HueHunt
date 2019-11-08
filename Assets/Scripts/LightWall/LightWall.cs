using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightWall : MonoBehaviour
{


    public string text;
    private char[] textCharArray;
    public GameObject[] lights;

    private bool doSpellWord = true;
    private bool CalledOnce = false;

    private float lightTime = 0;

    private float timeBetweenLights = 10f;
    float halfTimeBetweenLights;

    private int wordIndexCounter = 0;

    void Start()
    {
        textCharArray = text.ToCharArray();
        halfTimeBetweenLights = timeBetweenLights / 2;
    }

    GameObject light;
    UnityEngine.Light  lightScript = null;
        
    void Update(){
        if (doSpellWord){
            if (CalledOnce == false){
                char letter = textCharArray[wordIndexCounter];
                light = FindLightWithLetter(letter);

                if(light == null)
                lightScript = light.GetComponent<UnityEngine.Light>();
              
                light.SetActive(true);
                lightTime = 0;
                CalledOnce = true;
            }

            lightTime += Time.deltaTime;

            if (lightTime < halfTimeBetweenLights)
                lightScript.intensity += Time.deltaTime;
            else
                lightScript.intensity -= Time.deltaTime;

            if(lightTime >= timeBetweenLights){
                light.SetActive(false);
                wordIndexCounter++;
                CalledOnce = false;

                //if (wordIndexCounter >= textCharArray.Length)
                   // doSpellWord = false;
            }       
        }
    }

    public GameObject FindLightWithLetter(char c){  
            for (int i = 0; i < lights.Length; i++){
                GameObject light = lights[i];

                if(light. name == c.ToString())            
                    return light;               
            }

        Debug.LogError("No such letter in the light: " + c);
            return null;
    }

}
