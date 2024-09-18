using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseScript : MonoBehaviour
{
    FMOD.Studio.EventInstance SFXVolumeTestEvent;
    FMOD.Studio.Bus Master;

    public static bool Paused = false;

    public GameObject PauseMenuUI;
    public GameObject PauseMenuUIDEV;
    private GameObject Menu;
    private GameObject OptionsMenu;
    public Interactable interactScript;
    public CameraMovement mouseScript;
    //public MouseLook mouseLook;

    public float mouseSensitivity;
    public float masterVolume = 1;

    [SerializeField] private Slider _sensSlider;
    [SerializeField] private TextMeshProUGUI _sensSliderText;

    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private TextMeshProUGUI _volumeSliderText;

    public CameraMovement camScript;

    Resolution[] resolutions;

    public TMP_Dropdown resolutionDropdown;

    private void Awake()
    {
        Master = FMODUnity.RuntimeManager.GetBus("bus:/");
    }

    void Start()
    {
        Menu = PauseMenuUI.transform.GetChild(1).gameObject;
        Menu = PauseMenuUI.transform.GetChild(2).gameObject;
        mouseScript = FindObjectOfType<CameraMovement>();
        interactScript = FindObjectOfType<Interactable>();
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UnPause();

        ResolutionOptionInitialize();

        _sensSliderText.text = _sensSlider.value.ToString("0.00");

        _sensSlider.onValueChanged.AddListener( (v) => {
            _sensSliderText.text = v.ToString("0.00");
            mouseSensitivity = v;

        });

        _volumeSliderText.text = _volumeSlider.value.ToString("0.00");

        _volumeSlider.onValueChanged.AddListener((v) => {
            _volumeSliderText.text = v.ToString("0.00");
            masterVolume = v;

        });

        //Master.setVolume(PlayerPrefs.GetFloat("MasterVolume", 1));
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1);
        ApplyVolume();
        

    }

    void ResolutionOptionInitialize()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + "@" + resolutions[i].refreshRate + "hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height && 
                resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F))// && GameManager.GM.State == GameState.Gameplay)
        {

            if (Paused)
            {
                UnPause();
            }
            else
            {
                Pause();
            }

        }
#else
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (Paused)
            {
                UnPause();
            }
            else
            {
                Pause();
            }

        }
    
#endif
    }


    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void Pause()
    {
        if (mouseScript != null)
        {
            mouseScript.enabled = false;
        }
        if (interactScript != null)
        {
            interactScript.enabled = false;
        }
        PauseMenuUI.SetActive(true);
        //PauseMenuUI.GetComponent<Animator>().Play("PauseAnimation");
        Time.timeScale = 0;
        Paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Master.setPaused(true);
        //mouseLook.enabled = false;
    }

    public void UnPause()
    {
        if (mouseScript != null)
        {
            mouseScript.enabled = true;
        }
        if (interactScript != null)
        {
            interactScript.enabled = true;
        }
        

        Time.timeScale = 1;
        Paused = false;
 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1);
        _sensSlider.value = mouseSensitivity;
        PauseMenuUI.SetActive(false);

        Master.setPaused(false);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PreviousLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
   
    public void Option()
    {
        Menu.SetActive(false);
        Menu.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    
    public void MainMenu()
    {
        UnPause();
        SceneManager.LoadScene(1);
    }

    public void Apply()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);
        camScript.UpdateMouseSensitivity();
        ApplyVolume();
    }

    public void ApplySensitivity()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);
        if (camScript != null)
        {
            camScript.UpdateMouseSensitivity();
        }
        
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void ApplyVolume()
    {
        
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        Master.setVolume(PlayerPrefs.GetFloat("MasterVolume", 1));
        _volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1);
    }

    public void RestartLevel()
    {
        Master.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
