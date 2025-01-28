using UnityEngine;
using UnityEngine.UIElements;

public class TitleScreenEventManager : MonoBehaviour
{
    private VisualElement rootElement;
    private VisualElement startButton;
    private VisualElement exitButton;
    public GameObject titleScreenObj;
    public GameObject dificultySelectObj;

    void OnEnable()
    {
        var UIDocument = GetComponent<UIDocument>();
        rootElement = UIDocument.rootVisualElement;
        startButton = rootElement.Q<VisualElement>("StartGameBtn");
        exitButton = rootElement.Q<VisualElement>("ExitGameBtn");
    }

    void Awake()
    {
        titleScreenObj.SetActive(true);
        dificultySelectObj.SetActive(false);
    }


    private void Update()
    {
        startButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked!");

            titleScreenObj.SetActive(false);
            dificultySelectObj.SetActive(true);
        }));

        exitButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked!");
        }));
    }
}
