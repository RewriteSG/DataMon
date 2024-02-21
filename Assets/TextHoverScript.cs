using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextHoverScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    public float hoverScaleMultiplier = 1.2f;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ScaleTextOnHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetTextScale();
    }

    private void ScaleTextOnHover()
    {
        transform.localScale = originalScale * hoverScaleMultiplier;
    }

    private void ResetTextScale()
    {
        transform.localScale = originalScale;
    }
}
