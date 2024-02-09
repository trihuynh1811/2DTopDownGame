using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    [SerializeField] float maxExistTime;
    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke(nameof(DestroyAfter), maxExistTime);
    }

    // Update is called once per frame
    void DestroyAfter()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
