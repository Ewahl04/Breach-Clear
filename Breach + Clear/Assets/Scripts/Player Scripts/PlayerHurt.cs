using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHurt : MonoBehaviour
{

    [Header("Hurt Effect")]
    public RawImage hurtOverlay;
    public float hurtEffectDuration = 1.0f;
    public float effectPeak = 1f;
    private Color tempColor;
    private float currentAlpha;
    private bool hurtToggle;

    // Start is called before the first frame update
    void Start()
    {
        hurtOverlay.GetComponent<RawImage>();
        tempColor = hurtOverlay.color;
        hurtToggle = true;
    }


    // Update is called once per frame
    void Update()
    {
        hurtOverlay.color = tempColor;
        tempColor.a = currentAlpha;
        if (Input.GetKeyDown("g"))
        {
            Hurt();
        }
    }

    void Hurt()
    {
        hurtToggle = !hurtToggle;
        StartCoroutine(DamageOverlay(2, effectPeak, hurtToggle));
        Debug.Log(hurtToggle);
    }

    IEnumerator DamageOverlay(float duration, float strength, bool currentToggle)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            currentAlpha = Mathf.Lerp(strength, 0, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            if (currentToggle != hurtToggle)
            {
                yield break;
            }
            yield return null;
        }
        currentAlpha = 0f;
    }
}
