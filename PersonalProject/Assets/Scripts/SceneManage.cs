using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public static SceneManage Instance;
    
    [SerializeField] private PlayerController _playerController;

    private void Awake()
    {
        if (Instance == null && Instance != this)
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
        SceneManager.LoadScene(sceneName);
      
        if (sceneName == "VillageScene")
        {
            _playerController._isLock = false;
            _playerController.UnLocking();
        }
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void RegisterPlayer(GameObject player)
    {
        _playerController = player.GetComponent<PlayerController>();
    }
}
