using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Laser : MonoBehaviour
{
    public Transform player;

    public LayerMask bothLayers;

    public LayerMask groundLayer;
    public LayerMask playerLayer;

    public LineRenderer line;

    public Transform dot;

    public bool enemyEnabled;
    public bool canSee;

    public bool deactivate;

    public Vector2 prevPos;

    public Transform sprite;

    public Transform laserPoint;

    void Update() {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.position - transform.position, 200, bothLayers);
        RaycastHit2D rayGround = Physics2D.Raycast(transform.position, prevPos, 200, bothLayers);

        if(enemyEnabled) {

            canSee = Physics2D.OverlapBox(ray.point, new Vector2(0.5f, 0.5f), 0, playerLayer);
            if(canSee) {
                line.SetPosition(1, new Vector2(ray.point.x - transform.position.x, ray.point.y - transform.position.y));

                line.SetPosition(0, laserPoint.position - transform.position);

                Debug.DrawRay(transform.position, player.position - transform.position);

                dot.position = ray.point;

                prevPos = new Vector2(ray.point.x - transform.position.x, ray.point.y - transform.position.y);

                Vector3 relativePos = player.position - transform.position;
                float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
                sprite.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            } else {
                line.SetPosition(1, new Vector2(rayGround.point.x - transform.position.x, rayGround.point.y - transform.position.y));

                line.SetPosition(0, laserPoint.position - transform.position);

                dot.position = rayGround.point;
            }
        } else {
            line.SetPosition(1, new Vector2(rayGround.point.x - transform.position.x, rayGround.point.y - transform.position.y));

            line.SetPosition(0, laserPoint.position - transform.position);

            dot.position = rayGround.point;

            if(deactivate) {
                deactivate = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == 13) {
            enemyEnabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.layer == 13) {
            enemyEnabled = false;
            canSee = false;
            deactivate = true;
        }
    }
}
