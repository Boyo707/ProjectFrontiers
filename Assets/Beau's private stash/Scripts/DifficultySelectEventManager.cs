using UnityEngine;
using UnityEngine.UIElements;

public class DifficultySelectEventManager : MonoBehaviour
{
    private VisualElement rootElement;
    private VisualElement easyButton;
    private VisualElement normalButton;
    private VisualElement hardButton;
    private VisualElement backButton;

    public GameObject titleScreenObj;
    public GameObject difficultySelectObj;

    void OnEnable()
    {
        var UIDocument = GetComponent<UIDocument>();
        rootElement = UIDocument.rootVisualElement;
        easyButton = rootElement.Q<VisualElement>("Easy-Btn");
        normalButton = rootElement.Q<VisualElement>("Normal-Btn");
        hardButton = rootElement.Q<VisualElement>("Hard-Btn");
        backButton = rootElement.Q<VisualElement>("Back-Btn");
    }

    void Awake()
    {
        titleScreenObj.SetActive(false);
        difficultySelectObj.SetActive(true);
    }

    private void Update()
    {
        easyButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked!");
        }));

        normalButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked!");
        }));

        hardButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked!");
        }));

        backButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked!");

            titleScreenObj.SetActive(true);
            difficultySelectObj.SetActive(false);
        }));
    }
}
