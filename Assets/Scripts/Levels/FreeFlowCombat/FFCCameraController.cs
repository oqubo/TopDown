using UnityEngine;

public class FFCCameraController : MonoBehaviour
{
    private Transform _player;
    public Vector3 offset;

    private void Start()
    {
        _player = FindFirstObjectByType<FFCPlayer>().GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        gameObject.transform.position = _player.position + offset;
    }
}
