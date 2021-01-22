using System.Collections;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    private const float TYPING_SPEED = 0.03f;
    [SerializeField] private TextMeshProUGUI textDisplay; // TODO: Make dynamic so that each NPC doesn't need it's own textDisplay object.
    [SerializeField] private string[] sentences;
    private int sentenceIndex;
    private bool sentenceDone;
    private bool active;

    //========== PUBLIC METHODS ==========//
    public void StartDialog()
    {
        // TODO: Disable other UI and disable player controller (make player controller static).
        //UIManager.HealthUISetActive(false);
        // TODO: Disable enemies while in dialog.
        active = true;
        sentenceDone = false;
        StartCoroutine(Type());
    }

    public bool isActive()
    {
        return active;;
    }

    //========== PRIVATE METHODS ==========//
    private void FixedUpdate()
    {
        if (sentenceDone && Input.GetKey(KeyCode.E))
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
        sentenceIndex = 0;
        textDisplay.text = "";
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
