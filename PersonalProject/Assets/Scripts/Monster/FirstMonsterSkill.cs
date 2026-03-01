using System.Collections;
using UnityEngine;

public class FirstMonsterSkill : MonsterSkill
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _parent;
    [SerializeField] private GameObject _warningPrefab;
    [SerializeField] private GameObject _skillPrefab;
    
    private SkillWarning _skillwarning;

    private void Awake()
    {
        _skillwarning = _warningPrefab.GetComponent<SkillWarning>();
    }
    
    public override void OnSkill()
    {
        StartCoroutine(RockDrop());
    }

    private IEnumerator RockDrop()
    {
        Vector3 playerPos = _player.transform.position;
        
        GameObject warning = Instantiate(_warningPrefab, playerPos, Quaternion.identity, _parent.transform);
                
        Instantiate(
            _skillPrefab, 
            new Vector3(playerPos.x, playerPos.y + 11f, playerPos.z),
            Quaternion.identity);

        yield return new WaitForSeconds(_skillwarning._circleSkillTime);
        
        Destroy(warning);
    }
}
