using System.Collections;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource _audioSource;
    
    private GameObject _originalPrefab;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Setup(GameObject prefab, AudioClip audioClip, float volume, float pitch)
    {
        _originalPrefab = prefab;
        _audioSource.clip = audioClip;
        _audioSource.volume = volume;
        _audioSource.pitch = pitch;
        
        _audioSource.Play();

        StartCoroutine(ReturnToPool(_audioSource.clip.length));
    }

    private IEnumerator ReturnToPool(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        PoolManager.Instance.Release(_originalPrefab, gameObject);
    }
}
