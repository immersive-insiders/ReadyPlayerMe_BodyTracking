using System;
using UnityEngine;

public class CloseToUICollision : MonoBehaviour
{
    public static event Action<GameObject> OnEnterUIArea = delegate { };
    public static event Action<GameObject> OnExitUIArea = delegate { };

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("InteractiveUI"))
            OnEnterUIArea?.Invoke(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("InteractiveUI"))
            OnExitUIArea?.Invoke(gameObject);
    }
}
