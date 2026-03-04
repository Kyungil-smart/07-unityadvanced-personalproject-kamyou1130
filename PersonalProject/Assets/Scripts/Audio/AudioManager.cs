using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private GameObject _prefab;
    
    private void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Init();
    }

    private void Init()
    {
        _prefab = Resources.Load<GameObject>("AudioSFX");
    }

    public void PlaySound(AudioClip clip, float volume = 1.0f, float pitch = 1.0f)
    {
        if (_prefab == null) return;
        if (clip == null) return;
        
        GameObject obj = PoolManager.Instance.Get(Resources.Load<GameObject>("AudioSFX"), Vector3.zero, Quaternion.identity);

        if (obj.TryGetComponent(out AudioSource audioSource))
        {
            
        }
    }
}
