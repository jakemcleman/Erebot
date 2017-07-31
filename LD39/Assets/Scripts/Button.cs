using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
    public Activatable[] targets;
    public bool fireOnce = false;

    bool hasFired;



    void OnTriggerEnter(Collider other)
    {
        if ((!fireOnce || !hasFired) && other.transform.parent != null && other.transform.parent.gameObject.GetComponent<Player>() != null)
        {
            AudioSource audio = GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.Play();
            }

            foreach (Activatable target in targets)
            {
                if (target == null)
                {
                    Debug.LogWarning("No target on button!");
                }
                else
                {
                    Debug.Log("Button activated!");
                    target.Activate(other.transform.parent.gameObject);
                    hasFired = true;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        foreach (Activatable target in targets)
        {
            if (target != null)
            {
                Gizmos.DrawLine(transform.position, target.transform.position);
            }
        }
    }
}
