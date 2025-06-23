using System;
using UnityEngine;

public class FFCEnemy : MonoBehaviour
{
    [SerializeField] protected GameManagerSo gameManager;
    
    [SerializeField] private GameObject _markedTarget;
    private bool _isActiveTarget = false;
    public bool IsActiveTarget { get { return _isActiveTarget; } set { _isActiveTarget = value; } }

    [SerializeField] private float speed;
    
    [SerializeField] public float radius = 5f; // Radius of the circular path
    
    private float angle = 0f;

    private FFCPlayer _player;

    [SerializeField] private GameObject alertSign;
    private bool surpriseAttack = false;
    
    private FFCEnemyState _currentState = FFCEnemyState.Patrol;
    public FFCEnemyState CurrentState { get { return _currentState; } set { _currentState = value; } }
    
    [SerializeField] private float damage;
    [SerializeField] private float damageRadius;

    [SerializeField] private float lifePoints = 5;

    private FFCGameLevelValues _ffcLevelValues;

    private Vector3 targetPosition = new Vector3();

    private float hitTimer = 0;
    [SerializeField] private float hitTimerMax = 1.5f;
    [SerializeField] private float hitRecoverySpeed = 1.5f;
    
    [SerializeField] private int _hitPoints = 1;
    [SerializeField] private int _killPoints = 5;
    
    void Start()
    {
        _ffcLevelValues = gameManager.GetFFCCurrentLevelValues();
        _player = _ffcLevelValues.Player;
        
        angle = CalculateAngle(_player.transform.position, transform.position);
        CalculateNewTargetPosition();
    }

    void Update()
    {
        switch (_currentState)
        {
            case FFCEnemyState.Patrol:
                surpriseAttack = false;
                Patrol();
                break;
            case FFCEnemyState.Chase:
                surpriseAttack = true;
                Chase();
                break;
            case FFCEnemyState.Attack:
                surpriseAttack = false;
                Attack();
                break;
            case FFCEnemyState.Dead:
                surpriseAttack = false;
                break;
        }
        alertSign.SetActive(surpriseAttack);
        _markedTarget.SetActive(_isActiveTarget);
    }

    public int ReceiveDamage(float currentAttackPoints)
    {
        lifePoints -= currentAttackPoints;
        
        _ffcLevelValues.SetEnemyAsAttacker(this);
        if (lifePoints > 0)
            return _hitPoints;
        
        // Kill and deregister enemy
        _currentState = FFCEnemyState.Dead;
        _isActiveTarget = false;
        _ffcLevelValues.DeleteEnemy(this);
        Destroy(gameObject);
        
        return _killPoints;
    }

    private void Patrol()
    {
        if (Vector3.Distance(transform.position, targetPosition) <= 0.1)
        {
            CalculateNewTargetPosition();
        }
        
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
    }

    private void CalculateNewTargetPosition()
    {
        // Increment the angle based on speed and time
        angle += speed * Time.deltaTime;

        Vector3 playerOffset = new Vector3();
        // Calculate the circular position around the player
        playerOffset.x = Mathf.Cos(angle) * radius;
        playerOffset.y = Mathf.Sin(angle) * radius;
        
        targetPosition = _player.transform.position + playerOffset;
    }
    
    private float CalculateAngle(Vector3 a, Vector3 b)
    {
        Vector3 direction = b - a;

        Vector2 direction2D = new Vector2(direction.x, direction.y);

        float newAngle = Vector2.Angle(Vector2.right, direction2D);

        float sign = Mathf.Sign(Vector3.Cross(Vector3.right, direction2D).z);
        newAngle *= sign;

        return newAngle;
    }
    
    private void Attack()
    {
        hitTimer = hitTimerMax;
        _player.ReceiveDamage(damage);
        _currentState = FFCEnemyState.Chase;
    }
    
    private void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, Time.deltaTime * speed);
        
        hitTimer -= Time.deltaTime * hitRecoverySpeed;
        if (hitTimer > 0)
            return;
            
        if (Vector3.Distance(transform.position, _player.transform.position) <= 0.1)
        {
            _currentState = FFCEnemyState.Attack;
        }
    }

}
