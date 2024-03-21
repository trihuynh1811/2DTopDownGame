using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
public class Singularity : MonoBehaviour
{
    //This is the main script which pulls the objects nearby
    [SerializeField] public float GRAVITY_PULL = 100f;
    public static float m_GravityRadius = 1f;

    void Awake() {
        m_GravityRadius = GetComponent<CircleCollider2D>().radius;

        if(GetComponent<CircleCollider2D>()){
            GetComponent<CircleCollider2D>().isTrigger = true;
        }
    }
    
    void OnTriggerStay2D (Collider2D other) {
        if(other.attachedRigidbody && other.GetComponent<SingularityPullable>()) {
            float gravityIntensity = Vector3.Distance(transform.position, other.transform.position) / m_GravityRadius;
            other.attachedRigidbody.AddForce((transform.position - other.transform.position).normalized * gravityIntensity * other.attachedRigidbody.mass * GRAVITY_PULL * Time.smoothDeltaTime);
        }
    }
}
