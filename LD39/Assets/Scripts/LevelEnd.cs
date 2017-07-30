using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour {

    public string nextLevelName = "DEFAULT VALUE, CHANGE THIS";
    public int nextLevelNumber = 2;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent != null && other.transform.parent.GetComponent<Player>() != null)
        {
            SceneManager.LoadScene(nextLevelName);
            other.transform.parent.transform.position = new Vector3(0, 2, 0);
            Player player = other.transform.parent.GetComponent<Player>();
            player.StartLevel(nextLevelNumber);
        }
    }
}
