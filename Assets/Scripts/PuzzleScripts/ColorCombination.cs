using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCombination : MonoBehaviour
{
    public GameObject[] lights;

    public GameObject soundManager;

    private MangerSound sm;

    public TrapDoor door;
    public GameObject player;


    private float checkConditions = 0f;

    private bool puzzleSolved = false;


    private void Start()
    {
        sm = soundManager.GetComponent<MangerSound>();
    }
    void Update()
    {
        checkConditions += Time.deltaTime;

        if(checkConditions >= .3f && !puzzleSolved)
        {
            CheckPuzzleSolved();
            checkConditions = 0f;
        }
            
        
    }



    private void CheckPuzzleSolved()
    {
        if(lights[0].GetComponent<Light>().intensity > 0 && lights[1].GetComponent<Light>().intensity > 0)
        {
            for (int i = 2; i < lights.Length; i++)
            {
                if (lights[i].GetComponent<Light>().intensity != 0)
                    return;
            }

            door.GetComponent<TrapDoor>().openDoor();
            puzzleSolved = true;

            AudioSource aSource = player.GetComponent<AudioSource>();

            aSource.clip = sm.PuzzleSolved();
            aSource.Play();
        }
    }



}
