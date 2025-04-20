using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Book", menuName = "Item/Book")]
public class Book : Item
{
    public Action OnPickup;
    public override void Use(GameObject consumer)
    {
        OnPickup?.Invoke();
    }
}
