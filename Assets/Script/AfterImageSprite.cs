using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageSprite : MonoBehaviour
{
    [SerializeField] float activeTime = .1f;
    float timeActivated;
    float alpha;
    [SerializeField] float alphaSet = .8f;
    float alphaMultiplier = .85f;

    public Transform objectTransform { get; set; }

    [SerializeField] SpriteRenderer sr;
    public SpriteRenderer objectSr { get; set; }

    Color color;

    private void OnEnable()
    {
        alpha = alphaSet;
        sr.sprite = objectSr.sprite;
        transform.position = objectTransform.position;
        transform.rotation = objectTransform.rotation;
        timeActivated = Time.time;
    }

    private void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(1, 1, 1, alpha);
        sr.color = color;

        if (Time.time >= (timeActivated + activeTime))
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
