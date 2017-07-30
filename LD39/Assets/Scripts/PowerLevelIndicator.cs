using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerLevelIndicator : MonoBehaviour {
    Player player;
    public int numBars = 10;
    Text text;

    int lastBars;

    public AudioClip barUpSound;
    public AudioClip barDownSound;
    AudioSource audioSource;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindObjectOfType<Player>();
        text = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();

        lastBars = numBars;
    }
	
	// Update is called once per frame
	void Update ()
    {
        int powerLevel = (int)(player.powerLevel * numBars);

        string displayText = "";

        for(int i = 0; i < powerLevel; ++i)
        {
            displayText += "|";
        }

        if(player.powerLevel < 0.6f)
        {
            text.color = Color.red;
        }
        else
        {
            text.color = Color.white;
        }
        
        if(powerLevel == 0)
        {
            displayText = "- - - -";
        }

        if(powerLevel > lastBars)
        {
            audioSource.PlayOneShot(barUpSound);
        }
        else if (powerLevel < lastBars)
        {
            audioSource.PlayOneShot(barDownSound);
        }

        lastBars = powerLevel;

        text.text = displayText;
	}
}
