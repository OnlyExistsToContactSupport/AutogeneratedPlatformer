using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Boss : MonoBehaviour, Interactable
{
    public GameObject goodBuffEffect;
    public GameObject randomBuffEffect; // Temporário
    public GameObject debuffEffect;

    private Outline highlight;

    private bool hasGeneratedBuff;

    private DialogueController dialogueController;

    private ActiveBuffs.BuffType buff;

    // Start is called before the first frame update
    void Start()
    {
        if(highlight == null)
        {
            highlight = GetComponent<Outline>();
            highlight.enabled = false;
        }
        hasGeneratedBuff = false;
        dialogueController = FindObjectOfType<DialogueController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (highlight != null)
                highlight.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (highlight != null)
                highlight.enabled = false;
        }
    }

    public void Interact()
    {
        if(!hasGeneratedBuff)
        {
            hasGeneratedBuff = true;

            buff = ActiveBuffs.GenerateBuff();

            int buffValue = (int)buff;

            // Instanciar efeito
            if (buffValue <= 5)
            {
                if (goodBuffEffect != null)
                {
                    GameObject effect = Instantiate(goodBuffEffect, transform.position, Quaternion.identity);
                    effect.transform.SetParent(gameObject.transform, false);
                }

            }
            else if (buffValue >= 6 && buffValue <= 10)
            {
                if (randomBuffEffect != null)
                {
                    GameObject effect = Instantiate(randomBuffEffect, transform.position, Quaternion.identity);
                    effect.transform.SetParent(gameObject.transform, false);
                }

            }
            else // value > 10
            {
                if (debuffEffect != null)
                {
                    GameObject effect = Instantiate(debuffEffect, transform.position, Quaternion.identity);
                    effect.transform.SetParent(gameObject.transform, false);
                }

            }
        }

        dialogueController.GetMessage(buff);
    }
}
