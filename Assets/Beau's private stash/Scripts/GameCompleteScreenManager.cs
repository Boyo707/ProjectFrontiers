using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameCompleteScreenManager : MonoBehaviour
{
    private VisualElement rootElement;
    private VisualElement exitButton;
    private VisualElement resumeButton;
    public AudioSource menuAudio;
    public AudioClip uiButtonSFX;

    public GameObject endScreenMenuObj;

    void OnEnable()
    {
        var UIDocument = GetComponent<UIDocument>();
        rootElement = UIDocument.rootVisualElement;
        exitButton = rootElement.Q<VisualElement>("ExitGameBtn");
        resumeButton = rootElement.Q<VisualElement>("RestartBtn");
    }

    private void Update()
    {
        if (endScreenMenuObj.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }

        resumeButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked! And selected 'Resume' button");

            menuAudio.PlayOneShot(uiButtonSFX);
            Time.timeScale = 1.0f;
            endScreenMenuObj.SetActive(false);
        }));

        exitButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked! And sent to TitleScreen Scene");

            menuAudio.PlayOneShot(uiButtonSFX);
            SceneManager.LoadScene("TitleScreen");
            endScreenMenuObj.SetActive(false);
        }));
    }
}
