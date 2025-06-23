using UnityEngine;
using UnityEngine.UI;

public class SistemaInventario : MonoBehaviour
{
    [SerializeField] private GameObject marcoInventario;
    [SerializeField] private Button[] botones;
    private int itemsDisponibles = 0;


    void Start()
    {
        for (int i = 0; i < botones.Length; i++)
        {
            // Captura el índice actual para evitar problemas de cierre
            int index = i;
            botones[i].onClick.AddListener(() => BotonClickado(index));
        }
    }

    private void BotonClickado(int index)
    {
        Debug.Log($"Botón {index} clickeado");

    }

    public void NuevoItem(ItemSO item)
    {
        if (itemsDisponibles >= botones.Length)
        {
            Debug.LogWarning("No hay espacio en el inventario para más items.");
            return;
        }

        botones[itemsDisponibles].gameObject.SetActive(true);
        botones[itemsDisponibles].GetComponent<Image>().sprite = item.icono;
        itemsDisponibles++;
         
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (marcoInventario.activeSelf)
            {
                marcoInventario.SetActive(false);
            }
            else
            {
                marcoInventario.SetActive(true);
            }
        }
        
    }
}
