using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatePlatform : MonoBehaviour
{
    private float blinkDuration = 1.5f;
    private float disappearTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {    
        blinkDuration = 1.5f;
        disappearTime = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Blink();
    }
    private void Blink()
    {
        if (Random.Range(1, 1000) <= 10)
        {
            StartCoroutine(Disappear(disappearTime, blinkDuration));
        }
    }
    IEnumerator Disappear(float disappearTime, float blinkDuration)
    {
        Material material;
        Color originalColor;
        //float fadeOutTime = 0f;
        float elapsedTime = 0f;
        //float blinkDuration = 1.0f;

        material = GetComponent<MeshRenderer>().material;


        Debug.Log("Material name = " + material.name + " With the value to String:" + material.ToString());

        originalColor = material.color;
        while (elapsedTime < blinkDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (blinkDuration / 2);
            Color newColor = Color.Lerp(originalColor, Color.black, t);
            material.color = newColor;
            yield return null;
        }
        material.color = Color.black;

        elapsedTime = 0f;
        while (elapsedTime < blinkDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (blinkDuration / 2);
            Color newColor = Color.Lerp(Color.black, originalColor, t);
            material.color = newColor;
            yield return null;
        }
        material.color = originalColor;

        gameObject.SetActive(false);

        // Wait for the specified time
        yield return new WaitForSeconds(disappearTime);

        // Enable the platform
        gameObject.SetActive(true);
    }
}
