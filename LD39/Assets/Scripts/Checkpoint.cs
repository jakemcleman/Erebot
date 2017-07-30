using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : Activatable
{
    public override void Activate(GameObject player)
    {
        Debug.Log("Checkpoint Reached");

        player.GetComponent<Player>().spawnPoint = player.transform.position;
    }
}
