using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private int MaterialValue;
    [SerializeField] private FMODUnity.EventReference _footsteps;
    private FMOD.Studio.EventInstance footsteps;
    RaycastHit rh;
    float distance = 0.1f;
    [SerializeField]
    LayerMask lm;
    // Start is called before the first frame update
    void Start()
    {
        if (!_footsteps.IsNull)
        {
            footsteps = FMODUnity.RuntimeManager.CreateInstance(_footsteps);
        }
    }

    private void Update()
    {
        MaterialCheck();
        
    }

    public void PlayFootstep()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(footsteps, transform);
        footsteps.setParameterByName("WalkRun", 0, false);
        footsteps.setParameterByName("Footsteps", MaterialValue, false);
        footsteps.start();  
    }

    public void PlayRunFootstep()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(footsteps, transform);
        footsteps.setParameterByName("WalkRun", 1, false);
        footsteps.setParameterByName("Footsteps", MaterialValue, false);
        footsteps.start();
    }

    public void PlayCrouchFootstep()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(footsteps, transform);
        footsteps.setParameterByName("WalkRun", 2, false);
        footsteps.setParameterByName("Footsteps", MaterialValue, false);
        footsteps.start();
    }

    void MaterialCheck()
    {

        Debug.DrawRay(transform.position, Vector3.down * distance);
        if (Physics.Raycast(transform.position, Vector3.down, out rh, distance, lm))
        {
            switch (rh.collider.tag)
            {
                case "Concrete":
                    MaterialValue = 0;
                    break;
                case "Grass":
                    MaterialValue = 1;
                    break;
                case "Metal":
                    MaterialValue = 2;
                    break;
                case "Wood":
                    MaterialValue = 3;
                    break;
                case "Water":
                    MaterialValue = 4;
                    break;
                case null:
                    MaterialValue = 0;
                    break;
            }
        }
    }   
}
