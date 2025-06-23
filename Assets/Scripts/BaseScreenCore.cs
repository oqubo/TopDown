using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseScreenCore : MonoBehaviour
{
    [SerializeField]
    string SceneNameToLoad;
    
    private float _timeToWaitForNextScreen = 3;
    private float _currentTime = 0f;
    
    protected void Start()
    {
        
    }
    
    protected void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime < _timeToWaitForNextScreen)
        {
            return;
        }
        
        _currentTime = 0f;
        
        SceneManager.LoadScene(SceneNameToLoad);
    }
}

