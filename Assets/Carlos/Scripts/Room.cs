using UnityEngine;

public class Room : MonoBehaviour
{

    [SerializeField] private GameObject topDoor, bottomDoor, leftDoor, rightDoor;

    public Vector2Int RoomIndex { get; set; }

    
    
    public void OpenDoor(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            topDoor.SetActive(false);
        }
        else if (direction == Vector2Int.down)
        {
            bottomDoor.SetActive(false);
        }
        else if (direction == Vector2Int.left)
        {
            leftDoor.SetActive(false);
        }
        else if (direction == Vector2Int.right)
        {
            rightDoor.SetActive(false);
        }
    }

}
