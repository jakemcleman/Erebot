using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour {

    public string nextLevelName = "DEFAULT VALUE, CHANGE THIS";

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent != null && other.transform.parent.GetComponent<Player>() != null)
        {
            SceneManager.LoadScene(nextLevelName);
        }
    }
}
