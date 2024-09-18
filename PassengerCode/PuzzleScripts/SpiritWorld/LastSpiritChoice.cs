using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LastSpiritChoice : MonoBehaviour, IDialogueEndEvent2
{
    public bool goBack;
    public Animator transitionAnim;
    public SkinnedMeshRenderer mesh;
    public Animator trainAnim;
    public GameObject arrow;
    public GameObject chair;
    public void Event()
    {
        if (goBack)
        {
            //mesh.enabled = false;
            arrow.SetActive(true);
            trainAnim.Play("SpiritWorldArrive2");
            chair.SetActive(true);
        }
        else
        {
            StartCoroutine(EndGame());
            
        }
        PlayerPrefs.SetInt("LastLevelChoice", 1);
    }

    IEnumerator EndGame()
    {
        transitionAnim.Play("TransitionClose");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Credits");
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
