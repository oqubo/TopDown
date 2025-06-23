using UnityEngine;
using TMPro;

public class FFCCounterUpdater : MonoBehaviour
{
    private TextMeshProUGUI _counterUIText;
   
    void Start()
    {
        _counterUIText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        int counter = GameInstance.Singleton.currentCounter;
        _counterUIText.text = counter <= 0 ? "" : "x" + GameInstance.Singleton.currentCounter;
    }
}
