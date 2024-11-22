using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float AniTime = 0.5f;

    public float ScaleAmount = 0.6f;

    public GameObject mainMenu;
    public GameObject poseNetMenu;
   public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.scale(gameObject, new Vector3(ScaleAmount, ScaleAmount, ScaleAmount), AniTime).setEase(LeanTweenType.easeOutElastic);
    }

    public void OnPointerExit(PointerEventData eventData) 
    {
        LeanTween.scale(gameObject, new Vector3(0.5f, 0.5f, 0.5f) , AniTime).setEase(LeanTweenType.easeOutElastic);
    }

    public void Quit()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void Play()
    {
        //Disable Main Menu
        mainMenu.SetActive(false);

        //Enable PoseNet
        poseNetMenu.SetActive(true);

    }
}
