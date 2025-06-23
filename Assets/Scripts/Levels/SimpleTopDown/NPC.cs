using System.Collections;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] protected GameManagerSo gameManager;
    [SerializeField, TextArea(1,5)] protected string[] phrases;
    [SerializeField] protected float timeBetweenLetters = 0.2f;
    [SerializeField] protected GameObject DialogSystem;
    [SerializeField] protected TextMeshProUGUI textMesh;
    private bool speaking = false;
    private int currentPhraseIndex = -1;

    protected void Start()
    {
        
    }
    
    public void Interact()
    {
        //gameManager.ChangePlayerState(false);
        DialogSystem.SetActive(true);
        if (!speaking)
            NextPhrase();
        else
            completeCurrentPhrase();
    }

    private void completeCurrentPhrase()
    {
        StopAllCoroutines();
        textMesh.text = phrases[currentPhraseIndex];
        speaking = false;
    }

    IEnumerator WritePhrase()
    {
        speaking = true;
        textMesh.text = "";
        
        char[] charactersInPhrase = phrases[currentPhraseIndex].ToCharArray();
        foreach (char currentChar in charactersInPhrase)
        {
            textMesh.text += currentChar;
            yield return new WaitForSeconds(timeBetweenLetters);
        }
        speaking = false;
    }

    private void NextPhrase()
    {
        currentPhraseIndex++;
        if (currentPhraseIndex >= phrases.Length)
        {
            EndDialog();
        }
        else
        {
            StartCoroutine(WritePhrase());
        }
            
    }

    private void EndDialog()
    {
        speaking = false;
        textMesh.text = "";
        currentPhraseIndex = -1;
        DialogSystem.SetActive(false);
        //gameManager.ChangePlayerState(true);
    }
}
