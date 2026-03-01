using UnityEngine;
using UnityEngine.UI;

public class BoxWarning : MonoBehaviour
{
    [SerializeField] private Image _fillImage;

    private float _timer;

    public float _boxSkillTime = 2f;
    
    private void Start()
    {
        _fillImage.fillAmount = 0;
        _timer = 0;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _boxSkillTime) return;
        _fillImage.fillAmount = (_timer / _boxSkillTime);
    }
}
