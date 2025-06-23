using UnityEngine;
using UnityEngine.SceneManagement;

public class EPlayer : MonoBehaviour
{
    [SerializeField] protected GameManagerSo gameManager;
    
    [SerializeField] protected int Speed = 5;
    
    void Start()
    {
        
    }

    void Update()
    {
        UpdateMovement();
        
        // Temporary function to change to combat scene
        if (Input.GetKey(KeyCode.Space))
            SceneManager.LoadScene(gameManager.LevelHandler.nextScene);
    }
    
    private void UpdateMovement()
    {
        // Left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(MathConstants.Left * (Time.deltaTime * Speed));
        }
        
        // Right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(MathConstants.Right * (Time.deltaTime * Speed));
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
