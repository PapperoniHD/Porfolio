using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcWalkandLook : MonoBehaviour
{

    private NavMeshAgent navAgent;
    private Animator anim;
    public Transform walkLocation;
    public Transform lookPos;

    [SerializeField] private FMODUnity.EventReference _footstep;
    private FMOD.Studio.EventInstance footstep;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        navAgent.enabled = true;

        if (!_footstep.IsNull)
        {
            footstep = FMODUnity.RuntimeManager.CreateInstance(_footstep);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var distance = ((gameObject.transform.position - walkLocation.position).magnitude);
        if (distance > 0.5)
        {
            navAgent.SetDestination(walkLocation.position);
            anim.Play("walking");

        }
        if (distance < 0.5)
        {
            anim.Play("idle");
            transform.LookAt(lookPos);
            navAgent.SetDestination(transform.position);

        }
    }


    public void PlayStep()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(footstep, transform);
        footstep.start();
    }
}
