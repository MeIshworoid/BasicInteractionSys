using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Food : MonoBehaviour, IUseable
{
    [field: SerializeField]
    public UnityEvent OnUse { get; private set; }

    private int healthBoost = 1;
    public void Use(GameObject actor)
    {
        OnUse?.Invoke();
        actor.GetComponent<Player>().AddHealth(healthBoost);
        Destroy(gameObject);
    }
}
