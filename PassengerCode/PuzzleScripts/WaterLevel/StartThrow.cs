using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartThrow : MonoBehaviour, IDialogueEndEvent
{
    public GameObject bodyThrow;
    public GameObject player;
    public SkinnedMeshRenderer bodySkin;
    public Transform playerPos;
    public void Event()
    {
        StartCoroutine(Throw());
    }

    IEnumerator Throw()
    {
        bodyThrow.SetActive(true);
        bodySkin.enabled = false;
        yield return new WaitForSeconds(2);
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = playerPos.position;
        player.transform.rotation = playerPos.rotation;
        yield return new WaitForSeconds(7);
        bodyThrow.SetActive(false);
        yield return new WaitForSeconds(1);
        bodySkin.enabled = true;
        player.GetComponent<CharacterController>().enabled = true;

    }

}
