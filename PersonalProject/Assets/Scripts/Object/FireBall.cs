using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _attackDamage;
    
    private void Update()
    {
        transform.position += Vector3.back * (Time.deltaTime * _speed);
        
        Destroy(gameObject, 1.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

            playerController.TakeDamage(_attackDamage);
        }
    }
}
