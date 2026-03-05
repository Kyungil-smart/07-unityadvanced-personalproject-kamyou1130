using UnityEngine;

public class CameraFix : MonoBehaviour
{
    [SerializeField] private Transform _player;
    
    [SerializeField] private Vector3 _offset;
    
    private PlayerController _playerController;

    private void Awake()
    {
        if(_player == null) _player = GameObject.FindGameObjectWithTag("Player").transform;
        if (_player != null) _playerController = _player.GetComponent<PlayerController>();
        if (_playerController._camera == null) _playerController._camera = Camera.main;
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
