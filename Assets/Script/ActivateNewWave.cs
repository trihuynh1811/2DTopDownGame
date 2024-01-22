using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateNewWave : MonoBehaviour
{
    [SerializeField] CircleCollider2D cc2d;
    [SerializeField] GameObject startNewWaveTxt;
    [SerializeField] GameManager gameManager;

    private void Awake()
    {
        this.gameObject.SetActive(false);
        startNewWaveTxt.SetActive(false);
    }

    private void OnEnable()
    {
        startNewWaveTxt.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            startNewWaveTxt.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                gameManager.SetNewWaveStart(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            startNewWaveTxt.SetActive(false);
        }
    }
}
