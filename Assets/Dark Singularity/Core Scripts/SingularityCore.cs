using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
public class SingularityCore : MonoBehaviour
{
    //This script is responsible for what happens when the pullable objects reach the core
    //by default, the game objects are simply turned off
    //as this is much more performant than destroying the objects
    void OnTriggerStay2D (Collider2D other) {
        if(other.GetComponent<SingularityPullable>()){
            //other.gameObject.SetActive(true);
            //Destroy(other.gameObject, 2);
        }
    }

    void Awake(){
        if(GetComponent<CircleCollider2D>()){
            GetComponent<CircleCollider2D>().isTrigger = true;
        }
    }
}
