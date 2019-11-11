using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class HueMessenger : MonoBehaviour
{
    jsonConverter jsonConv;    
    
    public static Message currentMessage, pastMessage, defaultMessage;

    bool lightChange = false;

    // Start is called before the first frame update
    void Start() {
        jsonConv = new jsonConverter();
        currentMessage = new Message(1);
       
        pastMessage = new Message(2);

        pastMessage.lights[0].on = false;
        pastMessage.lights[1].on = false;
        pastMessage.lights[2].on = false;

       // StartCoroutine("updateLights"); // Set the initial state of the bulbs.

    }   

    float lightCounter = 0;
    int messagesPrSecond = 1/ 20;
    void Update() {

        lightCounter += Time.deltaTime;


          if (hueBridgeLinker.bridgeLinked && !jsonConv.isSameMessage(currentMessage, pastMessage) && !lightChange && lightCounter >= messagesPrSecond)  {
            lightCounter =  0;

            StartCoroutine("updateLights");
        } 
    }

    IEnumerator updateLights () {

        lightChange = true;
        int count = 1;

        foreach (HueLight _light in currentMessage.lights)
        {
          
            if (_light.update) {
                string urlPath = hueBridgeLinker.currentBridge.internalipaddress + "/api/" + hueBridgeLinker.currentBridge.username + "/lights/" + count + "/state";
                byte[] myData = System.Text.Encoding.UTF8.GetBytes(jsonConv.stateOfLights(_light,pastMessage.lights[count-1],count));
                pastMessage.lights[count-1].transferValues(_light);    
                _light.update = false;

                UnityWebRequest www1 = UnityWebRequest.Put(urlPath, myData);
                yield return www1.SendWebRequest();
                yield return new WaitUntil (()=> www1.downloadProgress >= 1);
                           
            } 

            count++;
        }
        lightChange = false;
    }

}
