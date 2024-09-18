using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class SpiritPickup : MonoBehaviour
{
    private Interactable interactScript;
    public Transform handTransform;
    private DialogueScript dialogueScript;
    public DialogueText spiritDialogue;

    public CinemachineVirtualCamera vcam;
    public float vcamAmpFreq = 0.2f;
    public bool lerpCamShake = false;

    private DuckStart duck;

    // Start is called before the first frame update
    void Start()
    {
        interactScript = FindObjectOfType<Interactable>();
        lerpCamShake = true;
        dialogueScript = FindObjectOfType<DialogueScript>();
        duck = FindObjectOfType<DuckStart>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vcam != null && vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>() != null)
        {
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = vcamAmpFreq;
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = vcamAmpFreq;

            if (lerpCamShake)
            {
                vcamAmpFreq = Mathf.Lerp(vcamAmpFreq, 2, Time.deltaTime * 0.5f);
            }
            else
            {
                //vcamAmpFreq = Mathf.Lerp(vcamAmpFreq, 0, Time.deltaTime * 0.5f);
                vcamAmpFreq = 0;
            }
        }
    }

    public void SetPlayerOnHand()
    {
        GameManager.GM.Hiding();
        interactScript.sitObj = handTransform;
        lerpCamShake = false;
    }

    public void StopShake()
    {
        lerpCamShake = false;
    }

    public void StartDuck()
    {
        duck.StartDucks();
    }

    public void StartShake() 
    {
        lerpCamShake = true;
    }

    public void StartDialogue()
    {
        dialogueScript.dialogueObj = spiritDialogue.gameObject;
        dialogueScript.SetLines(spiritDialogue.firstLines);
        dialogueScript.SetLookAt(spiritDialogue.transform);
        dialogueScript.voice = 10;
        StartCoroutine(dialogueScript.StartDialogue());
    }
}
