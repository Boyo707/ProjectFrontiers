using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseScreenMenuManager : MonoBehaviour
{
    private VisualElement rootElement;
    private VisualElement resumeButton;
    private VisualElement exitButton;
    private VisualElement restartButton;

    public GameObject pauseMenuObj;

    void OnEnable()
    {
        var UIDocument = GetComponent<UIDocument>();
        rootElement = UIDocument.rootVisualElement;
        resumeButton = rootElement.Q<VisualElement>("ResumeBtn");
        exitButton = rootElement.Q<VisualElement>("ExitGameBtn");
        restartButton = rootElement.Q<VisualElement>("RestartBtn");
    }

    private void Update()
    {
        if (pauseMenuObj.activeSelf)
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

            Time.timeScale = 1.0f;
            pauseMenuObj.SetActive(false);
        }));

        restartButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked! And selected 'Restart' button");

            Time.timeScale = 1.0f;
            SceneManager.LoadScene("Main");
            pauseMenuObj.SetActive(false);
        }));

        exitButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked! And sent to TitleScreen Scene");

            SceneManager.LoadScene("TitleScreen");
            pauseMenuObj.SetActive(false);
        }));
    }
}
