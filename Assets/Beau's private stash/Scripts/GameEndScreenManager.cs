using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameEndScreenManager : MonoBehaviour
{
    private VisualElement rootElement;
    private VisualElement exitButton;
    private VisualElement restartButton;

    public AudioSource menuSource;
    public AudioClip menuButtonAudio;
    public AudioClip startGameAudio;

    void OnEnable()
    {
        var UIDocument = GetComponent<UIDocument>();
        rootElement = UIDocument.rootVisualElement;
        exitButton = rootElement.Q<VisualElement>("ExitGameBtn");
        restartButton = rootElement.Q<VisualElement>("RestartBtn");
    }

    private void Update()
    {
        restartButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked! And selected 'Restart' button");

            Time.timeScale = 1.0f;
            menuSource.PlayOneShot(startGameAudio);
            StartCoroutine(WaitForStartGameAudio());
        }));

        exitButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked! And sent to TitleScreen Scene");

            menuSource.PlayOneShot(menuButtonAudio);
            StartCoroutine(WaitForUIButtonAudio());
        }));

        IEnumerator WaitForStartGameAudio()
        {
            yield return new WaitForSeconds(startGameAudio.length - 2f);
            SceneManager.LoadScene("Main");
            Debug.Log("restarted");
        }

        IEnumerator WaitForUIButtonAudio()
        {
            yield return new WaitForSeconds(menuButtonAudio.length);
            SceneManager.LoadScene("TitleScreen");
            Debug.Log("Exited");
        }
    }
}
