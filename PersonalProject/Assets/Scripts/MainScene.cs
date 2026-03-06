using UnityEngine;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    
    [SerializeField] private Button _explainButton;
    [SerializeField] private GameObject _explainPanel;
    [SerializeField] private Button _explainExitButton;
    
    [SerializeField] private Button _exitButton;
    
    private void Awake()
    {
        _explainPanel.SetActive(false);
    }

    private void Start()
    {
        _startButton.onClick.AddListener(StartButton);
        _explainButton.onClick.AddListener(ExplainButton);
        _explainExitButton.onClick.AddListener(ExplainPanelExit);
        _exitButton.onClick.AddListener(ExitButton);
    }

    private void StartButton()
    {
        SceneManage.Instance.LoadScene("ForestScene");
    }

    private void ExplainButton()
    {
        _explainPanel.SetActive(true);
    }

    private void ExplainPanelExit()
    {
        _explainPanel.SetActive(false);
    }

    private void ExitButton()
    {
        Application.Quit();
    }
}
