using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameLevelValues : GameLevelValues
{
    private float Timer = 3;
    private float TimerSpeed = 1f;
   
    public override void InitializeValues(LevelHandler levelHandler)
    {
        Initialize(LevelType.Menu, levelHandler);

        GameInstance.Singleton.Reset();
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

