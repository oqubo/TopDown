using UnityEngine;

public class FFCPlayer : MonoBehaviour
{
    [SerializeField] protected GameManagerSo gameManager;
    
    [SerializeField] protected int Speed = 5;
    [SerializeField] private float enemyAcceptableDistance = 40;
    
    public FFCPlayerState CurrentState = FFCPlayerState.Idle;
    private FFCEnemy currentEnemy;
    
    public float CurrentAttackPoints = 1;
    
    private bool _blockMovement = false;

    private Animator anim;
    private float h, v;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        UpdateAttack();
        UpdateParry();
        
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        h = 0;
        v = 0;
        
        if (_blockMovement)
            return;
        
        // Left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(MathConstants.Left * (Time.deltaTime * Speed));
            h = -1;
        }
        
        // Right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(MathConstants.Right * (Time.deltaTime * Speed));
            h = 1;
        }
        
        // Up
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(MathConstants.Up * (Time.deltaTime * Speed));
            v = 1;
        }
        
        // Down
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(MathConstants.Down * (Time.deltaTime * Speed));
            v = -1;
        }
        
        anim.SetBool("andando", true);
        anim.SetFloat("inputV", v);
        anim.SetFloat("inputH", h);
    }

    void UpdateAttack()
    {
        switch (CurrentState)
        {
            case FFCPlayerState.Idle:
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 mousePositionInAttack = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    FFCEnemy closestEnemy = gameManager.GetFFCCurrentLevelValues().ClosestEnemy(mousePositionInAttack, enemyAcceptableDistance);
                    if (closestEnemy is null)
                        return;
                    
                    CurrentState = FFCPlayerState.StartAttack;
                    _blockMovement = true;
                    currentEnemy = closestEnemy;
                    closestEnemy.IsActiveTarget = true;
                }
                break;
            case FFCPlayerState.StartAttack:
                if (Vector3.Distance(transform.position, currentEnemy.transform.position) < 0.1f)
                {
                    CurrentState = FFCPlayerState.Attacking;
                    return;
                }
                transform.position = Vector3.MoveTowards(transform.position, currentEnemy.transform.position, Time.deltaTime * Speed);
                break;
            case FFCPlayerState.Attacking:
                int pointsToAdd = currentEnemy.ReceiveDamage(CurrentAttackPoints);
                GameInstance.Singleton.AddPoints(pointsToAdd);
                CurrentState = FFCPlayerState.Idle;
                _blockMovement = false;
                break;
        }
    }

    void UpdateParry()
    {
        switch (CurrentState)
        {
            case FFCPlayerState.Idle:
                if (Input.GetMouseButtonDown(1))
                {
                    Vector2 mousePositionInAttack = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    FFCEnemy closestEnemy = gameManager.GetFFCCurrentLevelValues().GetChaseEnemy(mousePositionInAttack, enemyAcceptableDistance);
                    if (closestEnemy is null)
                        return;
                    
                    CurrentState = FFCPlayerState.StartAttack;
                    _blockMovement = true;
                    currentEnemy = closestEnemy;
                    closestEnemy.IsActiveTarget = true;
                }
                break;
            case FFCPlayerState.StartAttack:
                if (Vector3.Distance(transform.position, currentEnemy.transform.position) < 0.1f)
                {
                    CurrentState = FFCPlayerState.Attacking;
                    return;
                }
                transform.position = Vector3.MoveTowards(transform.position, currentEnemy.transform.position, Time.deltaTime * Speed * 2);
                break;
            case FFCPlayerState.Attacking:
                int pointsToAdd = currentEnemy.ReceiveDamage(CurrentAttackPoints);
                GameInstance.Singleton.AddPoints(pointsToAdd);
                CurrentState = FFCPlayerState.Idle;
                _blockMovement = false;
                break;
        }
    }

    public void ReceiveDamage(float damage)
    {
        GameInstance.Singleton.currentLifePoints -= damage;
        if (GameInstance.Singleton.currentLifePoints > 0)
            return;
     
        gameManager.PlayerDies();
    }
}
