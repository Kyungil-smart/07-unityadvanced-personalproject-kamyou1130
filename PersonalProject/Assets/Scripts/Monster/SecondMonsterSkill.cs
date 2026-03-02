using System.Collections;
using UnityEngine;

public class SecondMonsterSkill : MonsterSkill
{
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _parent;
    [SerializeField] private GameObject _warningPrefab;
    [SerializeField] private GameObject _skillPrefab;
    
    private BoxWarning _boxWarning;

    private void Awake()
    {
        _boxWarning = _warningPrefab.GetComponent<BoxWarning>();
    }

    public override void OnSkill()
    {
        StartCoroutine(RockPunch());
    }

    private IEnumerator RockPunch()
    {
        GameObject boxWarning = Instantiate(
            _warningPrefab,
            transform.position + transform.forward * 10f,
            transform.rotation,
            _parent.transform
            );
        
        yield return new WaitForSeconds(_boxWarning._boxSkillTime);
        
        Destroy(boxWarning);

        Instantiate(_skillPrefab,
            transform.position + transform.forward * 3f,
            transform.rotation
        );
    }
}
