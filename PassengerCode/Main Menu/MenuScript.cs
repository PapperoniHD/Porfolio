using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuScript : MonoBehaviour
{
    // animator variable
    [SerializeField] private FMODUnity.EventReference _woosh;
    private FMOD.Studio.EventInstance woosh;
    public Animator anim;
    public IntroManager intro;
    public GameObject[] introDialogue;
    public GameObject[] mainMenu;
    public GameObject bg;

    public TextMeshProUGUI startText;

    public GameObject continueGameText;

    public int levelSaveIndex = 0;

    public Animator trainAnim;

    private void Start()
    {
        if (!_woosh.IsNull)
        {
            woosh = FMODUnity.RuntimeManager.CreateInstance(_woosh);
        }
        intro = FindObjectOfType<IntroManager>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        foreach (var item in introDialogue)
        {
            item.SetActive(false);
        }
        bg.SetActive(false);

        levelSaveIndex = PlayerPrefs.GetInt("LevelIndex");
        if (levelSaveIndex > 0)
        {
            startText.SetText("new game");
            continueGameText.SetActive(true);
        }
        else
        {
            startText.SetText("start");
            continueGameText.SetActive(false);
        }

        
    }

    // starts animation trigger, delays loading "game" scene and resets animation after 2 seconds.
    public void NewGame()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(woosh, transform);
        woosh.start();
        anim.Play("TransitionClose");
        trainAnim.Play("MainMenuCloseDoor");
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        //Invoke("Next", 4f);
        //intro.StartDialogue();
        Time.timeScale = 1.0f;
        StartCoroutine(RemoveMainMenu());
        PlayerPrefs.SetInt("LastLevelChoice", 0);
        PlayerPrefs.SetInt("LevelIndex", 0);
        //PlayerPrefs.SetString("PlayerName", null);
        //ResetAnim(2.0f);
    }

    public void ContinueGame()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(woosh, transform);
        woosh.start();
        anim.Play("TransitionClose");
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        //Invoke("Next", 4f);
        //intro.StartDialogue();
        Time.timeScale = 1.0f;

        LoadSavedScene(PlayerPrefs.GetInt("LevelIndex"));
    }

    IEnumerator RemoveMainMenu()
    {
        yield return new WaitForSeconds(4);
        foreach (var item in mainMenu)
        {
            item.SetActive(false);
        }
        foreach (var item in introDialogue)
        {
            item.SetActive(true);
        }
        bg.SetActive(true);
    }

    // waits for 'time' and then resets the animator anim trigger.
    public IEnumerator ResetAnim(float time)
    {
        yield return new WaitForSeconds(time);

        anim.ResetTrigger("StartTrigger");
    }

    // loads game scene
    public void Play()
    {
        SceneManager.LoadScene("Intro");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("PassengerMainMenu");
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // loads next scene
    public void Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Previous()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void LoadSavedScene(int index)
    {
        StartCoroutine(LoadYourAsyncScene(index));
    }

    public IEnumerator LoadYourAsyncScene(int index)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        yield return new WaitForSeconds(1);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    // quits game
    public void Quit()
    {
        Application.Quit();
    }
}
