using UnityEngine;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    private VisualElement rootElement;
    private Button pauseButton;

    public GameObject overlayHUDObj;
    public GameObject pauseMenuObj;
    public GameObject placeTowerInteraction;

    void OnEnable()
    {
        var UIDocument = overlayHUDObj.GetComponent<UIDocument>();
        rootElement = UIDocument.rootVisualElement;
        pauseButton = rootElement.Q<Button>("PauseButton");
    }

    void Awake()
    {
        pauseMenuObj.SetActive(false);
        placeTowerInteraction.SetActive(true);
    }

    private void Update()
    {
        if (pauseButton != null)
        {
            pauseButton.clicked += () =>
            {
                Debug.Log("");
                Time.timeScale = 1.0f;
                pauseMenuObj.SetActive(true);
                placeTowerInteraction.SetActive(false);
            };
        }

        if (!pauseMenuObj.activeSelf)
        {
            placeTowerInteraction.SetActive(true);
        }
    }
}
