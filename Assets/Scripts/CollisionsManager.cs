using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CollisionsManager : MonoBehaviour
{
    [SerializeField]
    private List<IColisionDecorator> collisionsInteractions;
    [SerializeField]
    private List<IColisionDecorator> triggersInteractions;
    // Start is called before the first frame update
    void Start()
    {
        var allDecorators = GetComponents<IColisionDecorator>();
        collisionsInteractions = allDecorators.Where(x => !x.IsTrigger()).OrderBy(x => x.GetPriority()).ToList();
        triggersInteractions = allDecorators.Where(x => x.IsTrigger()).OrderBy(x => x.GetPriority()).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var collisionDecorator in collisionsInteractions)
        {
            collisionDecorator.UpdateInteraction();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (var collisionDecorator in collisionsInteractions)
        {
            collisionDecorator.StartInteraction(gameObject, collision.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        foreach (var triggerInteraction in triggersInteractions)
        {
            triggerInteraction.StartInteraction(gameObject, other.gameObject);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        foreach (var triggerInteraction in triggersInteractions)
        {
            triggerInteraction.StartInteraction(gameObject, collision.gameObject);
        }
    }
}
