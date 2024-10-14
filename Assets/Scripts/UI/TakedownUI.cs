using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class TakedownUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Text text;
    [SerializeField] private float destroyDelay;
    [SerializeField] private float opacityAnimationDuration;

    public void Init(string value)
    {
        text.text = value;
        StartCoroutine(DelayedDestroy());
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(destroyDelay);

        Color originalColor = text.color;
        float elapsedTime = 0f;

        while (elapsedTime < opacityAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(1f, 0f, elapsedTime / opacityAnimationDuration);
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            yield return null;
        }

        text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        Destroy(gameObject);
    }
}
