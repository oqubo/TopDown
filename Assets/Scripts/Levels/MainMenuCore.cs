using UnityEngine;

public class MainMenuCore : BaseScreenCore
{
    private new void Start()
    {
        base.Start();
    }

    private new void Update()
    {
        base.Update();
        
        GameInstance.Singleton.currentScore = 0;
        GameInstance.Singleton.currentLifePoints = GameInstance.Singleton.maxLifePoints;
    }
}
