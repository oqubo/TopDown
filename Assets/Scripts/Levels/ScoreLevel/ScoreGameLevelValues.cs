using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreGameLevelValues:  GameLevelValues
{
    private float Timer = 3;
    private float TimerSpeed = 1f;
    
    public override void InitializeValues(LevelHandler levelHandler)
    {
        Initialize(LevelType.Score, levelHandler);
    }

    public override void Update()
    {
        Timer -= Time.deltaTime * TimerSpeed;
        if (Timer < 0)
        {
            SceneManager.LoadScene(_levelHandler.nextScene);
        }
    }
}