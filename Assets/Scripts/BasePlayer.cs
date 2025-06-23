using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    [SerializeField] 
    protected GameObject gameCore;
    
    protected GameCore GameCoreRef;
    
    [SerializeField]
    protected int Speed = 5;
    
    protected void Start()
    {
        GameCoreRef = gameCore.GetComponent<GameCore>();
    }
    
    protected void Update()
    {
        UpdateMovement();
    }
    
    void UpdateMovement()
    {
        // Left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-MathConstants.Left * (Time.deltaTime * Speed));
        }
        
        // Right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(-MathConstants.Right * (Time.deltaTime * Speed));
        }
        
        // Up
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(MathConstants.Up * (Time.deltaTime * Speed));
        }
        
        // Down
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(MathConstants.Down * (Time.deltaTime * Speed));
        }
    }
}
