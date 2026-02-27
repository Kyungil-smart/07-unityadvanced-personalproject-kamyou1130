using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    [SerializeField] private Transform _playerTf;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        if(_playerTf == null) _playerTf = GameObject.FindGameObjectWithTag("Player").transform;
        if(_navMeshAgent == null) _navMeshAgent = FindAnyObjectByType<NavMeshAgent>();
    }

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        _navMeshAgent.SetDestination(_playerTf.position);
    }
}
