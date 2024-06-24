using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    // Adicionar efeito dos buffs
    // Adicionar pontos
    // Adicionar munições e armas

    // Jogador
    public static float jumpForce;
    public static float runSpeed;
    public static float walkSpeed;
    public static float health;
    public static float punchDamage;
    public static float swordDamage;
    public static float gunDamage;
    public static float bullets;
    public static bool hasInfiniteAmmo;
    public static bool hasChanceToNotSpendAmmo;
    public static int ammoCost;

    public static int currentLevel;

    // Inicializado aqui para não levar reset
    public static int points = 0;

    // Controls
    public static int sensitivity;
    public static KeyCode interactKey;
    public static KeyCode jumpKey;
    public static KeyCode runKey;
    public static KeyCode attackKey;

    public static void ResetStats()
    {
        jumpForce = 15;
        runSpeed = 15;
        walkSpeed = 10;
        health = 1006;
        punchDamage = 100;
        swordDamage = 250;
        gunDamage = 400;
        bullets = 0;
        hasInfiniteAmmo = false;
        hasChanceToNotSpendAmmo = false;
        ammoCost = 1;

        currentLevel = 1;

        sensitivity = 400;
        interactKey = KeyCode.E;
        jumpKey = KeyCode.Space;
        runKey = KeyCode.LeftShift;
        attackKey = KeyCode.Mouse0;
    }
}
