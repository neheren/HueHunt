using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class HueMessenger : MonoBehaviour
{
    jsonConverter jsonConv;
    public int modFrame = 10;
    public static Message currentMessage, pastMessage, defaultMessage;


    bool lightChange = false;

    // Start is called before the first frame update
    void Start() {
        jsonConv = new jsonConverter();
        currentMessage = new Message(1);
        pastMessage = new Message(0);
        pastMessage = currentMessage;

        
        currentMessage.lights[0].on = true;
        currentMessage.lights[1].on = true;
        currentMessage.lights[2].on = true;
    }

    int lightState = 0;
    void Update() {

        if (!lightChange && Time.frameCount % modFrame == 0) {

            
            //singleLightUpdate();
            //updateLights();
            //StartCoroutine("updateScene");

            StartCoroutine("updateLights");
            
            lightState++;

            
        }
    }


     void singleLightUpdate () {
         for (int i = 0; i < 1; i++) {
            string urlPath = hueBridgeLinker.currentBridge.internalipaddress + "/api/" + hueBridgeLinker.currentBridge.username + "/lights/" + (i + 1) + "/state";
            byte[] myData = System.Text.Encoding.UTF8.GetBytes(jsonConv.stateOfLights(currentMessage.lights[i]));

            UnityWebRequest www1 = UnityWebRequest.Put(urlPath, myData);
                
//                print(jsonConv.stateOfLights(currentMessage.lights[i]));
            www1.SendWebRequest();
            }
    }


    IEnumerator updateLights () {

        lightChange = true;
        int count = 1;
        foreach (Light _light in currentMessage.lights)
        {
            string urlPath = hueBridgeLinker.currentBridge.internalipaddress + "/api/" + hueBridgeLinker.currentBridge.username + "/lights/" + count + "/state";
            byte[] myData = System.Text.Encoding.UTF8.GetBytes(jsonConv.stateOfLights(_light));
            UnityWebRequest www1 = UnityWebRequest.Put(urlPath, myData);
            www1.SendWebRequest();

            yield return new WaitForSeconds(0.1f);
            count++;
        }

        lightChange = false;
    }

    // IEnumerator updateScene () {

    //     lightChange = true;

    //     byte[] mySceneData = System.Text.Encoding.UTF8.GetBytes(jsonConv.updateScene(currentMessage.lights));
    //     UnityWebRequest updateRequest = UnityWebRequest.Put(hueBridgeLinker.updateScenePath, mySceneData);
       
    //     yield return updateRequest.SendWebRequest();

    //     yield return new WaitUntil (() => updateRequest.isDone);

    //     Debug.Log(updateRequest.downloadHandler.text);

    //     byte[] myDataGroup = System.Text.Encoding.UTF8.GetBytes("{\"scene\" : \"wLJTzQOhmxdfmkE\"}");
    //     updateRequest = UnityWebRequest.Put(hueBridgeLinker.updateGroupPath, myDataGroup);

    //     yield return updateRequest.SendWebRequest();

    //     Debug.Log(updateRequest.downloadHandler.text);

    //     yield return new WaitUntil (() => updateRequest.isDone);

    //     lightChange = false;



    // }

    // void updateLights () {
    //     byte[] mySceneData = System.Text.Encoding.UTF8.GetBytes(jsonConv.updateScene(currentMessage.lights));
    //     UnityWebRequest updateRequest = UnityWebRequest.Put(hueBridgeLinker.updateScenePath, mySceneData);
    //     updateRequest.SendWebRequest();
    
    //     byte[] myDataGroup = System.Text.Encoding.UTF8.GetBytes("{\"scene\" : \"wLJTzQOhmxdfmkE\"}");
    //     updateRequest = UnityWebRequest.Put(hueBridgeLinker.updateGroupPath, myDataGroup);
    //     updateRequest.SendWebRequest();
    // }





}
