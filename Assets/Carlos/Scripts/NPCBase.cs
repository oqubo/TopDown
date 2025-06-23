using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NPCBase : MonoBehaviour, Interactuable
{

    [SerializeField, TextArea(1, 5)] private string[] frases;
    [SerializeField] private float tiempoEntreLetras;
    [SerializeField] private GameObject cuadroDialogo;
    [SerializeField] private TextMeshProUGUI textoDialogo;
    private bool hablando = false;
    private int indiceActual = -1;


    void Start()
    {

    }

    public void Interactuar()
    {
        Debug.Log("Interacting with NPC: " + gameObject.name);
        
        GameManager.Instance.CambiarEstadoPlayerInteractuando(true);

        cuadroDialogo.SetActive(true);
        if (!hablando)
        {
            SiguienteFrase();
        }
        else
        {
            CompletarFrase();
        }
    }

    private void SiguienteFrase()
    {
        indiceActual++;
        if (indiceActual >= frases.Length)
        {
            TerminarDialogo(); 
        }
        else
        {
            StartCoroutine(EscribirFrase());
        }
    }

    private void TerminarDialogo()
    {
        StopAllCoroutines(); 
        hablando = false;
        cuadroDialogo.SetActive(false);
        textoDialogo.text = string.Empty;
        indiceActual = -1;
        GameManager.Instance.CambiarEstadoPlayerInteractuando(false);
    }

    IEnumerator EscribirFrase()
    {
        hablando = true;
        textoDialogo.text = string.Empty; 

        // Subdividir la frase actual en caracteres
        char[] caracteresFrase = frases[indiceActual].ToCharArray();

        foreach (char caracter in caracteresFrase)
        {
            textoDialogo.text += caracter;
            yield return new WaitForSeconds(tiempoEntreLetras);
        }

        hablando = false;
    }
    
    private void CompletarFrase()
    {
        StopAllCoroutines(); 
        textoDialogo.text = frases[indiceActual];
        hablando = false;
    }

}
