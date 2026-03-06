using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public static SceneManage Instance;
    
    [SerializeField] private PlayerController _playerController;
    
    private string _currentScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void LoadScene(string sceneName)
    {
        _currentScene = sceneName;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void RegisterPlayer(GameObject player)
    {
        _playerController = player.GetComponent<PlayerController>();
        
        if (_currentScene == "VillageScene")
        {
            _playerController._isLock = false;
            _playerController.UnLocking();
        }
    }
}
