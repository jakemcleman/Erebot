using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : MonoBehaviour {
    public float rotationSpeed = 20;
    public Transform blades;

	// Update is called once per frame
	void Update () {
        blades.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
	}
}
