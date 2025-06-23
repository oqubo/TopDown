using UnityEngine;

public class GameInstance : MonoBehaviour
{
    public static GameInstance Singleton;
    
    public int currentScore = 0;
    [SerializeField] 
    public float currentLifePoints = 10;
    [SerializeField] 
    public float maxLifePoints = 10;
    
    public int currentCounter = 0;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPoints(int points)
    {
        currentScore += points;
    }
    
    public float GetLifePercentage()
    {
        return currentLifePoints / maxLifePoints;
    }

    public void Reset()
    {
        currentScore = 0;
        currentLifePoints = maxLifePoints;
    }
}