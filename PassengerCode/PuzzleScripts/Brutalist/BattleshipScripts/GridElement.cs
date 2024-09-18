using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class GridElement : MonoBehaviour
{
    public bool shipTile;

    public bool clicked = false;

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
        GetComponent<Button>().enabled = false;
        FindObjectOfType<BattleshipsGame>().Clicked();
        FindObjectOfType<BattleshipsGame>().chosenElement = this;
        clicked = true;
        ///ResetButton();
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
        GetComponent<Button>().enabled = true;
        var image = GetComponent<Image>();
        image.color = normalColor;
        clicked = false;
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
