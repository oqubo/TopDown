using System.Collections.Generic;
using UnityEngine;

public class GameCore : MonoBehaviour
{
    [SerializeField]
    public GameObject levelHandler;
    private LevelHandler _levelHandlerRef;
    private List<FFCEnemy> _enemies = new List<FFCEnemy>();
    [SerializeField] private float enemyAcceptableDistance = 2;
    
    private void Start()
    {
        _levelHandlerRef = levelHandler.GetComponent<LevelHandler>();
    }

    void Update()
    {
        
    }

    public void RegisterEnemy(GameObject newEnemy)
    {
        FFCEnemy enemy = newEnemy.GetComponent<FFCEnemy>();
        _enemies.Add(enemy);
    }

    public GameObject ClosestEnemy(Vector2 mousePosition)
    {
        foreach (FFCEnemy enemy in _enemies)
            enemy.IsActiveTarget = false;
        
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (FFCEnemy enemy in _enemies)
        {
            float currentDistanceToEnemy = Vector2.Distance(enemy.gameObject.transform.position, mousePosition);
            if (currentDistanceToEnemy <= closestDistance)
            {
                closestDistance = currentDistanceToEnemy;
                closestEnemy = enemy.gameObject;
            }
        }

        return closestDistance > enemyAcceptableDistance ? null : closestEnemy;
    }
}
