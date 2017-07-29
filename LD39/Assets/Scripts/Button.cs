using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
    public Activatable target;
    public bool fireOnce = false;

    bool hasFired;

    void OnTriggerEnter(Collider other)
    {
        if (target == null)
        {
            Debug.LogWarning("No target on button!");
        }
        else
        {
            Debug.Log("Button pressed!");
            if ((!fireOnce || !hasFired))
            {
                Debug.Log("Button activated!");
                target.Activate();
                hasFired = true;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (target == null)
        {
            Debug.LogWarning("No target on button!");
        }
        else
        {
            Debug.Log("Button pressed!");
            if ((!fireOnce || !hasFired))
            {
                Debug.Log("Button activated!");
                target.Activate();
                hasFired = true;
            }
        }
    }
}
