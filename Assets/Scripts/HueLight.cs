using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class HueLight
{
    public bool on, update, changeStatus;
    public int hue, sat, bri;
    public int transitiontime;
    public List <string> elementsToUpdate;
    
    public HueLight() {
        on = true;
        update = true;
        hue = 1000;
        sat = 100;
        bri = 200;
        transitiontime = 0;

        elementsToUpdate = new List<string>();
    }

    public override bool Equals(object obj) {
        if (obj == null || !(obj is HueLight)) {
            return false;
        }

        if (this.hue == ((HueLight)obj).hue) {
            // Debug.Log("hue NOT changed");
        }else{
            // Debug.Log("hue changed!");
        }

        changeStatus = 
                    this.on == ((HueLight)obj).on 
                && this.hue == ((HueLight)obj).hue 
                && this.sat == ((HueLight)obj).sat 
                && this.bri == ((HueLight)obj).bri 
                && this.transitiontime == ((HueLight)obj).transitiontime;

        update = changeStatus;
        // Debug.Log("Change Statis: " + changeStatus);
        return changeStatus;
    }

    public void compareLight (HueLight prevLight) {
        if (!changeStatus) {
            if (this.on != (prevLight).on) {
                // elementsToUpdate.Add("\"on\":" + this.on.ToString().ToLower());
            }
            if (this.hue != (prevLight).hue) {
                elementsToUpdate.Add("\"hue\":" + this.hue);
            }

            if (this.sat != (prevLight).sat) {
                elementsToUpdate.Add("\"sat\":" + this.sat);
            }

            if (this.bri != (prevLight).bri) {
                elementsToUpdate.Add("\"bri\":" + this.bri);
            }
            if (this.transitiontime != (prevLight).transitiontime) {
                elementsToUpdate.Add("\"transitiontime\":" + this.transitiontime);
            }

        }
    }
 
    public void setvalues(bool _on, int _hue, int _sat, int _bri, int _transitiontime)
    {
        on = _on;
        hue = _hue;
        sat = _sat;
        bri = _bri;
        transitiontime = _transitiontime;
    }

    public void transferValues (HueLight light) {
        on = light.on;
        hue = light.hue;
        sat = light.sat;
        bri = light.bri;
        transitiontime = light.transitiontime;
    }
    public void transferOnValues (HueLight light) {
        on = light.on;
    }



}
