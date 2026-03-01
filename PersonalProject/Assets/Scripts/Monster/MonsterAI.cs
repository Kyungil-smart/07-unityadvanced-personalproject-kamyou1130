using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    [SerializeField] private Transform _playerTf;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    public bool _canMove;

    private void Awake()
    {
        if(_playerTf == null) _playerTf = GameObject.FindGameObjectWithTag("Player").transform;
        if(_navMeshAgent == null) _navMeshAgent = FindAnyObjectByType<NavMeshAgent>();
    }

    private void Update()
    {
        if (_canMove)
        {
            _navMeshAgent.isStopped = false;
            FollowPlayer();   
        }
        else
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.ResetPath();
        }
    }

    private void FollowPlayer()
    {
        _navMeshAgent.SetDestination(_playerTf.position);
    }
}
