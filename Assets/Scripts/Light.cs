using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Light
{
    public Light()
    {
        on = false;
        hue = 1000;
        sat = 100;
        bri = 200;
        transitiontime = 0;
    }
    public bool on;
    public int hue, sat, bri;
    public int transitiontime;
    public void setvalues(bool _on, int _hue, int _sat, int bri, int _transitiontime)
    {
        on = _on;
        hue = _hue;
        sat = _sat;
        this.bri = bri;
        transitiontime = _transitiontime;
    }


}
