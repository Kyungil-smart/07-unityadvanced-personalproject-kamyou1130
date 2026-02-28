using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillWarning : MonoBehaviour
{
    [SerializeField] private Image _fillImage;

    private float _timer;

    public float _skillTime = 2f;
    
    private void Start()
    {
        _fillImage.transform.localScale = Vector3.zero;
        _timer = 0;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        float ratio = (_timer / _skillTime);   

        _fillImage.transform.localScale = new Vector3(ratio, ratio, 1f);
    }
}
