using System.Collections;
using UnityEngine;

public class FourthMonsterSkill : MonsterSkill
{
    [SerializeField] protected GameObject _parent;
    [SerializeField] private GameObject _warningPrefab;
    [SerializeField] private GameObject _skillPrefab;

    private GameObject[] warnings;
    [SerializeField] private int _count;
    
    private BoxWarning _boxWarning;

    private void Awake()
    {
        _boxWarning = _warningPrefab.GetComponent<BoxWarning>();
        warnings = new GameObject[_count];
    }
    
    public override void OnSkill()
    {
        StartCoroutine(Laser());
    }

    private IEnumerator Laser()
    {
        for (int i = 0; i < _count; i++)
        {
            warnings[i] = Instantiate(
                _warningPrefab, 
                new Vector3(transform.position.x - (5f * (i - (_count / 2))), 0f, transform.position.z), 
                Quaternion.identity, 
                _parent.transform);    
        }
        
        yield return new WaitForSeconds(_boxWarning._boxSkillTime);

        foreach (GameObject warning in warnings)
        {
            Destroy(warning);
        }

        for (int i = 0; i < _count; i++)
        {
            Instantiate(
                _skillPrefab,
                new Vector3(transform.position.x - (5f * (i - (_count / 2))), 0f, transform.position.z + 12f),
                Quaternion.identity);
        }
    }
}
