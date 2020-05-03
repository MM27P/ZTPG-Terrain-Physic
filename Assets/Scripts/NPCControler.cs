using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCControler : MonoBehaviour
{
    public GameObject[] NPCCollections;
    private List<GameObject> NPCList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        NPCList = new List<GameObject>();
        foreach (var collection in NPCCollections)
        {
            NPCList.AddRange(collection.GetComponentsInChildren<GameObject>().ToList());
        }
    }

    public void TurnOnInfectionVision()
    {
        foreach (var npc in NPCList)
        {
            var infectionComponent = npc.GetComponent<Infection>();
            infectionComponent.EnableVision();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
