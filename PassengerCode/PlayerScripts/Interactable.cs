using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Febucci.UI;

public class Interactable : MonoBehaviour
{
    [Header("Player Body")]
    public Transform body;

    [Header("Use UI")]
    [SerializeField] private int timeToShowUI = 2;
    [SerializeField] private Image showUseUI;

    [Header("Pause Timer")]
    [SerializeField] private int waitTimer = 2;
    [SerializeField] private bool pauseInteraction = false;
    [SerializeField] private bool pauseLockedDoorInteraction = false;


    [Header("Raycast Parameters")]
    [SerializeField] private int rayLength = 3;
    RaycastHit hit;

    //private DoorController raycasted_obj;

    [Header("Key Codes")]
    [SerializeField] private KeyCode useKey = KeyCode.E;

    [Header("UI Parameters")]
    [SerializeField] private Image cursor;


    [Header("Cursor Images")]
    [SerializeField] private Sprite cursorDefault;
    [SerializeField] private Sprite cursorHold;
    [SerializeField] private Sprite chatBubble;


    [Header("Text Images")]
    [SerializeField] private Sprite brokenText;
    [SerializeField] private Sprite lockedText;
    [SerializeField] private Sprite keyAcquiredText;
    [SerializeField] private Sprite wontBudgeText;
    [SerializeField] private GameObject diaryUI;
    [SerializeField] private TextMeshProUGUI infoText;
    private TMP_Text entry = null;

    [Header("Inventory")]
    public int amountOfKeys = 0;
    public int amountOfMoney = 0;


    [Header("GameObjects")]
    [SerializeField] private Transform player;
    [SerializeField] private Volume globalVolume;
    DepthOfField dof;
    bool uiActive;
    private Inventory inventory;
    private float dofValue = 10;
    private float readDelay = 0f;
    [SerializeField]
    private CinemachineVirtualCamera vcam;
    public bool unlockComb = false;

    [Header("Dialogue")]
    [SerializeField] private DialogueScript dialogue;

    [Header("Hold Objects Variables")]
    [SerializeField] float LerpSpeed = 30f;
    [SerializeField] Transform holdPos;
    [SerializeField] Camera cam;

    Rigidbody grabbedRB;
    bool outBound = false;
    private string[] interactableTags = new string[] { "BrokenObject", "Door", "Pickup", "Key", "Read", "Hide", "Ladder", "CombLock", "Dialogue", "Money", "Coffee", "InteractiveObject"};


    public Transform sitObj;

    //Camera Zoom Var
    private CinemachineBrain cmBrain;
    float zoomFOV = 40;
    float defaultFOV = 65;
    float zoomSpeed = 7.5f;
    static float t = 0.0f;

    public OutlineInteract[] allOutlines;

    void Start()
    {
        infoText.text = " ";
        cursor.enabled = false;
        showUseUI.enabled = false;
        uiActive = false;
        inventory = new Inventory();

        dofValue = 10;

    }
    void Update()
    {
        if (GameManager.GM.State != GameState.Cutscene)
        {
            ShowCursor();
            //StopHiding();
        }
        else
        {

        }

        if (GameManager.GM.State == GameState.Reading)
        {
            DiaryUI();
        }
        else
        {
            diaryUI.SetActive(false);
            dofValue = 10;
        }

        dofValueHandler();
        if (GameManager.GM.State == GameState.Gameplay)
        {
            GrabObject();
        }
        if (GameManager.GM.State == GameState.Gameplay || GameManager.GM.State == GameState.Hiding)
        {
            Zoom();
        }
           
    }

    void FixedUpdate()
    {
        
    }

    private void LateUpdate()
    {
        if (GameManager.GM.State == GameState.Hiding)
        {
            if (sitObj != null)
            {
                body.SetPositionAndRotation(sitObj.transform.position, sitObj.transform.rotation);
            }
        }
    }

    void ShowCursor()
    {
          
          Vector3 fwd = transform.TransformDirection(Vector3.forward);

          if (Physics.Raycast(transform.position, fwd, out hit, rayLength))
          {
              Debug.DrawRay(transform.position, fwd * rayLength, Color.green);
               if (interactableTags.Contains(hit.collider.tag) && !pauseLockedDoorInteraction && GameManager.GM.State == GameState.Gameplay && !pauseInteraction)
               {
                  var tag = hit.collider.tag;
                  var obj = hit.collider.gameObject;

                  Interact(tag, obj);
                  cursor.enabled = true;

                 InteractDescriptor descriptor = obj.GetComponent<InteractDescriptor>();
                if (descriptor != null)
                {
                    infoText.text = descriptor.interactDescriptor;
                }
                else
                {
                    infoText.text = " ";
                }
                OutlineInteract outline = hit.transform.GetComponent<OutlineInteract>();
                if (outline != null)
                {
                    if (outline.outlineOnHover) outline.EnableOutline();
                }
                


                if (!showUseUI.enabled)
                {
                    if (tag == "Dialogue")
                    {
                        infoText.text = " ";
                        cursor.sprite = chatBubble;
                    }
                    else if(!grabbedRB)
                    {
                        cursor.sprite = cursorDefault;
                    }
                }
                else
                {
                    infoText.text = " ";
                }
                  
               }
               else
               {
                  cursor.enabled = false;
                  infoText.text = " ";
                if (allOutlines != null)
                {
                    foreach (var item in allOutlines)
                    {
                        if (item.outlineOnHover)
                        {
                            item.DisableOutline();
                        }
                    }
                }
                  
               }
            
          }
          else
          {
              cursor.enabled = false;
                infoText.text = " ";
            if (allOutlines != null)
            {
                foreach (var item in allOutlines)
                {
                    if (item.outlineOnHover)
                    {
                        item.DisableOutline();
                    }
                }
            }
            
          } 
    }
    void Interact(string tag, GameObject obj)
    {
        if (!Input.GetButtonDown("Interact"))
        {
            return;
        }
        //Interface events
        IScaryEvent iEvent = hit.transform.GetComponent<IScaryEvent>();
        if (iEvent != null)
        {
            iEvent.Event();
        }
        IGeneral iGeneral = hit.transform.GetComponent<IGeneral>();
        if (iGeneral != null)
        {
            iGeneral.Event();
        }
        OutlineInteract outline = hit.transform.GetComponent<OutlineInteract>();
        if (outline != null)
        {
            outline.DisableOutline();
        }

        switch (tag)
        {
            case "Door":
            {
                    LockedDoor doorScript = obj.GetComponent<LockedDoor>();
                    if (doorScript != null )
                    {
                        if (!doorScript.wontbudge)
                        {
                            
                                if (!doorScript.locked)
                                {
                                    OpenDoor(obj);
                                }
                                else
                                {
                                    if (amountOfKeys > 0)
                                    {
                                        amountOfKeys--;
                                        doorScript.locked = false;
                                        StartCoroutine(OpenLockedDoor(obj));
                                    }
                                    else
                                    {
                                        showUseUI.sprite = lockedText;

                                        StartCoroutine(ShowUseUI());
                                        StartCoroutine(PauseInteraction());
                                    }

                                }
                        }
                        else
                        {
                            showUseUI.sprite = wontBudgeText;

                            StartCoroutine(ShowUseUI());
                            StartCoroutine(PauseInteraction());
                        }
                    }
                    else
                    {
                        OpenDoor(obj);
                    }
                    
                    
                
                break;
            }
            case "BrokenObject":
            {
                showUseUI.sprite = brokenText;
                if (!pauseInteraction)
                {
                   StartCoroutine(ShowUseUI());
                   StartCoroutine(PauseInteraction());
                }

                break;
            }
            case "Hide":
            {
                    infoText.text = "sit";
                    StartCoroutine(HideDelay(obj.transform));
                    sitObj = obj.transform;
                break;
            }
            case "Key":
            {
                showUseUI.sprite = keyAcquiredText;
                StopCoroutine(ShowUseUI());
                StartCoroutine(ShowUseUI());
                amountOfKeys++;
                Destroy(obj);
                break;
            }
            case "Read":
            {
                
                StartCoroutine(ReadPaper(obj));
                break;
            }
            case "Ladder":
            {
                StartCoroutine(Ladder(obj));
                break;
            }
            case "CombLock":
            {
                StartCoroutine(CombLock(obj));
                break;
            }
            case "Dialogue":
            {
                    infoText.text = "talk";
                    dialogue.voice = obj.GetComponent<DialogueText>().voice;
                    dialogue.SetLines(obj.GetComponent<DialogueText>().lines);
                 dialogue.StartCoroutine(dialogue.StartDialogue());
                    //dialogue.SetLookAt(obj.transform);
                 dialogue.lookTarget = obj.transform.GetChild(0);
                 dialogue.dialogueObj = obj;
                 break;
            }
            case "Money":
            {
                 infoText.text = "tip jar";
                 break;
            }
        }

    }

    void Zoom()
    {

        if (Input.GetKey(KeyCode.Mouse1))
        {
            vcam.m_Lens.FieldOfView = Mathf.Lerp(defaultFOV, zoomFOV, t);
            t += zoomSpeed * Time.deltaTime;

            if (t >= 1)
            {
                t = 1;
            }
        }
        else
        {
            vcam.m_Lens.FieldOfView = Mathf.Lerp(defaultFOV, zoomFOV, t);
            //vcam.m_Lens.FieldOfView = 65;
            t -= zoomSpeed * Time.deltaTime;

            if (t <= 0)
            {
                t = 0;
            }
            
        }
        
    }

    public void ResestGrab(Rigidbody rb)
    {
        StartCoroutine(ResetLayer(rb));
        grabbedRB = null;     
    }

    IEnumerator ResetLayer(Rigidbody rb)
    {
        yield return new WaitForSeconds(0.5f);
        rb.gameObject.layer = LayerMask.NameToLayer("Default");
    }


    void GrabObject()
    {

        if (grabbedRB)
        {
            var cablePickup = grabbedRB.GetComponent<CablePickupOBJ>();
            if (cablePickup != null)
            {
                cablePickup.holding = true;
                cablePickup.attached = false;
            }
            var paintingPickup = grabbedRB.GetComponent<MuseumPickupOBJ>();
            if (paintingPickup != null)
            {
                if (paintingPickup.cableScript != null)
                {
                    paintingPickup.cableScript.GetComponent<BoxCollider>().enabled = true;
                    paintingPickup.cableScript = null;
                }
                paintingPickup.ShowPlaceLocations();
                paintingPickup.attached = false;
            }
            grabbedRB.MovePosition(Vector3.Lerp(grabbedRB.transform.position, holdPos.position, LerpSpeed));//Time.deltaTime * LerpSpeed));

            cursor.sprite = cursorHold;
            cursor.enabled = true;
        }
        else
        {
            //cursor.sprite = cursorDefault;
        }

        if (Input.GetButton("Interact"))
        {
            
            RaycastHit hit;
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            if (Physics.Raycast(ray, out hit, rayLength))
            {
                if (hit.collider.CompareTag("Pickup") && grabbedRB == null)
                {

                    grabbedRB = hit.collider.gameObject.GetComponent<Rigidbody>();
                    if (grabbedRB)
                    {
                        Physics.IgnoreCollision(grabbedRB.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
                        grabbedRB.isKinematic = true;
                        grabbedRB.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                        //grabbedRB.freezeRotation = true;
                        //grabbedRB.useGravity = false;
                    }
                    else
                    {
                        //grabbedRB.isKinematic = false;
                    }                                  
                }
                
            } 
        }
        else
        {
            //cursor.sprite = cursorDefault;
            if (grabbedRB != null)
            {
                var cablePickup = grabbedRB.GetComponent<CablePickupOBJ>();
                if (cablePickup != null)
                {
                    cablePickup.holding = false;
                }
                var paintingPickup = grabbedRB.GetComponent<MuseumPickupOBJ>();
                if (paintingPickup != null)
                {
                    paintingPickup.HidePlaceLocations();
                }
                float distance = Vector3.Distance(grabbedRB.transform.position, player.transform.position);
                Physics.IgnoreCollision(grabbedRB.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
                //grabbedRB.freezeRotation = false;
                //grabbedRB.useGravity = true;
                grabbedRB.isKinematic = false;
                grabbedRB.gameObject.layer = LayerMask.NameToLayer("Ground");
                grabbedRB = null;
            }
            
        }
        
    }
    public void StopHiding(Transform standPos)
    {
        if (GameManager.GM.State == GameState.Hiding)
        {
            
            var obj = body.gameObject;
            StartCoroutine(HideDelay(standPos));

            
        }
    }

    void OpenDoor(GameObject obj)
    {
        obj.GetComponent<LockedDoor>().DoorInteract();
        /*
        var door = obj.GetComponent<Animator>();
        if (door.GetCurrentAnimatorStateInfo(0).IsName("OpenDoor") || door.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            if (door.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                door.Play("CloseDoor");
                FMODUnity.RuntimeManager.PlayOneShot("event:/door", obj.transform.position);
            }
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/door", obj.transform.position);
            door.Play("OpenDoor");
        }*/
    }

    void dofValueHandler()
    {
        if (globalVolume.profile.TryGet<DepthOfField>(out dof))
        {
            dof.focusDistance.value = dofValue;
        }
    }
    
    void DiaryUI()
    {
        
        
        
       /* if (Input.GetKeyDown(KeyCode.Space) && !uiActive)
        {
            diaryUI.SetActive(true);
            //string entry = obj.GetComponent<Diary>().diaryText;
            
            TMP_Text text = diaryUI.GetComponentInChildren<TMP_Text>();

            if (entry.text != null)
            {
                text.text = entry.text;
            }
            
            
            dofValue = 0.1f;
            uiActive = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && uiActive)
        {
            diaryUI.SetActive(false);
            uiActive = false;
            dofValue = 10f;
        }*/
    }

    IEnumerator ReadPaper(GameObject obj)
    {
        cmBrain = FindObjectOfType<CinemachineBrain>();
        cmBrain.m_DefaultBlend.m_Time = 2;

        GameManager.GM.Reading();
        float distance = (player.position - obj.transform.position).magnitude;
        CinemachineVirtualCamera readCam = obj.GetComponentInChildren<CinemachineVirtualCamera>();
        entry = obj.GetComponentInChildren<TMP_Text>();

        readCam.Priority = 10;

        yield return new WaitForSeconds(2);

        diaryUI.SetActive(true);
        //string entry = obj.GetComponent<Diary>().diaryText;

        TMP_Text text = diaryUI.GetComponentInChildren<TMP_Text>();

        if (entry.text != null)
        {
            text.text = entry.text;
        }


        dofValue = 0.1f;
        uiActive = true;


        while (!Input.GetButtonDown("Interact"))
        {
            
            yield return null;

        }
        

        diaryUI.SetActive(false);
        uiActive = false;
        dofValue = 10f;

        readCam.Priority = 0;
        yield return new WaitForSeconds(2);
        GameManager.GM.Gameplay();

    }

    IEnumerator CombLock(GameObject obj)
    {
        

        cmBrain = FindObjectOfType<CinemachineBrain>();
        cmBrain.m_DefaultBlend.m_Time = 2;

        GameManager.GM.Reading();
        float distance = (player.position - obj.transform.position).magnitude;
        CinemachineVirtualCamera readCam = obj.GetComponentInChildren<CinemachineVirtualCamera>();
        Canvas canvas = obj.GetComponentInChildren<Canvas>();
        entry = obj.GetComponentInChildren<TMP_Text>();

        readCam.Priority = 10;

        yield return new WaitForSeconds(2);
        canvas.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        dofValue = 0.1f;
        uiActive = true;
        

        while (unlockComb == false || !Input.GetButtonDown("Interact"))
        {
            yield return null;

        }
        

        uiActive = false;
        dofValue = 10f;

        readCam.Priority = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canvas.enabled = false;
        yield return new WaitForSeconds(2);
        obj.SetActive(false);
        GameManager.GM.Gameplay();

    }

    IEnumerator ShowUseUI()
    {
        showUseUI.enabled = true;
        yield return new WaitForSeconds(timeToShowUI);
        showUseUI.enabled = false;
        
    }

    public IEnumerator PauseInteraction()
    {
        pauseInteraction = true;
        yield return new WaitForSeconds(waitTimer);
        pauseInteraction = false;

    }

    IEnumerator Ladder (GameObject obj)
    {
        if (amountOfKeys > 0)
        {
            Transform ladderPos;
            if (obj.transform.position.z < player.position.z)
            {
                ladderPos = obj.transform.GetChild(0);
            }
            else
            {
                ladderPos = obj.transform.GetChild(1);
            }
            

            var cc = player.GetComponent<CharacterController>();

            cc.enabled = false;
            GameManager.GM.Transition();
            GameManager.GM.Cutscene();

            yield return new WaitForSeconds(1f);
            body.position = ladderPos.position;
            body.rotation = ladderPos.rotation;


            GameManager.GM.Gameplay();
            cc.enabled = true;
            
        }
        else
        {
            showUseUI.sprite = lockedText;

            StartCoroutine(ShowUseUI());
            StartCoroutine(PauseInteraction());
            
        }
        
    }


    IEnumerator HideDelay(Transform obj)
    {
        
        GameManager.GM.Transition();
  
        if (GameManager.GM.State != GameState.Hiding)
        {
            GameManager.GM.Cutscene();
            yield return new WaitForSeconds(1f);
            GameManager.GM.Hiding();
            body.SetPositionAndRotation(obj.transform.position, obj.transform.rotation);
            body.GetComponentInChildren<Animator>().enabled = false;
        }
        else
        {
            GameManager.GM.Cutscene();
            yield return new WaitForSeconds(1f);
            body.SetPositionAndRotation(sitObj.transform.GetChild(0).position, sitObj.transform.GetChild(0).rotation);
            yield return new WaitForSeconds(0.5f);
            GameManager.GM.Gameplay();
            body.GetComponentInChildren<Animator>().enabled = true;
        }

        
    }

    

    IEnumerator OpenLockedDoor(GameObject obj)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/DoorUnlock", obj.transform.position);
        pauseLockedDoorInteraction = true;
        yield return new WaitForSeconds(waitTimer);
        pauseLockedDoorInteraction = false;

    }

}
