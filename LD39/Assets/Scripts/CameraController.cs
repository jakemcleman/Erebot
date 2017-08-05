using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject target;
    public float moveSpeed;
    public float maxRotSpeed = 10;
    public float edgeDistance = 0.3f;
    public float orbitDistance = 10;
    public float wallOffset = 0.5f;

    Camera cam;

    public bool tracking_enabled;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update () {
        if (tracking_enabled && target != null)
        {
            Vector3 rotationChange = new Vector3();
            rotationChange.x = -Input.GetAxis("Mouse Y");
            rotationChange.y = Input.GetAxis("Mouse X");
            //rotationChange *= Time.deltaTime;

            Quaternion desiredRotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationChange);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, maxRotSpeed * Time.deltaTime);

            Ray toCast = new Ray(target.transform.position, -transform.forward);
            RaycastHit hit;

            int layerFlag = ~(1 << 2);

            Vector3 position;

            if(Physics.Raycast(toCast, out hit, orbitDistance, layerFlag))
            {
                position = hit.point + transform.forward * wallOffset;
            }
            else
            {
                position = target.transform.position - (transform.forward * orbitDistance);
            }

            transform.position = position;
        }
    }
}
