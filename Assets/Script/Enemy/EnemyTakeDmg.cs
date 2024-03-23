using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTakeDmg : MonoBehaviour, ITakeDamage
{
    [SerializeField] GameObject floatingText;
    [SerializeField] Transform floatingTextPos;
    [SerializeField] Vector2 randomFloatingTextPos;
    public int health;
    [SerializeField] bool multipleEnemySpriteList;
    [SerializeField] List<SpriteRenderer> enemySpriteList;
    [SerializeField] SpriteRenderer enemySprite;
    [SerializeField] Material hurtMat;
    Material originalMat;
    List<Material> originMatList = new List<Material>();
    [SerializeField] float flashDuration;
    public Transform healthCanvasPos;
    public GameObject healtCanvas;
    public Image healthBar;
    float currentFlashDuration;
    public int maxHealth { get; set; }
    Vector2 newFloatingTextPos;

    private void Awake()
    {
        if(healthBar != null && healtCanvas != null)
        {
            healthBar.fillAmount = health;
        }
    }

    private void Start()
    {
        maxHealth = health;
        if (multipleEnemySpriteList)
        {
            for (int i = 0; i < enemySpriteList.Count; i++)
            {
                originMatList.Add(enemySpriteList[i].material);
            }
        }
        else
        {
            originalMat = enemySprite.material;
        }
        currentFlashDuration = flashDuration;

    }
    public void TakeDamage(int dmg)
    {
        if (floatingText != null)
        {
            ShowDamage(dmg);
        }
        health -= dmg;
        if (healthBar != null && healtCanvas != null)
        {
            Debug.Log((float)health / maxHealth);
            healthBar.fillAmount = (float)health / maxHealth;
        }
        StartCoroutine(GetHit());
    }

    public void HealthCanvasFollow()
    {
        healtCanvas.transform.position = healthCanvasPos.position;
    }

    IEnumerator GetHit()
    {
        if (multipleEnemySpriteList)
        {
            for (int i = 0; i < enemySpriteList.Count; i++)
            {
                enemySpriteList[i].material = hurtMat;
            }
        }
        else
        {
            enemySprite.material = hurtMat;
        }
        yield return new WaitForSeconds(flashDuration);
        if (multipleEnemySpriteList)
        {
            for (int i = 0; i < enemySpriteList.Count; i++)
            {
                enemySpriteList[i].material = originMatList[i];
            }
        }
        else
        {
            enemySprite.material = originalMat;
        }
    }

    void ShowDamage(float dmg)
    {
        newFloatingTextPos = new(floatingTextPos.position.x + Random.Range(-randomFloatingTextPos.x, randomFloatingTextPos.x), floatingTextPos.position.y + Random.Range(0, randomFloatingTextPos.y));
        GameObject floatingTextClone = ObjectPoolManager.SpawnObject(floatingText, newFloatingTextPos, Quaternion.identity);
        floatingTextClone.transform.SetParent(gameObject.transform);
        floatingTextClone.GetComponent<TextMesh>().text = dmg.ToString();
    }
}
