using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IColisionDecorator
{
    int GetPriority();
    void StartInteraction(GameObject root, GameObject other);
    void EndInteraction(GameObject root, GameObject other);
    void UpdateInteraction();
    bool IsTrigger();
}
