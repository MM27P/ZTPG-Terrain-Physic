using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Infection : MonoBehaviour, IColisionDecorator
{
    public float CurrentLevel;
    public float DeathTreshHold = 100;
    public float InfectionSpeed = 1;
    public float maxColorValue=1;
    public int Priority;
    public bool PatientZero;


   // Color currentColor;
    // Start is called before the first frame update
    void Start()
    {
        CurrentLevel = 0;
        //currentColor = GetComponentInChildren<Renderer>().material.GetColor("_EmissionColor");
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentLevel > DeathTreshHold && !PatientZero)
        {
            Destroy(gameObject);
        }
        CurrentLevel += (InfectionSpeed * Time.deltaTime);

        //Material mymat = GetComponent<Renderer>().material;
        //if(currentColor!= null)
        //{
        //    ChangeColor(CurrentLevel);
        //    mymat.SetColor("_EmissionColor", currentColor);
        //}      
    }

    //public void ChangeColor(float currentLevel)
    //{
    //    int segment = (int)currentLevel / ((int)DeathTreshHold / 4);
    //    float rest = (int)currentLevel % ((int)DeathTreshHold / 4);
    //    switch (segment)
    //    {
    //        case 0:
    //            currentColor = new Color(0, rest * maxColorValue / ((int)DeathTreshHold / 4), maxColorValue);
    //            break;
    //        case 1:
    //            currentColor = new Color(0, maxColorValue, maxColorValue - rest * maxColorValue / ((int)DeathTreshHold / 4));
    //            break;
    //        case 2:
    //            currentColor = new Color( rest * maxColorValue / ((int)DeathTreshHold / 4), maxColorValue, 0);
    //            break;
    //        default:
    //            currentColor = new Color(maxColorValue, maxColorValue - rest * maxColorValue / ((int)DeathTreshHold / 4), 0);
    //            break;

    //    }
    //}

    public int GetPriority()
    {
        return Priority;
    }

    public bool IsTrigger()
    {
        return true;
    }

    public void StartInteraction(GameObject root, GameObject other)
    {
        var unit = other.GetComponent<UnitAI>();
        var player = other.GetComponent<PlayerControler>();
        var infection = other.GetComponent<Infection>();
        if ((unit != null || player != null) && infection==null)
        {
            other.AddComponent<Infection>();
        }
    }

    public void UpdateInteraction()
    {

    }

    public void EndInteraction(GameObject root, GameObject other)
    {

    }
}
