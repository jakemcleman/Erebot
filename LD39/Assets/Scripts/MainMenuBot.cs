using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBot : MonoBehaviour {
    float rollRightRot = -120;
    float rollLeftRot = 60;

    float rotSpeed = 60;
    float rollSpeed = 1;

    public string playLevel = "Level1";
    public GameObject fadeObject;

    void Start () {
        //Fade in
        fadeObject.GetComponent<Image>().CrossFadeAlpha(0, 1, false);
    }

    public void StartGame()
    {
        StartCoroutine("RollOffRight");
    }

    public void QuitGame()
    {
        StartCoroutine("RollOffLeft");
    }


    IEnumerator RollOffLeft()
    {
        Quaternion targetRot = Quaternion.Euler(0, rollLeftRot, 0);

        while (Quaternion.Angle(targetRot, transform.rotation) > 1)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
            yield return null;
        }

        float timer = 3.0f;
        fadeObject.GetComponent<Image>().CrossFadeAlpha(1, timer, false);

        //CharacterController controller = GetComponent<CharacterController>();

        while (timer > 0)
        {
            Vector3 motion = transform.forward;
            motion *= -rollSpeed;
            motion *= Time.fixedDeltaTime;

            transform.position += motion;
            timer -= Time.deltaTime;
            yield return null;
        }

        Application.Quit();
    }

    IEnumerator RollOffRight()
    {
        Quaternion targetRot = Quaternion.Euler(0, rollRightRot, 0);

        while(Quaternion.Angle(targetRot, transform.rotation) > 1)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
            yield return null;
        }

        float timer = 3.0f;
        fadeObject.GetComponent<Image>().CrossFadeAlpha(1, timer, false);

        //CharacterController controller = GetComponent<CharacterController>();

        while (timer > 0)
        {
            Vector3 motion = transform.forward;
            motion *= -rollSpeed;
            motion *= Time.fixedDeltaTime;

            transform.position += motion;
            timer -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(playLevel);
    }
}
