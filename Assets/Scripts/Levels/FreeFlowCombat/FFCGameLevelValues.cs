using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class FFCGameLevelValues : GameLevelValues
{
    private List<FFCEnemy> _enemies;
    public List<FFCEnemy> Enemies { get { return _enemies; } }
    
    private FFCPlayer _player;
    public FFCPlayer Player { get { return _player; } }

    private FFCEnemyManagerAI _enemyManagerAI;
    
    private float counterUpdateSpeed = 1;
    private float counterTimer = 0;
    private float counterTimerMax = 4;
    
    public override void InitializeValues(LevelHandler levelHandler)
    {
        Initialize(LevelType.Combat, levelHandler);

        _enemyManagerAI = new FFCEnemyManagerAI(this); 
        
        // Player
        _player = Object.FindFirstObjectByType<FFCPlayer>();
        if (_player == null)
        {
            Debug.LogError("Player not found");
            Application.Quit();
            return;
        }
        
        // Enemies
        _enemies = new List<FFCEnemy>();
        FFCEnemy[] enemies = Object.FindObjectsByType<FFCEnemy>(FindObjectsSortMode.None);
        foreach (FFCEnemy enemy in enemies)
        {
            _enemies.Add(enemy);
        }
    }
    
    public FFCEnemy ClosestEnemy(Vector2 mousePosition, float enemyAcceptableDistance)
    {
        foreach (FFCEnemy enemy in _enemies)
            enemy.IsActiveTarget = false;
        
        FFCEnemy closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (FFCEnemy enemy in _enemies)
        {
            float currentDistanceToEnemy = Vector2.Distance(enemy.gameObject.transform.position, mousePosition);
            if (currentDistanceToEnemy <= closestDistance)
            {
                closestDistance = currentDistanceToEnemy;
                closestEnemy = enemy;
            }
        }

        return closestDistance > enemyAcceptableDistance ? null : closestEnemy;
    }

    public override void Update()
    {
        UpdateCounter();
        _enemyManagerAI.Update();

        if (AreAllEnemiesDead())
            SceneManager.LoadScene("ExploreTest");
    }

    private void UpdateCounter()
    {
        counterTimer -= Time.deltaTime * counterUpdateSpeed;

        if (counterTimer <= 0)
        {
            GameInstance.Singleton.AddPoints(GameInstance.Singleton.currentCounter);
            GameInstance.Singleton.currentCounter = 0;
        }
    }

    public void DeleteEnemy(FFCEnemy enemy)
    {
        Enemies.Remove(enemy);
    }

    public void SetEnemyAsAttacker(FFCEnemy ffcEnemy)
    {
        GameInstance.Singleton.currentCounter++;
        counterTimer = counterTimerMax;
        _enemyManagerAI.UpdateChaser(ffcEnemy);
    }

    public bool ThereIsAtLeastOneEnemyIn(FFCEnemyState state)
    {
        foreach (FFCEnemy enemy in _enemies)
        {
            if (enemy.CurrentState == state)
                return true;
        }
        
        return false;
    }
    
    public FFCEnemy GetOneEnemyInState(FFCEnemyState state)
    {
        foreach (FFCEnemy enemy in _enemies)
        {
            if (enemy.CurrentState == state)
                return enemy;
        }
        
        return null;
    }
    
    public FFCEnemy GetChaseEnemy(Vector2 mousePosition, float enemyAcceptableDistance)
    {
        FFCEnemy chaseEnemy = null;
        foreach (FFCEnemy enemy in _enemies)
            if (enemy.CurrentState == FFCEnemyState.Chase)
            {
                chaseEnemy = enemy;
                break;
            }

        if (chaseEnemy is null)
            return null;
        
        float currentDistanceToEnemy = Vector2.Distance(chaseEnemy.gameObject.transform.position, mousePosition);
        
        return currentDistanceToEnemy > enemyAcceptableDistance ? null : chaseEnemy;
    }
    
    private bool AreAllEnemiesDead()
    {
        foreach (FFCEnemy enemy in _enemies)
            if (enemy.CurrentState != FFCEnemyState.Dead)
                return false;
        
        return true;
    }
}
