using System.Collections;
using UnityEngine;

public class ThirdMonsterSkill : MonsterSkill
{
    [SerializeField] protected Transform _player;
    [SerializeField] protected GameObject _parent;
    [SerializeField] private GameObject _warningPrefab;
    [SerializeField] private GameObject _skillPrefab;

    private SkillWarning _skillwarning;
    
    private void Awake()
    {
        _skillwarning = _warningPrefab.GetComponent<SkillWarning>();
    }
    
    public override void OnSkill()
    {
        StartCoroutine(Tornado());
    }

    private IEnumerator Tornado()
    {
        Vector3 playerPos = _player.transform.position;
        
        GameObject warning = Instantiate(_warningPrefab,
            new Vector3(playerPos.x, 0f, playerPos.z), 
            Quaternion.identity, 
            _parent.transform);
        
        yield return new WaitForSeconds(_skillwarning._circleSkillTime);
        
        Destroy(warning);
                
        Instantiate(
            _skillPrefab, 
            new Vector3(playerPos.x, 0f, playerPos.z),
            Quaternion.identity);
    }
}
