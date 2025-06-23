using UnityEngine;

public abstract class GameLevelValues
{
    protected LevelHandler _levelHandler;
    protected LevelType levelType;

    protected void Initialize(LevelType _levelType, LevelHandler levelHandler)
    {
        levelType = _levelType;
        _levelHandler = levelHandler;
    }

    public abstract void InitializeValues(LevelHandler levelHandler);
    public abstract void Update();
}
