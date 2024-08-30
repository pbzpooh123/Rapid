using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ColorBlock cb = button.colors;
        Color hoverColor = cb.normalColor;
        hoverColor.a = 0.3f; 
        cb.normalColor = hoverColor;
        button.colors = cb;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ColorBlock cb = button.colors;
        Color originalColor = cb.normalColor;
        originalColor.a = 1f; 
        cb.normalColor = originalColor;
        button.colors = cb;
    }
}
