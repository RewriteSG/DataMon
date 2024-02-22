using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    public float fadeDuration = 2f;
    private Image image;
    private Color originalColor;

    void Start()
    {
        image = GetComponent<Image>();
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
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        Destroy(gameObject);
    }
}
