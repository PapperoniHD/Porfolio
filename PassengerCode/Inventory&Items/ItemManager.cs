using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] items;
    public Transform[] spawns;

    public Transform Key;
    
    void Start()
    {

        /*for (int i = 0; i < items.Length; i++)
        {
            for (int j = 0; j < spawns.Length; j++)
            {
                items[i].transform.position = spawns[j].position;
                
            }
        }*/
        ItemWorld.SpawnItemWorld(Key, new Item { itemType = Item.ItemType.Key, amount = 1 });
    }

}
