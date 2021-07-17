using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ProximityMine : MonoBehaviour
{
    [Header("Mine")]
    [SerializeField]    
    private float _explodeDuration = 5.0f;

    [SerializeField]
    private float _explosionStrength = 5;

    [SerializeField]    
    private float _minPitch = 1.0f;

    [SerializeField]    
    private float _maxPitch = 8.0f;

    [SerializeField]    
    private AnimationCurve _beepCurve;

    [SerializeField]
    private LayerMask _onlyMovable;

    [SerializeField]
    private bool _immediatelyTriggered;

    [Header("Color")]
    [SerializeField]
    private MeshRenderer _beeperMeshRenderer;

    [SerializeField]
    private Color _idleColor = Color.white;

    [SerializeField]
    private Color _beepColor = Color.red;

    [Header("Audio")]
    [SerializeField]    
    private AudioClip _beepClip;

    [SerializeField]    
    private AudioClip _preExplosionClip;

    [SerializeField]    
    private GameObject _explosionEffect;

    [SerializeField]    
    private AudioSource _beepAudioSource;

    [SerializeField]
    private AudioSource _preExplosionAudioSource;

    [SerializeField]
    private AnimationCurve _audioLimiterCurve;

    private bool _isGoingToExplode = false;
    private SphereCollider _triggerCollider;

    public float LimitedVolume01
    {
        get
        {
            return _audioLimiterCurve.Evaluate(1.0f - GameManager.Instance.TimeLeft01);
        }
    }

    private void Start()
    {
        _triggerCollider = GetComponent<SphereCollider>();
        if (_immediatelyTriggered)
        {
            StartExplosionSequence();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StartExplosionSequence();
    }

    private void StartExplosionSequence()
    {
        if (_isGoingToExplode) return;
        _isGoingToExplode = true;

        StartCoroutine(ExplodeSequence());
    }

    private IEnumerator ExplodeSequence()
    {
        _beepAudioSource.loop = true;
        _beepAudioSource.clip = _beepClip;
        _beepAudioSource.volume = _beepAudioSource.volume * LimitedVolume01;
        _beepAudioSource.Play();

        StartCoroutine(PreExplosionSound());

        float currentTime = 0f;
        while (currentTime <= _explodeDuration)
        {
            currentTime += Time.deltaTime;
            _beeperMeshRenderer.material.color = Color.Lerp(_beepColor, _idleColor, _beepAudioSource.time / _beepAudioSource.clip.length);
            _beepAudioSource.pitch = Mathf.Lerp(_minPitch, _maxPitch, _beepCurve.Evaluate(currentTime / _explodeDuration));
            yield return new WaitForEndOfFrame();
        }

        Explode();
    }

    private IEnumerator PreExplosionSound()
    {
        if (_preExplosionClip == null || _explodeDuration - _preExplosionClip.length < 0)
            yield break;

        yield return new WaitForSeconds(_explodeDuration - _preExplosionClip.length);
        _preExplosionAudioSource.volume = _preExplosionAudioSource.volume * LimitedVolume01;
        _preExplosionAudioSource.PlayOneShot(_preExplosionClip);
    }

    private void Explode()
    {
        GameObject explosion = Instantiate(_explosionEffect, transform.position, Quaternion.identity, ExplosionManager.Instance.transform);
        AudioSource explosionSound = explosion.GetComponent<AudioSource>();
        explosionSound.volume = explosionSound.volume * LimitedVolume01;

        if (GameManager.Instance.IsPaused)
        {
            Destroy(this.gameObject);
            return;
        }

        Collider[] movablesInRange = Physics.OverlapSphere(transform.position, _triggerCollider.radius, _onlyMovable);
        SetAllLayers(2);

        foreach (Collider movable in movablesInRange)
        {
            if (movable.GetComponentInParent<Rigidbody>() != null)
            {
                var diffVector = (movable.transform.position - this.transform.position).normalized;
                diffVector.y = 1;
                movable.GetComponentInParent<Rigidbody>().AddForceAtPosition(diffVector * _explosionStrength, transform.position, ForceMode.Impulse);
            }

            if (movable.GetComponentInParent<Player>())
            {
                GameManager.Instance.GameOver();
            }
        }

        Destroy(this.gameObject);
    }

    private void SetAllLayers(int layer)
    {
        MoveToLayer(this.transform, layer);
    }

    private void MoveToLayer(Transform root, int layer)
    {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
            MoveToLayer(child, layer);
    }
}
