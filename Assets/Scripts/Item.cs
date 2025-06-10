using UnityEngine;

public class Item : MonoBehaviour, Interactuable
{
 
    [SerializeField] private ItemSO itemData;
    
    public void Interactuar()
    {
        GameManager.Instance.Inventario.NuevoItem(itemData);
        Destroy(this.gameObject); 
    }
}
