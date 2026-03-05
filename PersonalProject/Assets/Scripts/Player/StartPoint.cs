using UnityEngine;

public class StartPoint : MonoBehaviour
{
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        _player.transform.position = transform.position;
    }
}
