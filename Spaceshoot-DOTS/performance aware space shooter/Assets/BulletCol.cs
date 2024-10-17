using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCol : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            print("coll");
        }
    }
}
