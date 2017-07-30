using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public float moveSpeed = 3;
    public float acceleration = 2;
    public float maxTurnDeg = 360;

    public float moveDrainRate = 0.2f;

    public float powerLevel = 10;

    public float curDrainRate;

    public GameObject deadBotPrefab;

    public GameObject fadeObject;

    public ParticleSystem[] dustSystems;

    Vector3 movement;
    Vector3 trueMovement;
    CharacterController controller;

    GameObject treads;
    GameObject solarPanel;
    GameObject head;

    public GameObject sun;
    public float rechargeRateMultiplier = 0.2f;

    public Vector3 spawnPoint;

    bool respawning;

	void Start () {
        movement = new Vector3();
        trueMovement = new Vector3();

        controller = GetComponent<CharacterController>();
        treads = transform.Find("Treads").gameObject;
        head = transform.Find("Head").gameObject;
        solarPanel = head.transform.Find("SolarPanel").gameObject;
        

        spawnPoint = transform.position;

        respawning = false;

        //Fade in
        fadeObject.GetComponent<Image>().CrossFadeAlpha(0, 2, false);
    }

    void Update()
    {
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

    IEnumerator Respawn()
    {
        GameObject cam = Camera.main.gameObject;

        //Stop camera tracking
        cam.GetComponent<CameraController>().enabled = false;

        //Turn of particle systems
        foreach (ParticleSystem sys in dustSystems)
        {
            ParticleSystem.EmissionModule emission = sys.emission;
            emission.enabled = false;
            emission.rateOverDistance = 0;
            emission.rateOverDistanceMultiplier = 0;
        }

        //Fade out
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

        //Turn off particle systems
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
