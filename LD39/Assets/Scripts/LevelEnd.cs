using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour {

    public string nextLevelName = "DEFAULT VALUE, CHANGE THIS";
    public int nextLevelNumber = 2;

    Player player;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent != null && other.transform.parent.GetComponent<Player>() != null)
        {
            //SceneManager.LoadScene(nextLevelName);
            player = other.transform.parent.GetComponent<Player>();
            //other.transform.parent.transform.position = new Vector3(0, 2, 0);
            //Player player = other.transform.parent.GetComponent<Player>();
            //player.StartLevel(nextLevelNumber);
            StartCoroutine("NextLevel");
        }
    }

    IEnumerator NextLevel()
    {
        Image toFade = player.fadeObject.GetComponent<Image>();
        if (toFade != null)
        {
            toFade.CrossFadeAlpha(1, 2, false);
            yield return new WaitForSeconds(2);
        }
        SceneManager.LoadScene(nextLevelName);
        yield return null;
    }
}
