using UnityEngine;

public class Magic : MonoBehaviour
{
    [SerializeField] private int _attackDamage;

    private float _attackTime;
    private PlayerController _playerController;
    private float _timer;
    
    [SerializeField] private GameObject _tornadoPrefab;
    
    private void Awake()
    {
        _timer = 0f;
        _attackTime = 1f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        
        Destroy(gameObject, 4f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerController = other.gameObject.GetComponent<PlayerController>();

            _playerController._moveSpeed *= 0.5f;

            if (_timer >= _attackTime)
            {
                _playerController.TakeDamage(_attackDamage);
                Debug.Log($"{_attackDamage} 피해!");
                _timer = 0f;
            } 
        }
    }

    private void OnDestroy()
    {
        Vector3 ray = transform.position + Vector3.up * 5f;
        Vector3 hitPoint = new Vector3();
            
        if (Physics.Raycast(ray, Vector3.down, out RaycastHit hit, 10f, LayerMask.GetMask("Ground")))
        {
             hitPoint = hit.point;
        }
        
        Instantiate(_tornadoPrefab, hitPoint, Quaternion.identity);
    }
}
