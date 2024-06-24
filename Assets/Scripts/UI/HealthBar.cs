using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // Barra da vida
    public RectTransform overlayBar;
    
    // Scale inicial da barra com vida ao máximo
    public float sizeMin;
    // Scale final da barra sem vida
    public float sizeMax;

    public void InitHealthBar(float minSize, float maxSize)
    {
        sizeMin = minSize;
        sizeMax = maxSize;
    }

    public void TakeDamage(float dano)
    {
        // Para perder vida em percentagem precisamos de fazer a conta:
        // O dano é calculado de 0% a 100%, e como 100% da vida é 1006, temos:
        float percentagemDano = (dano / 100) * sizeMax;

        Debug.Log("dano: " + percentagemDano);

        overlayBar.sizeDelta = new Vector2(overlayBar.sizeDelta.x + percentagemDano, overlayBar.sizeDelta.y);
    }
    public void TakeDamageInPercentage(float dano)
    {
        // Para perder vida em percentagem precisamos de fazer a conta:
        // O dano é calculado de 0% a 100%, e como 100% da vida é 1006, temos:
        float percentagemDano = (dano / 100) * sizeMax;

        Debug.Log("dano: " + percentagemDano);

        overlayBar.sizeDelta = new Vector2(overlayBar.sizeDelta.x + percentagemDano, overlayBar.sizeDelta.y);
    }
    public bool VerificarMorte()
    {
        return overlayBar.sizeDelta.x >= sizeMax;
    }
    public void ResetHealth()
    {
        overlayBar.sizeDelta = new Vector2(sizeMin, overlayBar.sizeDelta.y);
    }
    
}