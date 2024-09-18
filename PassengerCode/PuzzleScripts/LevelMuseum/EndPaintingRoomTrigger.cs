using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPaintingRoomTrigger : MonoBehaviour
{

    private Collider col;
    public GameObject[] lights;
    public SwitchPaintingTexture[] paintings;
    private MuseumPuzzleManager puzzleManager;
    public GameObject endPainting;
    public GameObject lowBassSound;

    [SerializeField] private FMODUnity.EventReference _spotlight;
    private FMOD.Studio.EventInstance spotlight;

    private GameObject player;
    public float teleportYValue = 80;

    // Start is called before the first frame update
    void Start()
    {
        endPainting.SetActive(false);
        col = GetComponent<Collider>();
        puzzleManager = FindObjectOfType<MuseumPuzzleManager>();
        if (!_spotlight.IsNull)
        {
            spotlight = FMODUnity.RuntimeManager.CreateInstance(_spotlight);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            col.enabled = false;
            StartCoroutine(LightsTurnOn());
            puzzleManager.bigDoors.Play("BigDoorsMuseumClose");
        }
    }

    public void ReturnToNormal()
    {
        TeleportPlayer();
        Destroy(lowBassSound);
        endPainting.SetActive(true);
        foreach (var item in paintings)
        {
            item.SwitchToOriginalMaterial();
        }
        foreach (var item in lights)
        {
            item.SetActive(false);
        }
    }
    private void TeleportPlayer()
    {
        Vector3 currentPos = player.transform.localPosition;
        Vector3 targetPos = new Vector3 (currentPos.x, currentPos.y + teleportYValue, currentPos.z);

        player.GetComponent<CharacterController>().enabled = false;
        player.transform.localPosition = targetPos;// + Vector3.up * 0.005f;
        player.GetComponent<CharacterController>().enabled = true;
    }

    IEnumerator LightsTurnOn()
    {
        lights[0].SetActive(true);
        paintings[0].SwitchToOtherMaterial();
        paintings[1].SwitchToOtherMaterial();
        PlaySound(spotlight);

        yield return new WaitForSeconds(1);

        paintings[2].SwitchToOtherMaterial();
        paintings[3].SwitchToOtherMaterial();
        lights[1].SetActive(true);
        PlaySound(spotlight);

        yield return new WaitForSeconds(1);

        paintings[4].SwitchToOtherMaterial();
        paintings[5].SwitchToOtherMaterial();
        lights[2].SetActive(true);
        PlaySound(spotlight);

        yield return new WaitForSeconds(1);

        paintings[6].SwitchToOtherMaterial();
        lights[3].SetActive(true);
        PlaySound(spotlight);
    }

    public void PlaySound(FMOD.Studio.EventInstance sound)
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(sound, transform);
        sound.start();
    }
}
