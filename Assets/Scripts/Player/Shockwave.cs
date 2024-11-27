using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    [SerializeField] private float _shockWaveTime = 0.75f;

    private Coroutine _shockWaveCoroutine;
    private Material _material;

    public Transform PlayerPos;

    private static int _waveDistanceFromCenter = Shader.PropertyToID("_WaveDiff");

    public float dis = _waveDistanceFromCenter;

    public ParticleSystem[] particles;

    public bool freezeParticle;

    public GameObject rippleMask;

    private void Awake() {
        _material = GetComponent<SpriteRenderer>().material;
    }

    private void Update() {
        this.transform.position = PlayerPos.position;
        if(freezeParticle) {
            foreach (ParticleSystem particle in particles) {
                particle.Pause();
            }   
        }
    }

    public void CallShockWave() {
        _material.SetFloat(_waveDistanceFromCenter, 0.38f);
        _shockWaveCoroutine = StartCoroutine(ShockWaveAction(0.38f, 0.08f));
        freezeParticle = true;
    }

    public void UnCallShockWave() {
        _material.SetFloat(_waveDistanceFromCenter, 0.08f);
        _shockWaveCoroutine = StartCoroutine(ShockWaveAction(0.08f, 0.5f));
        freezeParticle = false;
    }

    private IEnumerator ShockWaveAction(float startpos, float endpos) {
        _material.SetFloat(_waveDistanceFromCenter, -0.1f);

        float lerpedAmount = 0f;
        float sizeAmount = 0f;

        float elapsedTime = 0f;
        while(elapsedTime < _shockWaveTime) {
            elapsedTime += Time.deltaTime;

            lerpedAmount = Mathf.Lerp(startpos, endpos, (elapsedTime / _shockWaveTime));
            sizeAmount = Mathf.Lerp(startpos * 60f, endpos * 60f, (elapsedTime / _shockWaveTime));

            _material.SetFloat(_waveDistanceFromCenter, lerpedAmount);
            rippleMask.transform.localScale = new Vector2(sizeAmount, sizeAmount);
            dis = lerpedAmount;

            yield return null;
        }
    }
}
