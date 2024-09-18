using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class DialogueSelector : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference _click;
    private FMOD.Studio.EventInstance click;

    public float mouseDir;
    public float blend;
    public Image arrow;
    public float timeToReturn = 1;
    private float t;

    private Animator anim;

    public Color defaultColor;
    public Color yesColor;
    public Color noColor;
    private bool canPlaySound = true;

    public TextMeshProUGUI yesText;
    public TextMeshProUGUI noText;

    [SerializeField]
    private ButtonClickedEvent m_OnClickYes = new ButtonClickedEvent();
    [SerializeField]
    private ButtonClickedEvent m_OnClickNo = new ButtonClickedEvent();

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        anim = GetComponent<Animator>();

        if (!_click.IsNull)
        {
            click = FMODUnity.RuntimeManager.CreateInstance(_click);
        }
    }

    public ButtonClickedEvent onClickYes
    {
        get { return m_OnClickYes; }
        set { m_OnClickYes = value; }
    }
    public ButtonClickedEvent onClickNo
    {
        get { return m_OnClickNo; }
        set { m_OnClickNo = value; }
    }
    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        //print(blend);
        anim.SetFloat("Blend", blend);

        if (mouseX>0.5f)
        {

            mouseDir = 1;
            
        }
        else if (mouseX<-0.5f)
        {
            mouseDir = 0;
            
        }
        else
        {
            mouseDir = 0.5f;

        }
        

        if (mouseDir == 1)
        {
            //anim.SetFloat("Blend", mouseDir);
            blend = Mathf.Lerp(blend, mouseDir, Time.deltaTime * 50);
            noText.color = noColor;
            yesText.color = defaultColor;
            if (canPlaySound)
            {
                PlaySound();
            }
            if (blend > mouseDir)
            {
                blend = mouseDir;
            }          
        }
        if (mouseDir == 0)
        {
            //anim.SetFloat("Blend", mouseDir);
            yesText.color = yesColor;
            noText.color = defaultColor;
            if (canPlaySound)
            {
                PlaySound();
            }
            
            click.start();
            blend = Mathf.Lerp(blend, mouseDir, Time.deltaTime * 50);
        }
        if (mouseDir == 0.5)
        {
            t += Time.deltaTime;
            if (t > timeToReturn)
            {
                blend = Mathf.Lerp(blend, mouseDir, Time.deltaTime * 50);
                yesText.color = defaultColor;
                noText.color = defaultColor;
                canPlaySound = true;
            }
            
        }
        else
        {
            t = 0;
        }


        if (blend > 0.8f)
        {
            if (Input.GetButtonDown("Interact"))
            {
                m_OnClickNo.Invoke();
            }
        }
        if (blend < 0.2f)
        {
            if (Input.GetButtonDown("Interact"))
            {
                m_OnClickYes.Invoke();
            }
        }
    }

    void PlaySound()
    {
        canPlaySound = false;
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(click, transform);
        click.start();
    }
    public void Yes()
    {
        print("Yes");
    }
    public void No()
    {
        print("No");
    }
}
