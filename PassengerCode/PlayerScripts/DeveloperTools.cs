using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperTools : MonoBehaviour
{
    public GameObject Player = null;
    public Transform spawn = null;

    private CharacterController playerCC;
    [SerializeField]
    private Animator playerAnim;

    private float moveSpeed = 10f;
    public Camera playerCamera;

    public bool cheats = false;

    public GameObject cheatsTEXT;

    // Start is called before the first frame update
    void Start()
    {
        playerCC = FindObjectOfType<CharacterController>();

        EnablePlayer();
    }

    public void EnablePlayer()
    {
        playerAnim.enabled = true;
        playerCC.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            print("Respawn");
            Player.transform.position = spawn.position;
        }

        if (cheats)
        {
            NoClip();
            cheatsTEXT.SetActive(true);
        }
        else
        {
            cheatsTEXT.SetActive(false);
        }

        if (Input.GetKey(KeyCode.RightAlt) && Input.GetKey(KeyCode.Backspace))
        {
            if (cheats)
            {
                cheats = false;
            }
            else
            {
                cheats = true;
            }
            
        }
    
    }

    void NoClip()
    {
        if (Input.GetKey(KeyCode.V))
        {
            playerAnim.enabled = false;
            playerCC.enabled = false;
            NoClipMovement();
        }
        else
        {
            playerAnim.enabled = true;
            playerCC.enabled = true;
        }
    }

    void NoClipMovement()
    {

        Player.GetComponent<PlayerMovement>().velocity.y = -5;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 20f;
        }
        else
        {
            moveSpeed = 10f;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = playerCamera.transform.right * h + playerCamera.transform.forward * v;

        Player.transform.position += move * moveSpeed * Time.deltaTime;


    }
}
