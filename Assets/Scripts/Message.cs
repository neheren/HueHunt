using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message {
    public HueLight[] lights;
    public bool isActive = true;
    public enum messageType {activation, colorChange, transition, setup}; // I still dont get it
    public Message () {
        lights = new HueLight[3];
        for (int i = 0; i < lights.Length; i++) {
            lights[i] = new HueLight();
        }
     }
}
