using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleScreenEventManager : MonoBehaviour
{
    private VisualElement rootElement;
    private VisualElement startButton;
    private VisualElement exitButton;
    public AudioSource menuSource;
    public AudioClip startGameAudio;
    public AudioClip menuButtonAudio;
    //public GameObject titleScreenObj;
    //public GameObject dificultySelectObj;

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

            menuSource.PlayOneShot(startGameAudio);

            StartCoroutine(WaitForStartGameAudio());

            //titleScreenObj.SetActive(false);
            //dificultySelectObj.SetActive(true);
        }));

        exitButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked! And quit application");

            menuSource.PlayOneShot(menuButtonAudio);

            StartCoroutine(WaitForUIAudio());
        }));
    }

    IEnumerator WaitForStartGameAudio()
    {
        yield return new WaitForSeconds(startGameAudio.length - 2f);
        SceneManager.LoadScene("Main");
        Debug.Log("Started game");
    }

    IEnumerator WaitForUIAudio()
    {
        yield return new WaitForSeconds(menuButtonAudio.length);
        Application.Quit();
        Debug.Log("Exited game");
    }
}
