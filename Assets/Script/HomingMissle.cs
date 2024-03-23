using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle : MonoBehaviour
{
    public enum MissleType
    {
        Coin,
        Energy,
        Health,
        NormalMissle
    }
    public MissleType missleType;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float transformRotationSpeed, speed;
    [SerializeField] int amountOfCoin;
    Transform player;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        Vector2 force = speed * Time.deltaTime * direction;

        float rotateAmount = Vector3.Cross(direction, transform.right).z;

        rb.angularVelocity = -rotateAmount * transformRotationSpeed;

        rb.AddForce(force);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                switch (missleType)
                {
                    case MissleType.Coin:
                        gameObject.transform.parent = TopDownPlayerMovement.instance.itemPos;
                        gameObject.transform.position = Vector3.zero;
                        TopDownPlayerMovement.instance.coin += Random.Range(1, amountOfCoin);
                        TopDownPlayerMovement.instance.UpdateUi();
                        if (GameManager.itemList.Count > 0 && GameManager.itemList.Find(x => x == gameObject))
                        {
                            GameManager.itemList.Remove(gameObject);
                        }
                        gameObject.SetActive(false);
                        break;
                    case MissleType.Energy:
                        gameObject.transform.parent = TopDownPlayerMovement.instance.itemPos;
                        gameObject.transform.position = Vector3.zero;
                        TopDownPlayerMovement.instance.AddMoreEnergy(Random.Range(1, 5));
                        TopDownPlayerMovement.instance.UpdateUi();
                        if (GameManager.itemList.Count > 0 && GameManager.itemList.Find(x => x == gameObject))
                        {
                            GameManager.itemList.Remove(gameObject);
                        }
                        gameObject.SetActive(false);
                        break;
                }
                break;
        }
    }
}
