using UnityEngine;
using UnityEngine.UI;

public class MonsterFly : MonoBehaviour
{
    private Ray _ray;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private GameObject _underMarkObject;
    
    private void Update()
    {
        UnderMark();
    }

    private void UnderMark()
    {
        _ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        
        if (Physics.Raycast(_ray, out hit, 10f, _layerMask))
        {
            Vector3 targetPoint = hit.point;

            _underMarkObject.transform.position = hit.point;
        }
    }
}
