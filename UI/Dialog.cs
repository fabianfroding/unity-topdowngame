using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialog : MonoBehaviour
{
    private const float TYPING_SPEED = 0.015f;
    [SerializeField] private TextMeshProUGUI textDisplay; // TODO: Make script find component so you dont have to set the same ref for each npc.
    [SerializeField] private string[] sentences;
    [SerializeField] private GameObject dialogBG; // TODO: Find a way so you dont have to set image for each npc.
    private int sentenceIndex;
    private bool sentenceDone;
    private bool active;

    //========== PUBLIC METHODS ==========//
    public void StartDialog()
    {
        UIManager.instance.HealthUISetActive(false);
        UIManager.instance.ClockUISetActive(false);
        PlayerController.isEnabled = false;

        // TODO: Find a way to prevent enemy combat during dialog.

        active = true;
        sentenceDone = false;
        dialogBG.GetComponent<Image>().enabled = true;
        StartCoroutine(Type());
    }

    public bool isActive()
    {
        return active; ;
    }

    //========== PRIVATE METHODS ==========//
    private void FixedUpdate()
    {
        if (sentenceDone && Input.GetKeyDown(KeyCode.J))
        {
            sentenceDone = false;
            if (sentenceIndex < sentences.Length - 1)
            {
                NextSentence();
            }
            else
            {
                Invoke("ResetDialog", 0.2f);
            }
        }
    }

    private void NextSentence()
    {
        textDisplay.text = "";
        if (sentenceIndex < sentences.Length - 1)
        {
            sentenceIndex++;
            StartCoroutine(Type());
        }
    }

    private void ResetDialog()
    {
        Debug.Log("Exit dialog");
        dialogBG.GetComponent<Image>().enabled = false;
        sentenceIndex = 0;
        textDisplay.text = "";

        UIManager.instance.HealthUISetActive(true);
        UIManager.instance.ClockUISetActive(true);
        PlayerController.isEnabled = true;

        active = false;
    }

    IEnumerator Type()
    {
        for (int i = 0; i < sentences[sentenceIndex].Length; i++)
        {
            textDisplay.text += sentences[sentenceIndex].ToCharArray()[i];
            if (i == sentences[sentenceIndex].Length - 1)
            {
                sentenceDone = true;
            }
            yield return new WaitForSeconds(TYPING_SPEED);
        }
    }

}
