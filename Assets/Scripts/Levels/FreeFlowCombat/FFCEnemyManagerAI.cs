using UnityEngine;

public class FFCEnemyManagerAI
{
    private FFCGameLevelValues _ffcGameLevelValues;

    public FFCEnemyManagerAI(FFCGameLevelValues ffcGameLevelValues)
    {
        _ffcGameLevelValues = ffcGameLevelValues;
    }

    public void Update()
    {
        if (_ffcGameLevelValues is null)
            return;

        switch (_ffcGameLevelValues.Player.CurrentState)
        {
            case FFCPlayerState.Idle:
                HandlePlayerIdle();
                break;
            case FFCPlayerState.StartAttack:
                break;
            case FFCPlayerState.Attacking:
                HandlePlayerAttack();
                break;
            case FFCPlayerState.Dead:
                HandlePlayerDead();
                break;
        }
    }

    public void UpdateChaser(FFCEnemy ffcEnemy)
    {
        if (ffcEnemy.CurrentState == FFCEnemyState.Attack)
            return;

        foreach (FFCEnemy enemy in _ffcGameLevelValues.Enemies)
        {
            if (ffcEnemy != enemy && enemy.CurrentState != FFCEnemyState.Dead)
                enemy.CurrentState = FFCEnemyState.Patrol;
        }

        ffcEnemy.CurrentState = FFCEnemyState.Chase;
    }

    private void HandlePlayerIdle()
    {
        if (_ffcGameLevelValues.ThereIsAtLeastOneEnemyIn(FFCEnemyState.Chase))
            return;

        FFCEnemy chaseEnemy = _ffcGameLevelValues.GetOneEnemyInState(FFCEnemyState.Patrol);
        if (chaseEnemy is null)
            return;

        chaseEnemy.CurrentState = FFCEnemyState.Chase;
    }

    private void HandlePlayerAttack()
    {
        if (!_ffcGameLevelValues.ThereIsAtLeastOneEnemyIn(FFCEnemyState.Chase))
            return;
        
        FFCEnemy chaseEnemy = _ffcGameLevelValues.GetOneEnemyInState(FFCEnemyState.Patrol);
        if (chaseEnemy is null)
            return;

        chaseEnemy.CurrentState = FFCEnemyState.Chase;
    }
    
    private void HandlePlayerDead()
    {
        
    }
}
