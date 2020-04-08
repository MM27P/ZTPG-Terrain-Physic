using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collecter : MonoBehaviour
{
    public int CurrentCollected = 0;

    public void Add()
    {
        CurrentCollected++;
    }
}
