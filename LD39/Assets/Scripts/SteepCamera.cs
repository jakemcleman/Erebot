using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteepCamera : MonoBehaviour
{
    public float rotationSpeed = 10;
    public float angle = 80;

    bool rotateUp;
    bool rotateDown;

    private void Start()
    {
        rotateUp = false;
        rotateDown = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null && other.transform.parent.GetComponent<Player>() != null)
        {
            rotateUp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent != null && other.transform.parent.GetComponent<Player>() != null)
        {
            rotateDown = true;
        }
    }

    private void Update()
    {
        if (rotateUp)
        {
            Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, Quaternion.Euler(angle, -45, 0), rotationSpeed * Time.deltaTime);
            if (Quaternion.Angle(Camera.main.transform.rotation, Quaternion.Euler(angle, -45, 0)) < 0.1f)
            {
                rotateUp = false;
            }
        }
        else if (rotateDown)
        {
            Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, Quaternion.Euler(30, -45, 0), rotationSpeed * Time.deltaTime);
            if (Quaternion.Angle(Camera.main.transform.rotation, Quaternion.Euler(30, -45, 0)) < 0.1f)
            {
                rotateDown = false;
            }
        }
    }
}
