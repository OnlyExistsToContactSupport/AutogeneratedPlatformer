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
        Muni��oInfinita = 4,
        ChanceNaoGastarMuni��o = 5,

        // Debuffs
        MenorSalto = 6,
        MenorVelocidade = 7,
        PerderVida = 8,
        MenorDano = 9,
        Muni��oCustaDobro = 10,

        // Controlo
        NoBuff = 11
    }

    private static List<BuffType> activeBuffs = new List<BuffType>();

    public static BuffType GenerateBuff()
    {
        /* Probabilidade:
         * Bom (dura o n�vel inteiro): 30%
         * Bom/Mau mas tempor�rio: 40%
         * Mau (dura o n�vel inteiro): 30%
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

        return buff;
    }

}
