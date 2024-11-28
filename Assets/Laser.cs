using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Laser : MonoBehaviour
{
    public Transform player;

    public LayerMask bothLayers;
    public LayerMask playerLayer;

    public LineRenderer line;

    public Transform dot;

    public bool enemyEnabled;
    public bool canSee;

    public bool deactivate;

    public Vector2 prevPos;

    public Transform sprite;

    public Transform laserPoint;
    
    public Animator anim;

    public ParticleSystem[] _ps;

    private void Start() {
        foreach (ParticleSystem particle in _ps) particle.Play();
        
    }

    void Update() {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.position - transform.position, 10, bothLayers);
        RaycastHit2D rayGround = Physics2D.Raycast(transform.position, prevPos, 200, bothLayers);

        if(enemyEnabled) {
            anim.speed = 1;

            canSee = Physics2D.OverlapBox(ray.point, new Vector2(0.5f, 0.5f), 0, playerLayer);
            if(canSee) {
                anim.SetBool("Enabled", true);
                line.SetPosition(1, new Vector2(ray.point.x - transform.position.x, ray.point.y - transform.position.y));

                line.SetPosition(0, laserPoint.position - transform.position);

                Debug.DrawRay(transform.position, player.position - transform.position);

                dot.position = ray.point;

                prevPos = new Vector2(ray.point.x - transform.position.x, ray.point.y - transform.position.y);

                Vector3 relativePos = player.position - transform.position;
                float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
                sprite.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            } else {
                anim.SetBool("Enabled", false);
                line.SetPosition(1, new Vector2(rayGround.point.x - transform.position.x, rayGround.point.y - transform.position.y));

                line.SetPosition(0, laserPoint.position - transform.position);

                dot.position = rayGround.point;
            }
        } else {
            
            anim.speed = 0;
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
            foreach (ParticleSystem particle in _ps)particle.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.layer == 13) {
            enemyEnabled = false;
            canSee = false;
            deactivate = true;
            foreach (ParticleSystem particle in _ps)particle.Pause();
        }
    }
}
