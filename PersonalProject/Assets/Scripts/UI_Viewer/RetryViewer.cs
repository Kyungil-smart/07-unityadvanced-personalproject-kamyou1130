using UnityEngine;
using UnityEngine.UI;

public class RetryViewer : MonoBehaviour
{
    [SerializeField] private GameObject _retryPanel;
    [SerializeField] private Button _mainButton;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _mainButton.onClick.AddListener(Main);
        _exitButton.onClick.AddListener(Exit);
    }

    private void Start()
    {
        _retryPanel.SetActive(false);
    }

    public void Main()
    {
        SceneManage.Instance.LoadScene("MainMenuScene");
        _retryPanel.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
        _retryPanel.SetActive(false);
    }

    public void SetActivePanel()
    {
        _retryPanel.SetActive(true);
    }
}
