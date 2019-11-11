using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orientation : MonoBehaviour
{
    public Camera camera;
    public PseudoLight[] pseudoLights;
    public Bulb[] bulbs;
    public bool enableDebug = true;
    public int hueUpdateRate = 10;
    

    // Start is called before the first frame update
    void Start() {
        pseudoLights = FindObjectsOfType<PseudoLight>();
        bulbs = FindObjectsOfType<Bulb>();
        if(hueBridgeLinker.bridgeLinked) {
            foreach (var bulb in bulbs) {
                bulb.enabled = false;
            }
        }
        HueMessenger.MessageFunctionReferences.Add(
            new HueMessenger.MessageWrapper(1, CalcDirectionalLight)
        );
    }

    Message CalcDirectionalLight() {
        Vector3 playerPosition = camera.transform.position;
        Message HueMessenge = new Message();
        for (int l = 0; l < bulbs.Length; l++) {
            // Each bulb:
            bulbs[l].color = Color.black;
            List<Color> colors = new List<Color>();     
            Vector3 bulbOrientation = Vector3.Normalize(bulbs[l].transform.position - playerPosition);

            for (int i = 0; i < pseudoLights.Length; i++) {
                float distanceFactor = calculateDistanceFactor(playerPosition, pseudoLights[i].transform.position, pseudoLights[i].outerDistanceThreshold, pseudoLights[i].innerDistanceThreshold);
                if(distanceFactor > 0) {
                    distanceFactor = pseudoLights[i].IntensityCurve.Evaluate(distanceFactor);
                    pseudoLights[i].distanceToPlayer = Vector3.Distance(playerPosition, pseudoLights[i].transform.position);
                    Vector3 objectOrientation = Vector3.Normalize(pseudoLights[i].transform.position - playerPosition);
                    float simularity = (calculateSimularity(objectOrientation, bulbOrientation)); 
                    Color colorWeight = pseudoLights[i].LightColor * pseudoLights[i].intensity * simularity * distanceFactor;
                    colors.Add(colorWeight);
                    if(enableDebug){
                        debug(playerPosition, pseudoLights[i].transform.position);
                    }
                }
            }

            Color avgColor = new Color(0, 0, 0);
            foreach(Color c in colors) {
                avgColor += c;
            }
            avgColor = (avgColor / pseudoLights.Length) * pseudoLights.Length;
            bulbs[l].color = avgColor;

            float h, s, v;
            Color.RGBToHSV(avgColor, out h, out s, out v);

            h *= 65535.0f;
            s *= 254.0f;
            v *= 254.0f;
            bool onstate = v > 5.0f;

            if(hueBridgeLinker.bridgeLinked) {
                HueMessenge.lights[l].setvalues(true, (int)h, (int)s, (int)v, 0);
                HueMessenge.lights[l].on = onstate;
                // HueMessenge.isActive = onstate;
            }
        }
        return HueMessenge;
    }

    void debug(Vector3 _playerPosition, Vector3 _objectOrientation) {
        Vector3 down = new Vector3(0, -1, 0);
        Debug.DrawLine(_playerPosition + down,  _playerPosition, Color.red, Time.deltaTime);
        Debug.DrawLine(_playerPosition + down, _objectOrientation, Color.blue, Time.deltaTime);
    }

    float calculateSimularity(Vector3 lamp, Vector3 dir) {
        return (Mathf.Max(Vector3.Dot(lamp, dir), 0f));
    }

    float calculateDistanceFactor (Vector3 from, Vector3 to, float distanceThreshold, float innerDistanceThreshold) {
        return Mathf.Clamp(Remap(Vector3.Distance(from, to), distanceThreshold, innerDistanceThreshold, 0f, 1f), 0f, 1f);
    }
    public float Remap (float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
