using UnityEngine;
using UnityEngine.UIElements;
public class NewMonoBehaviourScript : MonoBehaviour
{
    private VisualElement rootElement;
    private Button placeTowersButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        rootElement = uiDocument.rootVisualElement;
        placeTowersButton = rootElement.Q<Button>("PlaceTowerButton");
        
        if (placeTowersButton != null)
        {
            placeTowersButton.clicked += () => GridManager.instance.StartPlacementMode(); // Increase health on click
        }
    }
}
