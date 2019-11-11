using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class jsonConverter{
    public bool isSameMessage (Message _message2Send, Message _pastMessage) {
        bool sameMessage = true;
        for (int i = 0; i < _message2Send.lights.Length; i++) {
            if (!_message2Send.lights[i].Equals(_pastMessage.lights[i])) {
                sameMessage = false;
                _message2Send.lights[i].update = true;
                
            }   
        }
        return sameMessage;

    }
    public void getDiffernces (HueLight currentLight, HueLight pastLight) {
        currentLight.compareLight(pastLight);
    }
    public string stateOfLights (HueLight currentlight, HueLight pastlight, int id) {
        string JSONToSend = "";
        getDiffernces(currentlight, pastlight);

        for (int i = 0; i < currentlight.elementsToUpdate.Count; i++) {
            if (i < currentlight.elementsToUpdate.Count - 1) {
                JSONToSend += currentlight.elementsToUpdate[i] + ",";
            } else {
                JSONToSend += currentlight.elementsToUpdate[i];
            }  
        }
        currentlight.elementsToUpdate.Clear();
        return "{" + JSONToSend + "}";
    }
}
