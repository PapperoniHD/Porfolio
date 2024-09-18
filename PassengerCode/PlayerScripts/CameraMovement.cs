using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 1f;
    public Transform t_player;
    public Animator camAnim;

    float xRotation = 0f;
    float yRotation = 0f;
    public float direction;


    // Start is called before the first frame update
    void Start()
    {
        UpdateMouseSensitivity();
    }

    // Update is called once per frame
    void Update()
    {
        MovementStates();
        //WalkCameraPanning();
    }

    public void UpdateMouseSensitivity()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1f);
    }

    

    void WalkCameraPanning()
    {
        if (camAnim != null)
        {
            var x = Input.GetAxisRaw("Horizontal");
            camAnim.SetFloat("DirectionFloat", x);
        }
        
    }

    void MovementStates()
    {
        var state = GameManager.GM.State;

        switch (state)
        {
            case GameState.Gameplay:
            {
                Movement();
                break;
            }
            case GameState.Hiding:
            {
                HidingMovement();
                break;
            }
            case GameState.Elevator:
            {
                ElevatorMovement();
                break;
            }
        }

    }

    void Movement()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        t_player.Rotate(Vector3.up * mouseX);

    }

    void HidingMovement()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        yRotation -= mouseX;
        xRotation = Mathf.Clamp(xRotation, -20f, 20f);
        yRotation = Mathf.Clamp(yRotation, -60f, 60f);

        transform.localRotation = Quaternion.Euler(xRotation, -yRotation, 0f);
        //t_player.Rotate(Vector3.up * mouseX);

    }

    void ElevatorMovement()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        yRotation -= mouseX;
        xRotation = Mathf.Clamp(xRotation, -20f, 20f);
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, -yRotation, 0f);
        //t_player.Rotate(Vector3.up * mouseX);

    }



}
