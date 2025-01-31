using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroductionScroller : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject previousButton;
    [SerializeField] private Sprite[] introImages;

    int currentImage = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image.sprite = introImages[currentImage];
    }

    // Update is called once per frame
    void Update()
    {
        if(currentImage == 0)
        {
            previousButton.SetActive(false);
        }
        else
        {
            previousButton.SetActive(true);
        }

        
        Debug.Log(introImages.Length);
    }

    public void NextButton()
    {
        if (currentImage + 1 <= introImages.Length - 1)
        {
            currentImage++;
            image.sprite = introImages[currentImage];
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }

    public void PrevButton() 
    { 
        if(currentImage - 1 >= 0) 
        {
            currentImage--;
            image.sprite = introImages[currentImage];
        }        
    }
}
