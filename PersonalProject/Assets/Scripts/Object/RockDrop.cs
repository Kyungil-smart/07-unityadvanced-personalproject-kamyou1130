using UnityEngine;
using UnityEngine.LowLevelPhysics2D;

public class RockDrop : MonoBehaviour
{
    [SerializeField] private float _fallSpeed;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _damageRadius;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private int _damage;
    
    private Rigidbody _rb;

    // 충돌 작용이 두번 일어나는 것을 방지하기 위한 필드
    private int _count;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        _rb.angularVelocity = Vector3.right * _turnSpeed;
        _rb.linearVelocity = Vector3.down * _fallSpeed;     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Explode();
            
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _damageRadius, _layerMask);

        foreach (Collider hit in colliders)
        {
            hit.GetComponent<PlayerController>()?.TakeDamage(_damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, _damageRadius);
    }
}
