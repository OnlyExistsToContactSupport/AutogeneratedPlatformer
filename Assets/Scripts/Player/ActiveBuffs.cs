using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ActiveBuffs
{
    public enum BuffType
    {
        // Buffs
        MaiorSalto = 0,
        MaiorVelocidade = 1,
        CurarVida = 2,
        MaiorDano = 3,
        MuniçãoInfinita = 4,
        ChanceNaoGastarMunição = 5,

        // Debuffs
        MenorSalto = 6,
        MenorVelocidade = 7,
        PerderVida = 8,
        MenorDano = 9,
        MuniçãoCustaDobro = 10,

        // Controlo
        NoBuff = 11
    }

    private static List<BuffType> activeBuffs = new List<BuffType>();

    public static BuffType GenerateBuff()
    {
        /* Probabilidade:
         * Bom (dura o nível inteiro): 30%
         * Bom/Mau mas temporário: 40%
         * Mau (dura o nível inteiro): 30%
         * 
        */

        var rand = new System.Random();
        int value = rand.Next(0, 100);
        BuffType buff;

        // Buscar qual o buff
        if (value < 30)
        {
            buff = (BuffType)rand.Next(0, 5);
        }
        else if (value >= 30 && value < 70)
        {
            buff = (BuffType)rand.Next(0, 10);
        }
        else // value >= 70
        {
            buff = (BuffType)rand.Next(6, 10);
        }

        activeBuffs.Add(buff);

        // Realizar o efeito do buff / debuff
        ParseBuff();

        return buff;
    }
    public static void ParseBuff()
    {
        foreach (BuffType buff in activeBuffs)
        {
            switch (buff)
            {
                case BuffType.MaiorSalto:
                    // Multiplica o salto do jogador por 2
                    PlayerStats.jumpForce *= 2;
                    break;

                case BuffType.MaiorVelocidade:
                    // Multiplica a velocidade do jogador por 2
                    PlayerStats.walkSpeed *= 2;
                    PlayerStats.runSpeed *= 2;
                    break;

                case BuffType.CurarVida:
                    // Devolve a vida perdida ao jogador
                    GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>().ResetHealth();
                    break;

                case BuffType.MaiorDano:
                    // Aumenta o dano dado
                    PlayerStats.punchDamage *= 2;
                    PlayerStats.swordDamage *= 2;
                    PlayerStats.gunDamage *= 2;
                    break;

                case BuffType.MuniçãoInfinita:
                    PlayerStats.hasInfiniteAmmo = true;
                    break;

                case BuffType.ChanceNaoGastarMunição:
                    PlayerStats.hasChanceToNotSpendAmmo = true;
                    break;

                case BuffType.MenorSalto:
                    // Diminui o salto do jogador
                    PlayerStats.jumpForce -= 5;
                    break;

                case BuffType.MenorVelocidade:
                    // Diminui a velocidade do jogador
                    PlayerStats.walkSpeed -= 5;
                    PlayerStats.runSpeed -= 5;
                    break;

                case BuffType.PerderVida:
                    // Faz o jogador perder vida em percentagem - 10%
                    GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>().TakeDamageInPercentage(10);
                    break;

                case BuffType.MenorDano:
                    // Diminui o dano dado
                    PlayerStats.punchDamage /= 2;
                    PlayerStats.swordDamage /= 2;
                    PlayerStats.gunDamage /= 2;
                    break;

                case BuffType.MuniçãoCustaDobro:
                    // Dobra o custo de uma bala
                    PlayerStats.ammoCost *= 2;
                    break;

                case BuffType.NoBuff:
                    // Não acontece nada
                    break;

            }
        }
    }
    public static void ResetBuffs()
    {
        activeBuffs = new List<BuffType>();
    }
}
