using System.Collections;
using UnityEngine;

public class SecondMonsterSkill : MonsterSkill
{
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _parent;
    [SerializeField] private GameObject _warningPrefab;
    [SerializeField] private GameObject _skillPrefab;
    
    private BoxCollider _collider;

    private void Awake()
    {
        _collider = _skillPrefab.GetComponent<BoxCollider>();
        _collider.enabled = false;
    }

    public override void OnSkill()
    {
        StartCoroutine(RockPunch());
    }

    private IEnumerator RockPunch()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_player.position - transform.position);
        
        GameObject boxWarning = Instantiate(
            _warningPrefab,
            new Vector3(0f, 0f, transform.position.z).normalized * 5f,
            targetRotation,
            _parent.transform
            );
        
        yield return new WaitForSeconds(2f);
        
        Destroy(boxWarning);
        _collider.enabled = true;
        
        yield return new WaitForSeconds(0.1f);
        
        _collider.enabled = false;
    }
}
