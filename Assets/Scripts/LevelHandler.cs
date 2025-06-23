using System;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    [SerializeField] 
    public string nextScene = "MainMenu";
    
    [SerializeField]
    public LevelType levelType = LevelType.Explore;
    
    [SerializeField]
    private GameManagerSo _gameManagerSo;
    
    private GameLevelValues _currentLevelValues;
    public GameLevelValues CurrentLevelValues {set => _currentLevelValues = value;}
    
    private void Update()
    {
        if (_currentLevelValues is null)
            return;
        
        _currentLevelValues.Update();
    }
}
