using UnityEngine;
using UnityEngine.UIElements;

public class TitleScreenEventManager : MonoBehaviour
{
    public UIDocument titleScreenDoc;
    private VisualElement rootElement;
    private VisualElement myButton;
    private VisualElement mySecButton;

    void OnEnable()
    {
        var UIDocument = GetComponent<UIDocument>();
        rootElement = UIDocument.rootVisualElement;
        myButton = rootElement.Q<VisualElement>("StartGameBtn");
        mySecButton = rootElement.Q<VisualElement>("ExitGameBtn");
    }
    private void Awake()
    {
       //element = titleScreenDoc.GetComponent<VisualElement>("");
    }

    private void Update()
    {
        myButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked!");
        }));

        mySecButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked!");
        }));
    }
}
