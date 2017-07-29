using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject target;
    public float moveSpeed;
    public float edgeDistance = 0.3f;

    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update () {
        Vector3 targetScreenPos = cam.WorldToViewportPoint(target.transform.position);
        Vector3 motion = new Vector3();

        if(targetScreenPos.x < edgeDistance)
        {
            motion += Vector3.left * (edgeDistance - targetScreenPos.x) * (1 / edgeDistance);
        }
        else if (targetScreenPos.x > 1 - edgeDistance)
        {
            motion += Vector3.right * (targetScreenPos.x - (1 - edgeDistance)) * (1 / edgeDistance);
        }

        if (targetScreenPos.y < edgeDistance)
        {
            motion += Vector3.back * (edgeDistance - targetScreenPos.y) * (1 / edgeDistance);
        }
        else if (targetScreenPos.y > 1 - edgeDistance)
        {
            motion += Vector3.forward * (targetScreenPos.y - (1 - edgeDistance)) * (1 / edgeDistance);
        }

        motion = Quaternion.Euler(0, -45, 0) * motion * moveSpeed * Time.deltaTime;
        transform.position += motion;

        
    }
}
