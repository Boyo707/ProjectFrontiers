using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [SerializeField] Difficulty difficultySetting;

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
            Debug.Log("'EASY' difficulty selected");

            difficultySetting.EasySelect();

            SceneManager.LoadScene("Main");
        }));

        normalButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("'NORMAL' difficulty selected");

            difficultySetting.NormalSelect();

            SceneManager.LoadScene("Main");
        }));

        hardButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("'HARD' difficulty selected");

            difficultySetting.HardSelect();

            SceneManager.LoadScene("Main");
        }));

        backButton.AddManipulator(new Clickable(evt =>
        {
            Debug.Log("Clicked and selected 'BACK' button");

            titleScreenObj.SetActive(true);
            difficultySelectObj.SetActive(false);
        }));
    }
}
