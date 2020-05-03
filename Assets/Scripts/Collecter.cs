using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collecter : MonoBehaviour
{
    public int CurrentCollected = 0;
    public bool VisionEnable = false;

    public void Add()
    {
        CurrentCollected++;
    }

    public void ActiveVision()
    {
        VisionEnable = true;
    }
}
