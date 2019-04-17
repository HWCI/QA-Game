using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Job : MonoBehaviour
{
    public enum JobType
    {
        A,
        B,
        C
    }
    public int design;
    public int prgm;

    private void OnEnable()
    {
        int rnd = Random.Range(0, 8);
        int prnd = Random.Range(0,6);
        design = rnd;
        prgm = Math.Abs(8 - rnd);
    }
}
