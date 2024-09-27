using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CarouselWithFullScreen : MonoBehaviour
{
    public GameObject imagePrefab;             
    public GameObject imagePrefab2;
    public GameObject imagePrefab3;
    public GameObject imagePrefab4;             
    public Transform carouselContainer;      
    public Button spawnButton;                
    public Button spawnButton2;
    public Button spawnButton3;
    public Button spawnButton4;
    public Button leftButton;                  
    public Button rightButton;                 
    public int visibleImageLimit = 3;          

    public GameObject fullScreenPanel;        
    public Image fullScreenImage;             
    public Image fullScreenImage2;
    public Image fullScreenImage3;
    public Image fullScreenImage4;
    public Button closeFullScreenButton;       
    public float transitionDuration = 0.3f;   

    private int currentImageCount = 0;
    private int currentStartIndex = 0;

    void Start()
    {
        spawnButton.onClick.AddListener(SpawnNewImage);
        leftButton.onClick.AddListener(SlideLeft);
        rightButton.onClick.AddListener(SlideRight);
        closeFullScreenButton.onClick.AddListener(CloseFullScreen);

        fullScreenPanel.SetActive(false); 
        UpdateSlideButtons();
    }

    void SpawnNewImage()
    {
        GameObject newImage = Instantiate(imagePrefab, carouselContainer);
        newImage.GetComponent<Button>().onClick.AddListener(() => OpenFullScreen(newImage.GetComponent<Image>()));
        currentImageCount++;

        UpdateCarouselPosition();
    }

    void OpenFullScreen(Image imageToFullScreen)
    {
        fullScreenPanel.SetActive(true);
        fullScreenImage.sprite = imageToFullScreen.sprite; 

        StartCoroutine(AnimateFullScreenTransition(imageToFullScreen.rectTransform, fullScreenImage.rectTransform, true));
    }

   
    void CloseFullScreen()
    {
        fullScreenPanel.SetActive(false); 
    }


    System.Collections.IEnumerator AnimateFullScreenTransition(RectTransform fromRect, RectTransform toRect, bool toFullScreen)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = fromRect.position;
        Vector3 targetPosition = toFullScreen ? toRect.position : initialPosition;

        Vector3 initialScale = fromRect.localScale;
        Vector3 targetScale = toFullScreen ? new Vector3(1f, 1f, 1f) : initialScale;

        while (elapsedTime < transitionDuration)
        {
            float progress = elapsedTime / transitionDuration;
            fromRect.position = Vector3.Lerp(initialPosition, targetPosition, progress);
            fromRect.localScale = Vector3.Lerp(initialScale, targetScale, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fromRect.position = targetPosition;
        fromRect.localScale = targetScale;
    }


    void SlideLeft()
    {
        if (currentStartIndex > 0)
        {
            currentStartIndex--;
            UpdateCarouselPosition();
        }
    }

    void SlideRight()
    {
        if (currentStartIndex < currentImageCount - visibleImageLimit)
        {
            currentStartIndex++;
            UpdateCarouselPosition();
        }
    }

    void UpdateCarouselPosition()
    {
        for (int i = 0; i < carouselContainer.childCount; i++)
        {
            Transform child = carouselContainer.GetChild(i);
            child.gameObject.SetActive(i >= currentStartIndex && i < currentStartIndex + visibleImageLimit);
        }

        UpdateSlideButtons();
    }

    void UpdateSlideButtons()
    {
        leftButton.gameObject.SetActive(currentStartIndex > 0);
        rightButton.gameObject.SetActive(currentStartIndex < currentImageCount - visibleImageLimit);
    }
}
