using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MangerSound : MonoBehaviour
{

    private AudioClip puzzleSolved;
    private void Start()
    {
        puzzleSolved = Resources.Load<AudioClip>("Sounds/puzzle_solved");
    }
    public AudioClip PuzzleSolved()
    {
        return puzzleSolved;
    }
}
