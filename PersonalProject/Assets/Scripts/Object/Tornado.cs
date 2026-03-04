using UnityEngine;

public class Tornado : MonoBehaviour
{
    private void Update()
    {
        Destroy(gameObject, 3f);
    }
}
