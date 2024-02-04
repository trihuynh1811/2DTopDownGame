using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDmg : MonoBehaviour, ITakeDamage
{
    [SerializeField] int health;
    [SerializeField] SpriteRenderer enemySprite;
    [SerializeField] Material hurtMat;
    Material originalMat;
    [SerializeField] float flashDuration;
    float currentFlashDuration;

    private void Awake()
    {
        originalMat = enemySprite.material;
        currentFlashDuration = flashDuration;

    }
    public void TakeDamage(float dmg)
    {
        StartCoroutine(GetHit());
    }
    
    IEnumerator GetHit()
    {
        enemySprite.material = hurtMat;
        yield return new WaitForSeconds(flashDuration);
        enemySprite.material = originalMat;

    }
}
