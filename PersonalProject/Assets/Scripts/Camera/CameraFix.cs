using UnityEngine;

public class CameraFix : MonoBehaviour
{
    [SerializeField] private Transform _player;
    
    [SerializeField] private Vector3 _offset;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(45f, 0f, 0f);
    }

    private void LateUpdate()
    {
        transform.position = _player.position + _offset;
    }
}
