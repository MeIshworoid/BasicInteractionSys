using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IUseable
{
    void Use(GameObject actor);
    UnityEvent OnUse { get; }
}
