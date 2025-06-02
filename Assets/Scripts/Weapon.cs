using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour, IUseable
{
    [field: SerializeField]
    public UnityEvent OnUse { get; private set; }
    [SerializeField] private Gun _gun;

    public void Use(GameObject actor)
    {
        if (_gun != null)
        {
            _gun.Shoot();
        }
        OnUse?.Invoke();
    }
}
