using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        Key
    }

    public ItemType itemType;
    public int amount;


    public GameObject GetGameobject()
    {
        switch (itemType)
        {
            default:
            case ItemType.Key: return ItemAssets.Instance.KeyPrefab;
            
        }
    }
}

