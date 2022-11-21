using System;
using UnityEngine;

public class Stone : MonoBehaviour, IPickable
{
    public static event Action OnStonePickedUp;

    public void PickUp()
    {
        Destroy(gameObject);
        OnStonePickedUp?.Invoke();
    }
}
