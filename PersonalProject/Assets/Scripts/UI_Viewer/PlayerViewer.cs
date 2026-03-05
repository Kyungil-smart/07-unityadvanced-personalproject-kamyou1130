using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerViewer : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    
    [SerializeField] private TextMeshProUGUI _hpText;

    public void SetPlayerHp(int currentHp, int maxHp)
    {
        _fillImage.fillAmount = currentHp / (float)maxHp;
        
        _hpText.text = $"{currentHp} / {maxHp}";
    }
}
