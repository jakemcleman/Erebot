using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public float moveSpeed = 3;
    public float acceleration = 2;
    public float maxTurnDeg = 360;

    public float moveDrainRate = 0.2f;

    public float powerLevel = 10;

    public float curDrainRate;

    public float maxTreadAudioVol = 0.5f;

    public GameObject deadBotPrefab;

    public GameObject fadeObject;

    public ParticleSystem[] dustSystems;

    Vector3 movement;
    Vector3 trueMovement;
    CharacterController controller;

    GameObject treads;
    GameObject solarPanel;
    GameObject head;

    DialogueDisplay dialogue;

    public GameObject sun;
    public float rechargeRateMultiplier = 0.2f;

    AudioSource trackAudio;
    AudioSource powerAudio;

    public Vector3 spawnPoint;

    bool respawning;

    public bool paused;
    public GameObject pauseMenu;

    public void StartLevel(int index)
    {
        switch(index)
        {
            case 1:
                string[] introText =
                {
                "POWER SYSTEM FAILURE",
                "RUNNING SYSTEM DIAGNOSTICS...",
                "SYSTEM DIAGNOSTICS REPORT:\nCAPACITOR BANK FAILURE",
                "EMERGENCY SHUT DOWN IMMINENT",
                "EMERGENCY SHUT DOWN CANCELLED",
                "POWER SOURCE DETECTED...",
                "SOLAR CELLS ARE OPERATIONAL",
                "DIRECTIVES:\nREMAIN IN DIRECT SUNLIGHT\nSEEK REPAIR"
                };

                float[] introTextTimes =
                {
                     3,
                     1.5f,
                     3,
                     2,
                     1,
                     3,
                     4,
                     6
                };

                dialogue.MessageChain(introText, introTextTimes);
                break;
            default:
                dialogue.ShowText("DEFAULT TEXT", 5);
                break;
        }
        
    }

	void Start () {
        SetPaused(false);

        movement = new Vector3();
        trueMovement = new Vector3();

        //DontDestroyOnLoad(gameObject);

        controller = GetComponent<CharacterController>();
        treads = transform.Find("Treads").gameObject;
        head = transform.Find("Head").gameObject;
        solarPanel = head.transform.Find("SolarPanel").gameObject;

        trackAudio = GetComponent<AudioSource>();
        powerAudio = head.GetComponent<AudioSource>();

        spawnPoint = transform.position;

        respawning = false;

        //Fade in
        fadeObject.GetComponent<Image>().CrossFadeAlpha(0, 2, false);

        dialogue = FindObjectOfType<DialogueDisplay>();

        if (SceneManager.GetActiveScene().name == "Level1")
        {
            StartLevel(1);
        }


        //Turn on particle systems
        foreach (ParticleSystem sys in dustSystems)
        {
            ParticleSystem.EmissionModule emission = sys.emission;
            emission.enabled = true;
            emission.rateOverDistance = 10;
            emission.rateOverDistanceMultiplier = 1;
        }
    }

    public void SetPaused(bool paused)
    {
        pauseMenu.SetActive(paused);
        this.paused = paused;

        Cursor.visible = paused;
    }

    public void Pause()
    {
        SetPaused(true);
    }

    public void Unpause()
    {
        SetPaused(false);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F12))
        {
            string timestamp = System.DateTime.Now.Month + "-" + System.DateTime.Now.Day + "." + System.DateTime.Now.Hour + "-" + System.DateTime.Now.Minute + "-" + System.DateTime.Now.Second;
            ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "\\screenshot_" + timestamp + ".png");
        }

        if (paused)
        {
            if(Input.GetButtonDown("Menu"))
            {
                SetPaused(false);
            }
        }
        else
        {
            if (Input.GetButtonDown("Menu"))
            {
                SetPaused(true);
            }

            curDrainRate = 1.0f + (movement.magnitude * moveDrainRate);

            if (transform.position.y < -50)
            {
                transform.position = spawnPoint;
                movement = Vector3.zero;
                trueMovement = Vector3.zero;
            }

            //Scale down the input every frame
            movement *= 0.95f;

            //Read the input from keyboard
            Vector3 input = new Vector3(Input.GetAxis("Horizontal") * acceleration, 0, Input.GetAxis("Vertical") * acceleration);

            //Calculate the new movement vector
            movement += Quaternion.Euler(0, -45, 0) * input;

            if (movement.magnitude > moveSpeed)
            {
                movement = movement.normalized * moveSpeed;
            }

            float moveSpeedPercent = (movement.magnitude / moveSpeed);
            trackAudio.volume = moveSpeedPercent * moveSpeedPercent * maxTreadAudioVol;

            if (powerLevel > 0)
            {
                if (movement.sqrMagnitude > 0.2f)
                {
                    head.transform.rotation = Quaternion.RotateTowards(head.transform.rotation, Quaternion.LookRotation(movement), maxTurnDeg * Time.deltaTime);
                    treads.transform.rotation = Quaternion.LookRotation(movement);
                }

                //Debug.DrawRay(solarPanel.transform.position, -sun.transform.forward);
                Ray ray = new Ray(solarPanel.transform.position, -sun.transform.forward);
                if (Physics.Raycast(ray, 1000, -1, QueryTriggerInteraction.Ignore))
                {
                    powerLevel -= curDrainRate * Time.deltaTime;
                }
                else if (powerLevel < 1)
                {
                    powerLevel += Time.deltaTime * rechargeRateMultiplier;

                    if (powerLevel > 1)
                        powerLevel = 1;
                }
            }
            else
            {
                movement = Vector3.zero;

                if (!respawning)
                {
                    StartCoroutine("Respawn");
                    respawning = true;
                }
            }

            if (controller.isGrounded)
            {
                trueMovement = movement;
            }
            else
            {
                trueMovement.y -= 9.81f * Time.deltaTime;
            }

            controller.Move(trueMovement * Time.deltaTime);
        }
    }

    IEnumerator Respawn()
    {
        GameObject cam = Camera.main.gameObject;

        //Stop camera tracking
        cam.GetComponent<CameraController>().enabled = false;

        //Turn off particle systems
        foreach (ParticleSystem sys in dustSystems)
        {
            ParticleSystem.EmissionModule emission = sys.emission;
            emission.enabled = false;
            emission.rateOverDistance = 0;
            emission.rateOverDistanceMultiplier = 0;
        }

        //Fade out
        powerAudio.Play();
        fadeObject.GetComponent<Image>().CrossFadeAlpha(1, 2, false);
        yield return new WaitForSeconds(2);

        //Respawn
        Instantiate(deadBotPrefab, transform.position, transform.rotation);
        transform.position = spawnPoint;
        movement = Vector3.zero;
        trueMovement = Vector3.zero;
        powerLevel = 1;

        //Reposition Camera
        cam.transform.SetPositionAndRotation(spawnPoint + new Vector3(20, 15, -26), cam.transform.rotation);
        cam.GetComponent<CameraController>().enabled = true;

        //Fade in
        fadeObject.GetComponent<Image>().CrossFadeAlpha(0, 2, false);
        yield return new WaitForSeconds(2);

        respawning = false;

        //Turn on particle systems
        foreach (ParticleSystem sys in dustSystems)
        {
            ParticleSystem.EmissionModule emission = sys.emission;
            emission.enabled = true;
            emission.rateOverDistance = 10;
            emission.rateOverDistanceMultiplier = 1;
        }
        //Resume camera tracking
        

    }
}
