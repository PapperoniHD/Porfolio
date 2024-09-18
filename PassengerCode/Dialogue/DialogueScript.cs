using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Febucci.UI;
using Febucci;
using Febucci.Attributes;
using Cinemachine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour
{
    public string playerName;
    public bool disableInput = false;
    public bool dialogueStarted = false;
    public bool textFinished = false;
    Animator anim;
    public GameObject selector;

    public int voice = 0;

    private string[] lines;
    public int index;

    public TextMeshProUGUI textComponent;
    public TextAnimatorPlayer textAnimator;

    [SerializeField]
    private SpriteRenderer arrow;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Interactable playerScript;
    [SerializeField]
    private Image[] window;
    //[SerializeField]
    //private GameObject[] yesandno;
    public Transform lookTarget;
    private Transform oldLookTarget;

    private bool destroy;

    public CinemachineVirtualCamera Cam;
    public Transform CamHolder;
    bool zoom;
    float zoomFOV = 30;
    float defaultFOV = 65;
    float zoomSpeed = 2.5f;
    static float t = 0.0f;
    public float typewriterSpeed = 0.5f;

    public GameObject dialogueObj;

    private void Awake()
    {

    }
    void Start()
    {
       /* foreach (var item in yesandno)
        {
            item.SetActive(false);
        }*/
        Initalize();
        DisableWindow();
        UpdateTextSpeed(typewriterSpeed);
        selector.SetActive(false);
        destroy = false;
    }

    public void UpdateTextSpeed(float speed)
    {
        textAnimator.SetTypewriterSpeed(speed);
    }
    
    void Update()
    {
        if (dialogueStarted)
        {
            if (Input.GetButtonDown("Interact") && !disableInput)
            {
                NextLine();
            }
            if (Cam != null)
            {
                Zoom();
            }            
            if (lookTarget != null)
            {
                LookAt();
            }
            
        }
    }
    
    void Initalize()
    {
        lines = null;
        anim = GetComponent<Animator>();
        textComponent.text = string.Empty;
        arrow.enabled = false;
        for (int i = 0; i < window.Length; i++)
        {
            window[i].enabled = false;
        }
        playerName = PlayerPrefs.GetString("PlayerName", "NoName");
    }

    public void SetLines(string[] dialogue)
    {
        lines = dialogue;
    }

    public void SetLookAt(Transform lookAt)
    {
        Cam.LookAt = lookAt.transform.GetChild(0);
    }

    public IEnumerator StartDialogue()
    {
        //oldLookTarget = new GameObject("oldLook").transform;
        //oldLookTarget.transform.position = Cam.transform.position + transform.forward * 3;

        disableInput = true;
        if (!dialogueStarted)
        {
            anim.Play("PopUp");
        }
        zoom = true;
        EnableWindow();
        dialogueStarted = true;
        GameManager.GM.Dialogue();   
        index = 0;
        yield return new WaitForSeconds(0.5f);
        disableInput = false;
        textComponent.text = lines[index];
        textAnimator.StartShowingText();

    }

    public void Zoom()
    {        
        if (zoom)
        {
            Cam.m_Lens.FieldOfView = Mathf.Lerp(defaultFOV, zoomFOV, t);
            t += zoomSpeed * Time.deltaTime;

            if (t >= 1)
            {
                t = 1;
            }
        }
        else
        {
            Cam.m_Lens.FieldOfView = Mathf.Lerp(defaultFOV, zoomFOV, t);
            //vcam.m_Lens.FieldOfView = 65;
            t -= zoomSpeed * Time.deltaTime;

            if (t <= 0)
            {
                t = 0;
            }

        }
    }

    public void LookAt()
    {
        Vector3 targetDirection = lookTarget.transform.position - CamHolder.transform.position;
        Quaternion lookRot = Quaternion.LookRotation(targetDirection, Vector3.up);
        float eulerY = lookRot.eulerAngles.y;
        float eulerX = lookRot.eulerAngles.x;
        Quaternion rotationY = Quaternion.Euler(0, eulerY, 0);
        Quaternion rotationX = Quaternion.Euler(eulerX, 0, 0);

        CamHolder.localRotation = Quaternion.Slerp(CamHolder.localRotation, rotationX, Time.deltaTime * 2);
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, rotationY, Time.deltaTime*2);
    }


    public void NextLine()
    {
        var dialogueText = dialogueObj.GetComponent<DialogueText>();
        var dialogueLook = dialogueObj.GetComponent<DialogueLook>();
        if (index < lines.Length - 1 && textFinished)
        {
            index++;
            textComponent.text = lines[index];
            textAnimator.StartShowingText();
            //isableInput = true;
            if (dialogueText != null)
            {
                if (dialogueText.hasQuestion && index == dialogueText.indexForQuestion)
                {
                    if (lines == dialogueText.secondLines || lines == dialogueText.firstLines)
                    {
                        disableInput = true;
                        anim.Play("Question");
                        StartCoroutine(ActivateButtons());
                    }

                }
            }
            if (dialogueLook != null && lines == dialogueObj.GetComponent<DialogueText>().firstLines)
            {
                dialogueLook.NextIndex();
            }
            
        }
        /*else if(!textFinished)
        {
            textAnimator.SkipTypewriter();
        }*/
        else if (index >= lines.Length - 1 && textFinished)
        {
            StartCoroutine(EndDialogue());
        }
    }

    public void Yes()
    {
        textComponent.text = string.Empty;
        var dialogueText = dialogueObj.GetComponent<DialogueText>();

        if (playerScript.amountOfMoney > 0 && dialogueText.optionalLines3 != null)
        {
            playerScript.amountOfKeys++;
            dialogueText.hasQuestion = false;
            lines = dialogueText.optionalLines3;
            playerScript.amountOfMoney--;
            destroy = true;
        }
        else if(dialogueText.optionalLines1 != null)
        {
            lines = dialogueText.optionalLines1;

        }
        selector.SetActive(false);
        if (dialogueText.optionalLines3[0] != null && dialogueText.optionalLines1[0] != null)
        {
            StartCoroutine(StartAfterQuestion());
            print("here?");
        }
        IDialogueEvent iEvent = dialogueObj.GetComponent<IDialogueEvent>();
        if (iEvent != null)
        {
            iEvent.Event();
        }
    }
    public void No()
    {
        textComponent.text = string.Empty;
        var dialogueText = dialogueObj.GetComponent<DialogueText>();
        lines = dialogueText.optionalLines2;
        StartCoroutine(StartAfterQuestion());
        selector.SetActive(false);
        INoEvent iEvent = dialogueObj.GetComponent<INoEvent>();
        if (iEvent != null)
        {
            iEvent.Event();
        }
    }

    IEnumerator StartAfterQuestion()
    {
        anim.Play("Answer");
        yield return new WaitForSeconds(2);
        StartCoroutine(StartDialogue());
        disableInput = false;
    }

    IEnumerator ActivateButtons()
    {
        yield return new WaitForSeconds(2);
        selector.SetActive(true);
    }

    public IEnumerator EndDialogue()
    {
        //Cam.LookAt = oldLookTarget;
        zoom = false;
        textComponent.text = string.Empty;
        //yield return new WaitForSeconds(0.2f);
        anim.Play("PopDown");
        yield return new WaitForSeconds(1f);
        //Destroy(oldLookTarget.gameObject);
        dialogueStarted = false;
        if (GameManager.GM.State != GameState.Battleship)
        {
            GameManager.GM.Gameplay();
        }      
        if (Cam.LookAt != null)
        {
            Cam.LookAt = null;
        }
        if (lines != dialogueObj.GetComponent<DialogueText>().optionalLines1 && !dialogueObj.GetComponent<DialogueText>().hasQuestion && lines != dialogueObj.GetComponent<DialogueText>().optionalLines3)
        {
            dialogueObj.GetComponent<DialogueText>().SetSecondLines();
        }
        else if(dialogueObj.GetComponent<DialogueText>().hasQuestion)
        {
            dialogueObj.GetComponent<DialogueText>().SetSecondLines();
        }
        if (destroy)
        {
            destroy = false;
            Destroy(dialogueObj);
        }
        index = 0;
        disableInput = true;

        var dialogueLook = dialogueObj.GetComponent<DialogueLook>();
        IDialogueEndEvent dialogueEndEvent = dialogueObj.GetComponent<IDialogueEndEvent>();
        IDialogueEndEvent2 dialogueEndEvent2 = dialogueObj.GetComponent<IDialogueEndEvent2>();
        if (dialogueEndEvent != null)
        {
            dialogueEndEvent.Event();
        }
        if (dialogueEndEvent2 != null)
        {
            dialogueEndEvent2.Event();
        }
        if (dialogueLook != null)
        {
            dialogueLook.index = 0;
        }
        
        
    }

    public void EnableWindow()
    {
        for (int i = 0; i < window.Length; i++)
        {
            window[i].enabled = true;
        }
    }
    public void DisableWindow()
    {
        for (int i = 0; i < window.Length; i++)
        {
            window[i].enabled = false;
        }
    }

    public void TextFinished()
    {
        arrow.enabled = true;
        textFinished = true;
    }

    public void TextWriting()
    {
        arrow.enabled = false;
        textFinished = false;
    }

}
