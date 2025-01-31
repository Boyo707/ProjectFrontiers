using UnityEngine;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    private VisualElement rootElement;
    private Button pauseButton;

    public GameObject overlayHUDObj;
    public GameObject pauseMenuObj;

    void OnEnable()
    {
        var UIDocument = overlayHUDObj.GetComponent<UIDocument>();
        rootElement = UIDocument.rootVisualElement;
        pauseButton = rootElement.Q<Button>("PauseButton");
    }

    void Awake()
    {
        pauseMenuObj.SetActive(false);
    }

    private void Update()
    {
        if (pauseButton != null)
        {
            pauseButton.clicked += () =>
            {
                pauseMenuObj.SetActive(true);
                EventBus<PauseGameEvent>.Publish(new PauseGameEvent(this));
            };
        }
    }
}
