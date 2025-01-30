using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleScreenEventManager : MonoBehaviour
{
    private VisualElement rootElement;
    private VisualElement startButton;
    private VisualElement exitButton;
    //public GameObject titleScreenObj;
    //public GameObject dificultySelectObj;
    public AudioSource menuAudio;
    public AudioClip gameStartSFX;
    public AudioClip uiButtonSFX;

    void OnEnable()
    {
        var UIDocument = GetComponent<UIDocument>();
        rootElement = UIDocument.rootVisualElement;
        startButton = rootElement.Q<VisualElement>("StartGameBtn");
        exitButton = rootElement.Q<VisualElement>("ExitGameBtn");
    }

    //void Awake()
    //{
    //    titleScreenObj.SetActive(true);
    //    dificultySelectObj.SetActive(false);
    //}


    private void Update()
    {
        startButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked! And selected 'START GAME' button");

            StartCoroutine(WaitForStartAudio());

            menuAudio.PlayOneShot(gameStartSFX);
            //titleScreenObj.SetActive(false);
            //dificultySelectObj.SetActive(true);
        }));

        exitButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked! And quit application");

            StartCoroutine(WaitForUIAudio());

            menuAudio.PlayOneShot(uiButtonSFX);
        }));
    }

    IEnumerator WaitForStartAudio()
    {
        yield return new WaitForSeconds(gameStartSFX.length - 0.5f);
        Debug.Log("Started Game");
        SceneManager.LoadScene("Main");
    }

    IEnumerator WaitForUIAudio()
    {
        yield return new WaitForSeconds(uiButtonSFX.length);
        Debug.Log("Exited Game");
        Application.Quit();
    }
}
