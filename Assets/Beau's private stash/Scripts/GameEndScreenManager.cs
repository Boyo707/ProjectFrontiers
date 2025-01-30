using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameEndScreenManager : MonoBehaviour
{
    private VisualElement rootElement;
    private VisualElement exitButton;
    private VisualElement restartButton;

    public GameObject endScreenMenuObj;

    void OnEnable()
    {
        var UIDocument = GetComponent<UIDocument>();
        rootElement = UIDocument.rootVisualElement;
        exitButton = rootElement.Q<VisualElement>("ExitGameBtn");
        restartButton = rootElement.Q<VisualElement>("RestartBtn");
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

        restartButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked! And selected 'Restart' button");

            Time.timeScale = 1.0f;
            SceneManager.LoadScene("Main");
            endScreenMenuObj.SetActive(false);
        }));

        exitButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked! And sent to TitleScreen Scene");

            SceneManager.LoadScene("TitleScreen");
            endScreenMenuObj.SetActive(false);
        }));
    }
}
