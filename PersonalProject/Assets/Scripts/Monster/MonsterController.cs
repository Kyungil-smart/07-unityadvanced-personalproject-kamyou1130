using UnityEngine;

public class MonsterController : MonoBehaviour, IDamagable
{
    // Monster 데이터 접근 필드 (MVP 패턴)
    private MonsterData _monsterData;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        
    }

    private void Init()
    {
        _monsterData = new MonsterData();
    }
    
    public void TakeDamage(int value)
    {
        _monsterData.MonsterHp -= value;
        if (_monsterData.MonsterHp <= 0)
        {
            Destroy(gameObject);
        }
    }
    
}
