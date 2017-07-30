using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerConnection : MonoBehaviour {
    public Transform connectTo;
    public Vector3 connectFrom;
    public GameObject cablePrefab;
    public float pointSpacing = 0.5f;

    GameObject cable;
    LineRenderer line;

    private Vector3 a, b, c;

	// Use this for initialization
	void Start () {
        if (connectTo == null || cablePrefab == null) return;

        cable = Instantiate(cablePrefab);
        line = cable.GetComponent<LineRenderer>();


        a = transform.position + connectFrom;
        b = connectTo.position;

        if(connectTo.gameObject.GetComponent<PowerConnection>() != null)
        {
            b += connectTo.gameObject.GetComponent<PowerConnection>().connectFrom;
        }

        Vector3 between = b - a;
        float length = between.magnitude;
        Vector3 perp = Vector3.Cross(a, b).normalized;
        if (perp.z > 0) perp = -perp;

        //Midpoint
        c = (a + b) / 2 + (Vector3.down * length * 0.2f);

        int steps = (int)(length / pointSpacing) + 1;

        Vector3[] positions = new Vector3[steps];

        for(int i = 0; i < steps; ++i)
        {
            positions[i] = BezPos((1.0f * i) / (1.0f * steps - 1));
        }

        line.positionCount = steps;
        line.SetPositions(positions);
    }

    private void OnDrawGizmos()
    {
        if (connectTo == null) return;

        a = transform.position + connectFrom;
        b = connectTo.position;

        if (connectTo.gameObject.GetComponent<PowerConnection>() != null)
        {
            b += connectTo.gameObject.GetComponent<PowerConnection>().connectFrom;
        }

        Gizmos.color = Color.grey;
        Gizmos.DrawLine(a, b);
    }

    private Vector3 BezPos(float t)
    {
        float OneMinusT = 1.0f - t;
        float b0 = OneMinusT * OneMinusT;
        float b1 = 2.0f * OneMinusT * t;
        float b2 = t * t;
        return b0 * a + b1 * c + b2 * b;
    }
}
