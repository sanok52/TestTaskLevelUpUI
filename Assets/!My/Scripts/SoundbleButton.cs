using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundbleButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.PlayClickButton();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlayOverButton();
    }
}