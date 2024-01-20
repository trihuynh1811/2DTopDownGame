using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer line;
    private RaycastHit2D hit;
    public float lineLength;
    public LayerMask wallMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnDrawGizmos()
    {
        line.enabled = true;
        hit = Physics2D.Raycast(transform.position, transform.right, lineLength, wallMask);
        if (hit)
        {
            float distance = ((Vector2)hit.point - (Vector2)transform.position).magnitude;
            line.SetPosition(1, new Vector2(distance, 0));
        }
        else
        {
            line.SetPosition(1, new Vector2(lineLength, 0));
        }
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.right * lineLength);
    }
}
