using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
public class LevelScripts : MonoBehaviour
{
    public string Level1;
    public string TrainRide;
    public string TrainRideDefault;
    public string Level2;
    public string Level3;
    public string Level4;
    public string Level5;
    public string LastLevel;
    public string BrutalistLevel;
    public string SpiritWorld;

    private Interactable interactScript;
    public Transform chairRideScene;
    public CinemachineVirtualCamera vcam;

    private GameManager gm;

    public Animator transitionAnim;
    public Animator trainAnim;

    float vcamAmpFreq = 0.2f;
    bool lerpFog = false;
    bool lerpCamShake = false;

    public GameObject savedText;

    FMOD.Studio.Bus MasterBus;

    // Start is called before the first frame update
    void Awake()
    {
        MasterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
        
        gm = GetComponent<GameManager>();
        interactScript = FindObjectOfType<Interactable>();
        if (SceneManager.GetActiveScene().name == TrainRide)
        {
            if (chairRideScene != null)
            {
                interactScript.sitObj = chairRideScene.GetChild(0);
            }
            gm.Hiding();
            StartCoroutine(FirstTrainRide());
            RenderSettings.fog = true;
        }
        if (SceneManager.GetActiveScene().name == "TrainRideDefault 1")
        {
            if (chairRideScene != null)
            {
                interactScript.sitObj = chairRideScene.GetChild(0);
            }
            gm.Hiding();
            StartCoroutine(SecondTrainRide());
            //RenderSettings.fog = true;
        }

        if (SceneManager.GetActiveScene().name == TrainRideDefault)
        {
            if (chairRideScene != null)
            {
                interactScript.sitObj = chairRideScene.GetChild(0);
            }
            gm.Hiding();
            StartCoroutine(SecondTrainRide());
            //RenderSettings.fog = true;
        }

        if (SceneManager.GetActiveScene().name == Level1)
        {
            gm.Gameplay();
        }
        if (SceneManager.GetActiveScene().name == Level2)
        {
            StartCoroutine(DarkLevel(10));
            trainAnim.Play("LevelDark");
            gm.Hiding();
            if (chairRideScene != null)
            {
                interactScript.sitObj = chairRideScene;
            }
            SaveLevel();
        }
        if (SceneManager.GetActiveScene().name == Level3)
        {
            MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            trainAnim.Play("TrainArriveMusem");
            StartCoroutine(DarkLevel(10));
            gm.Hiding();
            if (chairRideScene != null)
            {
                interactScript.sitObj = chairRideScene;
            }
            SaveLevel();         
        }
        if (SceneManager.GetActiveScene().name == Level4)
        {
            MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            trainAnim.Play("TrainWilliamLevel4");
            StartCoroutine(DarkLevel(10));
            gm.Hiding();
            if (chairRideScene != null)
            {
                interactScript.sitObj = chairRideScene;
            }
            SaveLevel();         
        }
        if (SceneManager.GetActiveScene().name == Level5)
        {
            trainAnim.Play("WaterLevelTrainArrive");
            StartCoroutine(DarkLevel(10));
            gm.Hiding();
            if (chairRideScene != null)
            {
                interactScript.sitObj = chairRideScene;
            }
            SaveLevel();
            PlayerPrefs.SetInt("LastLevelChoice", 0);
        }
        if (SceneManager.GetActiveScene().name == LastLevel)
        {
            trainAnim.Play("LastLevelArrive");
            StartCoroutine(DarkLevel(12));
            gm.Hiding();
            if (chairRideScene != null)
            {
                interactScript.sitObj = chairRideScene;
            }
            //MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //SaveLevel();
            
        }
        if (SceneManager.GetActiveScene().name == BrutalistLevel)
        {         
            SaveLevel();
        }
        if (SceneManager.GetActiveScene().name == SpiritWorld)
        {
            MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            if (PlayerPrefs.GetInt("LastLevelChoice") == 0)
            {
                trainAnim.Play("TrainSpiritWorldArrive");
                StartCoroutine(DarkLevel(12));
                gm.Hiding();
                if (chairRideScene != null)
                {
                    interactScript.sitObj = chairRideScene;
                }
                SaveLevel();
            }
            else
            {

            }
            
        }

    }

    void SaveLevel()
    {
        PlayerPrefs.SetInt("LevelIndex", SceneManager.GetActiveScene().buildIndex);
        StartCoroutine(SavedText());
    }

    public IEnumerator SavedText()
    {
        savedText.SetActive(true);
        yield return new WaitForSeconds(4);
        savedText.SetActive(false);
    }

    IEnumerator FirstTrainRide()
    {
        trainAnim.Play("TrainLights");
        yield return new WaitForSeconds(5);
        lerpCamShake = true;
        yield return new WaitForSeconds(9);
        lerpFog = true;
        lerpCamShake = false;
        yield return new WaitForSeconds(5);
        transitionAnim.Play("TransitionClose");
        yield return new WaitForSeconds(1);
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator SecondTrainRide()
    {
        yield return new WaitForSeconds(9);
        transitionAnim.Play("TransitionClose");
        yield return new WaitForSeconds(1);
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator DarkLevel(int wait)
    {
        yield return new WaitForSeconds(wait);
        interactScript.StopHiding(chairRideScene.GetChild(0));
        chairRideScene.gameObject.SetActive(false);
    }



    public IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lerpFog)
        {
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, 0.5f, Time.deltaTime * 0.5f);
            
        }
        

        if (vcam != null && vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>() != null)
        {
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = vcamAmpFreq;
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = vcamAmpFreq;

            if (lerpCamShake)
            {
                vcamAmpFreq = Mathf.Lerp(vcamAmpFreq, 2, Time.deltaTime * 0.5f);
            }
            else
            {
                vcamAmpFreq = Mathf.Lerp(vcamAmpFreq, 0, Time.deltaTime * 0.5f);
            }
        }
        
    }
}
