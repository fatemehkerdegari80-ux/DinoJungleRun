
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerInteract(other.gameObject);
        }
    }

    protected abstract void OnPlayerInteract(GameObject player);
}
