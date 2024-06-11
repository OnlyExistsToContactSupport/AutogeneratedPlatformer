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

        // Necessário verificar em todos, visto que o sistema de diálogo começa inativo
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
                dialogue = "Obtiveste a habilidade de poder correr mais rápido. Aproveita!";
                break;
            case ActiveBuffs.BuffType.CurarVida:
                dialogue = "Os deuses decidiram que a tua saúde é importante. A tua vida irá ser reposta a 100%";
                break;
            case ActiveBuffs.BuffType.MaiorDano:
                dialogue = "Obtiveste a habilidade de dar mais dano. Os teus inimigos irão cair mais facilmente.";
                break;
            case ActiveBuffs.BuffType.MuniçãoInfinita:
                dialogue = "Não tens de ter preocupar com munição, as tuas balas agora são infinitas. Dá-lhe cowboy!";
                break;
            case ActiveBuffs.BuffType.ChanceNaoGastarMunição:
                dialogue = "Terás uma chance de 50-50 de não gastar munição quando disparas uma arma. Boa sorte!";
                break;


            case ActiveBuffs.BuffType.MenorSalto:
                dialogue = "Perdeste a habilidade de saltar normalmente. O teu salto perdeu 50% da sua força.";
                break;
            case ActiveBuffs.BuffType.MenorVelocidade:
                dialogue = "Perdeste a habilidade de correr normalmente. Sentes-te cansado pelo que apenas conseguirás andar neste nível.";
                break;
            case ActiveBuffs.BuffType.PerderVida:
                dialogue = "Apanhas-te um vírus e não foste vacinado, perdes 50% da tua saúde.";
                break;
            case ActiveBuffs.BuffType.MenorDano:
                dialogue = "Esqueceste-te de ir ao ginásio este mês. Os teus ataques apenas terão 50% da sua efetividade. Os teus inimigos irão custar mais a cair.";
                break;
            case ActiveBuffs.BuffType.MuniçãoCustaDobro:
                dialogue = "O cartuxo da tua arma está partido. Irás gastar o dobro da munição com metade da efetividade.";
                break;

            // Controlo - ActiveBuffs.BuffType.NoBuff
            // Não deve passar aqui
            default:
                dialogue = "Não obtiveste um buff hoje. Má sorte";
                break;
        }

        npcTextBox.text = dialogue;
        dialogueParent.SetActive(true);
    }
    public void SkipMessage(bool buttonDown)
    {
        Debug.Log("Skip botão: " + buttonDown);
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
