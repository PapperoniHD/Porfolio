using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{

    public static ItemWorld SpawnItemWorld(Transform pos, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, pos.position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }
    private Item item;

    public void SetItem(Item item)
    {
        this.item = item;
    }
}
