using System;
using UnityEngine;
using UnityEngine.LowLevelPhysics2D;

public class Bomb : MonoBehaviour
{
    private bool _onWind;
    private void Update()
    {
        if (_onWind)
        {
            transform.Translate(Vector3.up * (Time.deltaTime * 5f));
        }
        
        Destroy(gameObject, 5f);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wind"))
        {
            _onWind = true;
        }
        
        if (other.gameObject.CompareTag("Monster"))
        {
            MonsterController monsterController = other.gameObject.GetComponent<MonsterController>();
            
            Destroy(gameObject);
        }
    }
}
