using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrashManager : MonoBehaviour
{
    public GameObject[] trash;

    public GameObject[] dialogueSwitch;

    public void ActivateTrash()
    {
        foreach (var item in trash)
        {
            item.tag = "InteractiveObject";
        }
    }

    public void CheckIfAllCleaned()
    {
        if (trash.All(obj => obj.GetComponent<Trash>().cleaned))
        {
            AllCleaned();
        }
        else
        {
            return;
        }
    }

    void AllCleaned()
    {
        dialogueSwitch[0].SetActive(false);
        dialogueSwitch[1].SetActive(true);
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
