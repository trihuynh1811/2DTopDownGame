using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class SingularityPullable : MonoBehaviour
{
    //Add this script to objects you want to be pulled by the singularity script
    public bool pullable = true;
}
