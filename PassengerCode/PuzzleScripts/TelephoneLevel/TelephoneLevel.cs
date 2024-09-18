using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelephoneLevel : MonoBehaviour
{
    private GameObject telephone;
    public GameObject[] objectsToStart;
    // Start is called before the first frame update
    void Start()
    {
        
        telephone = transform.GetChild(0).gameObject;
        telephone.SetActive(false);
        StartCoroutine(TelephoneActivate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TelephoneActivate()
    {
        GameManager.GM.Cutscene();
        yield return new WaitForSeconds(5);
        foreach (var item in objectsToStart)
        {
            item.SetActive(true);
        }
        telephone.SetActive(true);
        GameManager.GM.Gameplay();
    }
}
