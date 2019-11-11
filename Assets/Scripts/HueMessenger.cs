using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;

public class HueMessenger : MonoBehaviour
{
    jsonConverter jsonConv;    
    
    public static Message currentMessage, pastMessage, defaultMessage;
    bool readyForChange = true;

    // Start is called before the first frame update




    void Start() {
        StartCoroutine(LateStart(0.5f));

        jsonConv = new jsonConverter();
        currentMessage = new Message();
        pastMessage = new Message();

        pastMessage.lights[0].on = false;
        pastMessage.lights[1].on = false;
        pastMessage.lights[2].on = false;
    }   

    float lightCounter = 0;
    float messagesPrSecond = 1 / 20;
    void Update() {
        lightCounter += Time.deltaTime;
        if (hueBridgeLinker.bridgeLinked && readyForChange && lightCounter >= messagesPrSecond) {
            foreach (var letter in MessageFunctionReferences) {
                Message MessageWithPriority = letter.calculateHueMessage(); // notice the break further down. the first message that is active will be chosen as higest priority
                if(MessageWithPriority.isActive) { // if the first message (sorted by priority) is active
                    currentMessage = MessageWithPriority; // set as currentMessage
                    // print("higest priocolor: " + letter.prio + MessageWithPriority);
                    if(!jsonConv.isSameMessage(currentMessage, pastMessage)) { // if the new message differ from the last
                        StartCoroutine("updateLights"); // Fire the calls
                        lightCounter =  0;
                    }
                    break;
                }
            }            
        } 
    }


    class GFG : IComparer<MessageWrapper> { 
        public int Compare(MessageWrapper x, MessageWrapper y) {
            if (x.prio == 0 || y.prio == 0) { 
                return 0; 
            } 
            return y.prio.CompareTo(x.prio); 
        }
    } 
    GFG sortByLetterPrio = new GFG();
    public struct MessageWrapper {
        public int prio;
        public Func<Message> calculateHueMessage;
        public MessageWrapper(int _prio, Func<Message> _myFunc) {
            prio = _prio;
            calculateHueMessage = _myFunc;
        }
    }
    static public List<MessageWrapper> MessageFunctionReferences = new List<MessageWrapper>();
    
    IEnumerator LateStart(float waitTime) {
        print(MessageFunctionReferences.Count);
        yield return new WaitForSeconds(waitTime);
        MessageFunctionReferences.Sort(sortByLetterPrio);
    }

    IEnumerator updateLights () {
        readyForChange = false;
        int count = 1;
        bool didFail = false;

        foreach (HueLight light in currentMessage.lights) { 
            if (light.update) {
                HueLight prevLight = pastMessage.lights[count-1];
                string urlPath = hueBridgeLinker.currentBridge.internalipaddress + "/api/" + hueBridgeLinker.currentBridge.username + "/lights/" + count + "/state";

                if(light.on != prevLight.on){
                    print("SWITCHED LIGHT STATUS FROM " + prevLight.on + " TO " + light.on);
                    UnityWebRequest lightOnRequest = UnityWebRequest.Put(urlPath, System.Text.Encoding.UTF8.GetBytes("{\"on\":" + light.on.ToString().ToLower() + "}"));
                    yield return lightOnRequest.SendWebRequest();
                    yield return new WaitUntil (() => lightOnRequest.downloadProgress >= 1);
                    print(lightOnRequest.downloadHandler.text);
                    prevLight.transferOnValues(light);
                }

                if(light.on){
                    byte[] myData = System.Text.Encoding.UTF8.GetBytes(jsonConv.stateOfLights(light, prevLight, count));                
                    UnityWebRequest parameterRequest = UnityWebRequest.Put(urlPath, myData);
                    yield return parameterRequest.SendWebRequest();
                    yield return new WaitUntil (() => parameterRequest.downloadProgress >= 1);
                    didFail = parameterRequest.isHttpError || didFail;
                    print(parameterRequest.downloadHandler.text);
                    prevLight.transferValues(light);
                    light.update = false;
                }
            } 
            count++;
        }
        readyForChange = true;
    }


}


