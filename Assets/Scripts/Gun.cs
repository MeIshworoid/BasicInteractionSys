using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private ParticleSystem _impactParticle;
    [SerializeField] private TrailRenderer _bulletTrail;
    [SerializeField] private float _shootDelay = 0.5f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _shootTriggerName = "Shoot";

    private float _lastShootTime;

    public void Shoot()
    {
        if (_lastShootTime + _shootDelay < Time.time)
        {
            if (Physics.Raycast(_bulletSpawnPoint.position, _bulletSpawnPoint.forward, out RaycastHit hit, float.MaxValue, _layerMask))
            {
                TrailRenderer trail = Instantiate(_bulletTrail, _bulletSpawnPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));

                // Trigger the shoot animation
                if (_animator != null)
                {
                    _animator.SetTrigger(_shootTriggerName);
                }

                _lastShootTime = Time.time;
            }
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        trail.transform.position = hit.point;
        Instantiate(_impactParticle, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(trail.gameObject, trail.time);
    }
}
