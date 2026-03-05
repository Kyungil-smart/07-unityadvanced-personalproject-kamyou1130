using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillIconViewer : MonoBehaviour
{
    [SerializeField] private Image _fillAttackImage;
    [SerializeField] private TextMeshProUGUI _attackCooltimeText;

    [SerializeField] private Image _fillDashImage;
    [SerializeField] private TextMeshProUGUI _dashCooltimeText;
    
    [SerializeField] private Image _fillBombImage;
    [SerializeField] private TextMeshProUGUI _bombCooltimeText;
    
    public void SetAttack(float currentTime, float maxTime)
    {
        if (currentTime < maxTime)
        {
            _fillAttackImage.fillAmount = 1 - (currentTime / maxTime);
        }
        else
        {
            _fillAttackImage.fillAmount = 0;
        }

        if (currentTime < maxTime)
        {
            _attackCooltimeText.text = $"{maxTime - currentTime:F1}";
        }
        else
        {
            _attackCooltimeText.text = "";
        }

    }

    public void SetDash(float currentTime, float maxTime)
    {
        if (currentTime < maxTime)
        {
            _fillDashImage.fillAmount = 1 - (currentTime / maxTime);
        }
        else
        {
            _fillDashImage.fillAmount = 0;
        }

        if (currentTime < maxTime)
        {
            _dashCooltimeText.text = $"{maxTime - currentTime:F1}";
        }
        else
        {
            _dashCooltimeText.text = "";
        }
    }
    
    public void SetBomb(float currentTime, float maxTime)
    {
        if (currentTime < maxTime)
        {
            _fillBombImage.fillAmount = 1 - (currentTime / maxTime);
        }
        else
        {
            _fillBombImage.fillAmount = 0;
        }


        if (currentTime < maxTime)
        {
            _bombCooltimeText.text = $"{maxTime - currentTime:F1}";
        }
        else
        {
            _bombCooltimeText.text = "";
        }

    }
}
