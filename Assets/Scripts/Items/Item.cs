using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField] private string itemName;

    public abstract void Use(GameObject consumer);
}
