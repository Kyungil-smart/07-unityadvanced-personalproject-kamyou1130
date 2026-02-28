using System.Collections;
using UnityEngine;

public class FirstMonsterSkill : MonsterSkill
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _parent;
    [SerializeField] private GameObject _warningPrefab;
    [SerializeField] private GameObject _skillPrefab;
    [SerializeField] private float _skillRange;
    
    public override void OnSkill()
    {
        StartCoroutine(RockDrop());
    }

    private IEnumerator RockDrop()
    {
        Vector3 playerPos = _player.transform.position;
        
        GameObject warning = Instantiate(_warningPrefab, playerPos, Quaternion.identity, _parent.transform);

        yield return new WaitForSeconds(1f);
        
        Destroy(warning);
        
        
    }
}
