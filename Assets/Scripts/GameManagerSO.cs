using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Scriptable ObjectGameManager")]
public class GameManagerSo : ScriptableObject
{
    private LevelHandler _levelHandler;
    public LevelHandler LevelHandler { get { return _levelHandler; } }
    private GameLevelValues _currentLevelValues;
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += NewSceneLoaded;
    }

    private void NewSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _levelHandler = FindFirstObjectByType<LevelHandler>();
        if (_levelHandler == null)
        {
            Debug.Log("LevelHandler not found");
            //Application.Quit();
            return;
        }

        InitializeCurrentLevelGameValues();
    }

    private void InitializeCurrentLevelGameValues()
    {
        switch (_levelHandler.levelType)
        {
            case LevelType.Combat:
                _currentLevelValues = new FFCGameLevelValues();
                break;
            case LevelType.Explore:
                _currentLevelValues = new ExploreGameLevelValues();
                break;
            case LevelType.Menu:
                _currentLevelValues = new MenuGameLevelValues();
                break;
            case LevelType.Score:
                _currentLevelValues = new ScoreGameLevelValues();
                break;
        }
        _currentLevelValues.InitializeValues(_levelHandler);
        _levelHandler.CurrentLevelValues = _currentLevelValues;
    }

    public FFCGameLevelValues GetFFCCurrentLevelValues()
    {
        return _currentLevelValues as FFCGameLevelValues;
    }

    public void PlayerDies()
    {
        SceneManager.LoadScene(_levelHandler.nextScene);
    }
}
