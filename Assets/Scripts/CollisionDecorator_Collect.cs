using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionsManager))]
[RequireComponent(typeof(Collecter))]
public class CollisionDecorator_Collect : MonoBehaviour, IColisionDecorator
{
    public int Priority = 1;
    private Collecter collecter;

    public void EndInteraction(GameObject root, GameObject other)
    {

    }

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
        Collectable collectableItem = other.GetComponent<Collectable>();

        if (collectableItem != null)
        {
            collecter.Add();
            collectableItem.Collect();
        }
    }

    public void UpdateInteraction()
    {

    }

    void Start()
    {
        collecter = GetComponent<Collecter>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
