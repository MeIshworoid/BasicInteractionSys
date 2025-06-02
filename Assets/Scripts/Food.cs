using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Food : MonoBehaviour, IUseable
{
    [field: SerializeField]
    public UnityEvent OnUse { get; private set; }

    private int _healthBoost = 1;
    public void Use(GameObject actor)
    {
        OnUse?.Invoke();
        actor.GetComponent<Player>().AddHealth(_healthBoost);
        Destroy(gameObject);
    }
}
