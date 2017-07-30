using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
    public Activatable[] targets;
    public bool fireOnce = false;

    bool hasFired;

    void OnTriggerEnter(Collider other)
    {
        foreach (Activatable target in targets)
        {
            if (target == null)
            {
                Debug.LogWarning("No target on button!");
            }
            else
            {
                Debug.Log("Button pressed!");
                if ((!fireOnce || !hasFired) && other.transform.parent != null && other.transform.parent.gameObject.GetComponent<Player>() != null)
                {
                    Debug.Log("Button activated!");
                    target.Activate(other.gameObject);
                    hasFired = true;
                }
            }
        }
    }
}
