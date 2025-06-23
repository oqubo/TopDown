using System;
using UnityEngine;

public class CambioEscena : MonoBehaviour, Interactuable
{
    [SerializeField] string nombreEscena;
    public string NombreEscena { get => nombreEscena; set => nombreEscena = value; }



    public void Interactuar()
    {
        GameManager.Instance.CargarEscenaPorNombre(nombreEscena);
    }
}
