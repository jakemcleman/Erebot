using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDisplay : MonoBehaviour {
    Text displayText;
    Image panel;

    bool isShowing;

	// Use this for initialization
	void Awake () {
        displayText = transform.GetComponentInChildren<Text>();
        panel = GetComponent<Image>();

        panel.CrossFadeAlpha(0, 0, false);
        displayText.CrossFadeAlpha(0, 0, false);
    }

    private struct DialogMessage
    {
        public string text;
        public float time;

        public DialogMessage(string text, float time)
        {
            this.text = text;
            this.time = time;
        }
    }

    public void MessageChain(string[] messages, float[] times)
    {
        DialogMessage[] messageArray = new DialogMessage[messages.Length];
        for (int i = 0; i < messages.Length; ++i)
        {
            messageArray[i] = new DialogMessage(messages[i], times[i]);
        }

        StartCoroutine("MessageSequenceCoroutine", messageArray);
    }

    public void ShowText(string text, float time)
    {
        if(isShowing)
        {
            StopAllCoroutines();
        }

        StartCoroutine("ShowTextCoroutine", new DialogMessage(text, time));
    }

    IEnumerator MessageSequenceCoroutine(DialogMessage[] data)
    {
        foreach(DialogMessage message in data)
        {
            yield return ShowTextCoroutine(message);
            yield return new WaitForSeconds(message.time + 0.1f);
        }
    }
	
	IEnumerator ShowTextCoroutine(DialogMessage data)
    {
        //If text is already showing
        if(isShowing)
        {
            //Fade out
            panel.CrossFadeAlpha(0, 0.25f, false);
            displayText.CrossFadeAlpha(0, 0.25f, false);
        }

        //Change text
        displayText.text = data.text;

        //Fade in
        isShowing = true;
        panel.CrossFadeAlpha(1, 0.25f, false);
        displayText.CrossFadeAlpha(1, 0.25f, false);

        //Wait for the specified time
        yield return new WaitForSeconds(0.25f + data.time);

        //Fade out
        panel.CrossFadeAlpha(0, 0.25f, false);
        displayText.CrossFadeAlpha(0, 0.25f, false);
        isShowing = false;
        yield return null;
    }
}
