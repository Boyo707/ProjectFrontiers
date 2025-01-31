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
    void OnEnable()
    {
        var UIDocument = GetComponent<UIDocument>();
        rootElement = UIDocument.rootVisualElement;
        startButton = rootElement.Q<VisualElement>("StartGameBtn");
        exitButton = rootElement.Q<VisualElement>("ExitGameBtn");
    }

    private void Update()
    {
        startButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked! And selected 'START GAME' button");
            SceneManager.LoadScene("Main");
        }));

        exitButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked! And quit application");
            Application.Quit();
        }));
    }
}
