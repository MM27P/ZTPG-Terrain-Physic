using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAI : MonoBehaviour
{
    public float CurrentLevel;
    public float DeathTreshHold = 100;
    public float InfectionSpeed = 1;
    public NavMeshAgent agent;
    public Terrain terrain;

    private float minX = 0;
    private float minY = 0;
    private float maxX = 0;
    private float maxY = 0;
    private float minTime = 5f;
    private float maxTime = 50;
    public float TimeToChangeDecision;
    public float currentTime = 0;
    public Vector3 newPosition;
    public List<GameObject> list;
    public int currentIndex = 0;
    public bool IsPatrol = false;

    // Start is called before the first frame update
    void Start()
    {
        minX = terrain.transform.position.x;
        minY = terrain.transform.position.y;
        maxX = terrain.terrainData.size.x;
        maxY = terrain.terrainData.size.y;
        maxX = 100;
        maxY = 100;
        currentTime = 0;
        TimeToChangeDecision = Random.Range(minTime, maxTime);
        agent = GetComponent<NavMeshAgent>();

        CurrentLevel = 0;
        if (IsPatrol)
        {
            currentIndex = Random.Range(0, list.Count - 1);

            agent.isStopped = true;
            agent.destination = list[currentIndex].transform.position;
            agent.isStopped = false;
        }
        else
        {
            currentIndex = 0;

            agent.isStopped = true;
            agent.destination = list[currentIndex].transform.position;
            agent.isStopped = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChooseNextPoint();
    }

    public void ChooseNextPoint()
    {
        if (IsPatrol)
        {
            if (Vector3.Distance(list[currentIndex].transform.position, transform.position) < 10)
            {
                currentIndex++;
                if (currentIndex >= list.Count)
                {
                    currentIndex = 0;
                }

                agent.isStopped = true;
                agent.destination = list[currentIndex].transform.position;
                agent.isStopped = false;
            }
        }
        else
        {
            if (currentTime >= TimeToChangeDecision)
            {
                currentTime = 0;
                currentIndex = Random.Range(0, list.Count - 1);

                agent.isStopped = true;
                agent.destination = list[currentIndex].transform.position;
                agent.isStopped = false;
            }
            currentTime += Time.deltaTime;
        }
    }
}
