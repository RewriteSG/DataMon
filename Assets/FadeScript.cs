using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    public float fadeDuration = 2f;
    private Image image;
    private Color originalColor;
    TMPro.TextMeshProUGUI TextMeshProUGUI;

    void Start()
    {
        image = GetComponent<Image>();
        if (image.isNull())
        {

            TextMeshProUGUI = GetComponent<TMPro.TextMeshProUGUI>();
            originalColor = TextMeshProUGUI.color;
        }
        else
            originalColor = image.color;
        Invoke("FadeOut", fadeDuration);
    }

    void FadeOut()
    {
        StartCoroutine(FadeImage());
    }

    IEnumerator FadeImage()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            if(!image.isNull())
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            else
                TextMeshProUGUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (!image.isNull())
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        else
            TextMeshProUGUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        Destroy(gameObject);
    }
}
