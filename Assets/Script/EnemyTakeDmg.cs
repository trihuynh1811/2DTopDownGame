using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDmg : MonoBehaviour, ITakeDamage
{
    [SerializeField] GameObject floatingText;
    [SerializeField] Transform floatingTextPos;
    [SerializeField] Vector2 randomFloatingTextPos;
    public int health;
    [SerializeField] SpriteRenderer enemySprite;
    [SerializeField] Material hurtMat;
    Material originalMat;
    [SerializeField] float flashDuration;
    float currentFlashDuration;
    Vector2 newFloatingTextPos;

    private void Awake()
    {
        originalMat = enemySprite.material;
        currentFlashDuration = flashDuration;

    }
    public void TakeDamage(int dmg)
    {
        if(floatingText != null)
        {
            ShowDamage(dmg);
        }
        health -= dmg;
        StartCoroutine(GetHit());
    }
    
    IEnumerator GetHit()
    {
        enemySprite.material = hurtMat;
        yield return new WaitForSeconds(flashDuration);
        enemySprite.material = originalMat;

    }

    void ShowDamage(float dmg)
    {
        newFloatingTextPos = new(floatingTextPos.position.x + Random.Range(-randomFloatingTextPos.x, randomFloatingTextPos.x), floatingTextPos.position.y + Random.Range(0, randomFloatingTextPos.y));
        GameObject floatingTextClone = ObjectPoolManager.SpawnObject(floatingText, newFloatingTextPos, Quaternion.identity);
        floatingTextClone.transform.SetParent(gameObject.transform);
        floatingTextClone.GetComponent<TextMesh>().text = dmg.ToString();
    }
}
