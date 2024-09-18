using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TreeChopManager : MonoBehaviour
{
    public GameObject[] trees;

    public GameObject[] dialogueSwitch;

    public GameObject planks;

    public GameObject arrow;
    public void ActivateTrees()
    {
        foreach (var item in trees)
        {
            item.tag = "InteractiveObject";
        }
        planks.tag = "InteractiveObject";
    }

    public void CheckIfAllChopped()
    {
        if (trees.All(obj => obj.GetComponent<TreeChop>().chopped))
        {
            AllChopped();
        }
        else
        {
            return;
        }
    }

    void AllChopped()
    {
        dialogueSwitch[0].SetActive(false);
        dialogueSwitch[1].SetActive(true);

        arrow.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
