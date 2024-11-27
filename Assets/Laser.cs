using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Transform player;

    public LayerMask ground;

    public LineRenderer line;

    public Transform dot;

    void Update() {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.position - transform.position, 200, ground);

        line.SetPosition(1, new Vector2(ray.point.x - transform.position.x, ray.point.y - transform.position.y + 0.12f));

        Debug.DrawRay(transform.position, player.position - transform.position);

        dot.position = ray.point;
    }
}
