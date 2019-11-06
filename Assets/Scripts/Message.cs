using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message
{
    public Light[] lights;

    public int messagePriority;

    public Message (int _messagePriority)
    {
        lights = new Light[4];

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i] = new Light();
        }

        messagePriority = _messagePriority;

     }

}
