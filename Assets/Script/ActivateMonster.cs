using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateMonster : MonoBehaviour
{
    [SerializeField] AnimationClip spawnAnimation;
    public GameObject monster;
    [SerializeField] GameObject spawnObject;

    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(activate());
    }

    // Update is called once per frame
    IEnumerator activate()
    {
        yield return new WaitForSeconds(spawnAnimation.length);
        Instantiate(monster, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
