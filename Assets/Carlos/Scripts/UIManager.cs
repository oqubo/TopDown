using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerController pc;
    [SerializeField] private Slider uiVida;
    [SerializeField] private TextMeshProUGUI uiPuntos;

    private void OnEnable()
    {
        pc.eventoQuitarVida += ActualizarVida;
        pc.eventoAumentarPuntos += ActualizarPuntos;
        
    }

    private void OnDisable()
    {
        pc.eventoQuitarVida -= ActualizarVida;
        pc.eventoAumentarPuntos -= ActualizarPuntos;
    }

    private void ActualizarVida()
    {
        uiVida.value = pc.Vida / 100f;
    }

    private void ActualizarPuntos()
    {
        uiPuntos.text = "Puntos: " + pc.Puntos.ToString();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
