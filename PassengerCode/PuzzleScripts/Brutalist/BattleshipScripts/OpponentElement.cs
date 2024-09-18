using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OpponentElement : MonoBehaviour
{
    public bool shipTile;
    public bool alreadyClicked = false;

    public Color hit;
    public Color miss;
    public Color normalColor;

    [SerializeField] private FMODUnity.EventReference _hitSound;
    private FMOD.Studio.EventInstance hitSound;

    [SerializeField] private FMODUnity.EventReference _missSound;
    private FMOD.Studio.EventInstance missSound;

    public void Clicked()
    {
        var image = GetComponent<Image>();
        if (shipTile)
        {
            image.color = hit;
            PlaySound(hitSound);
        }
        else
        {
            image.color = miss;
            PlaySound(missSound);
        }
        alreadyClicked = true;
    }

    public void PlaySound(FMOD.Studio.EventInstance sound)
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(sound, transform);
        sound.start();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!_hitSound.IsNull)
        {
            hitSound = FMODUnity.RuntimeManager.CreateInstance(_hitSound);
        }
        if (!_missSound.IsNull)
        {
            missSound = FMODUnity.RuntimeManager.CreateInstance(_missSound);
        }
        var image = GetComponent<Image>();
        image.color = normalColor;
    }

    public void ResetButton()
    {
        var image = GetComponent<Image>();
        image.color = normalColor;
        alreadyClicked = false;
        var shipGame = FindObjectOfType<BattleshipsGame>();
        shipGame.aiElementList.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
