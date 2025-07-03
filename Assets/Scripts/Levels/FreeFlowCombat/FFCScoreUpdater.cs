using UnityEngine;
using TMPro;

public class FFCScoreUpdater : MonoBehaviour
{
    private TextMeshProUGUI _scoreUIText;
   
    void Start()
    {
        _scoreUIText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _scoreUIText.text = "Puntos: " + GameInstance.Singleton.currentScore;
    }
}
