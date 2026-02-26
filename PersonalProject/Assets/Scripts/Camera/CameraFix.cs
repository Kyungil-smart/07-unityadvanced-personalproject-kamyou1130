using UnityEngine;

public class CameraFix : MonoBehaviour
{
    [SerializeField] private Transform _player;
    
    private Vector3 _offset;

    private void Awake()
    {
        _offset = new Vector3(0f, 8f, -8f);
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(45f, 0f, 0f);
    }

    private void LateUpdate()
    {
        transform.position = _player.position + _offset;
    }
}
