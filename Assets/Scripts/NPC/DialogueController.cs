using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    private GameObject dialogueParent;
    private TextMeshProUGUI npcTextBox;
    private Image textBoxBackground;
    public static bool isDialogue;
    // Start is called before the first frame update
    void Start()
    {
        isDialogue = false;

        // Necess�rio verificar em todos, visto que o sistema de di�logo come�a inativo
        dialogueParent = Resources.FindObjectsOfTypeAll<GameObject>().ToList().Where(x => x.tag.Equals("DialogueSystem")).FirstOrDefault();

        npcTextBox = dialogueParent.GetComponentInChildren<TextMeshProUGUI>();
        textBoxBackground = dialogueParent.GetComponentInChildren<Image>();

        dialogueParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckSkipMessage();
    }
    public void GetMessage(ActiveBuffs.BuffType buff)
    {
        isDialogue = true;
        string dialogue;

        switch (buff)
        {
            case ActiveBuffs.BuffType.MaiorSalto:
                dialogue = "Obtiveste a habilidade de poder saltar mais alto. Aproveita!";
                break;
            case ActiveBuffs.BuffType.MaiorVelocidade:
                dialogue = "Obtiveste a habilidade de poder correr mais r�pido. Aproveita!";
                break;
            case ActiveBuffs.BuffType.CurarVida:
                dialogue = "Os deuses decidiram que a tua sa�de � importante. A tua vida ir� ser reposta a 100%";
                break;
            case ActiveBuffs.BuffType.MaiorDano:
                dialogue = "Obtiveste a habilidade de dar mais dano. Os teus inimigos ir�o cair mais facilmente.";
                break;
            case ActiveBuffs.BuffType.Muni��oInfinita:
                dialogue = "N�o tens de ter preocupar com muni��o, as tuas balas agora s�o infinitas. D�-lhe cowboy!";
                break;
            case ActiveBuffs.BuffType.ChanceNaoGastarMuni��o:
                dialogue = "Ter�s uma chance de 50-50 de n�o gastar muni��o quando disparas uma arma. Boa sorte!";
                break;


            case ActiveBuffs.BuffType.MenorSalto:
                dialogue = "Perdeste a habilidade de saltar normalmente. O teu salto perdeu 50% da sua for�a.";
                break;
            case ActiveBuffs.BuffType.MenorVelocidade:
                dialogue = "Perdeste a habilidade de correr normalmente. Sentes-te cansado pelo que apenas conseguir�s andar neste n�vel.";
                break;
            case ActiveBuffs.BuffType.PerderVida:
                dialogue = "Apanhas-te um v�rus e n�o foste vacinado, perdes 50% da tua sa�de.";
                break;
            case ActiveBuffs.BuffType.MenorDano:
                dialogue = "Esqueceste-te de ir ao gin�sio este m�s. Os teus ataques apenas ter�o 50% da sua efetividade. Os teus inimigos ir�o custar mais a cair.";
                break;
            case ActiveBuffs.BuffType.Muni��oCustaDobro:
                dialogue = "O cartuxo da tua arma est� partido. Ir�s gastar o dobro da muni��o com metade da efetividade.";
                break;

            // Controlo - ActiveBuffs.BuffType.NoBuff
            // N�o deve passar aqui
            default:
                dialogue = "N�o obtiveste um buff hoje. M� sorte";
                break;
        }

        npcTextBox.text = dialogue;
        dialogueParent.SetActive(true);
    }
    public void SkipMessage(bool buttonDown)
    {
        Debug.Log("Skip bot�o: " + buttonDown);
        isDialogue = false;
        npcTextBox.text = "";
        dialogueParent.SetActive(false);

    }
    private void CheckSkipMessage()
    {
        if (isDialogue && Input.GetKeyDown(KeyCode.E))
            SkipMessage(false);
    }
}
