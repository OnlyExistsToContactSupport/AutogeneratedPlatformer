using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerWeapons
{
    public enum WeaponType
    {
        Punch,
        Sword,
        Gun
    }
    // Arma que o jogador tem
    private static WeaponType activeWeapon = WeaponType.Punch;

    // Dano em percentagem
    public static int punchDamage = 5;
    public static int swordDamage = 20;
    public static int gunDamage = 40;

    public static void SetActiveWeapon(WeaponType type)
    {
        activeWeapon = type;
        // Ativar a arma
        switch(type)
        {
            // Quando não tem arma
            case WeaponType.Punch:
                GameObject.FindGameObjectWithTag("Sword").gameObject.SetActive(false);
                GameObject.FindGameObjectWithTag("Gun").gameObject.SetActive(false);
                break;
            // Espada dentro da mão direita do jogador
            case WeaponType.Sword:
                GameObject.FindGameObjectWithTag("Sword").gameObject.SetActive(true);
                GameObject.FindGameObjectWithTag("Gun").gameObject.SetActive(false);
                break;
            // Pistola dentro da mão direita do jogador
            case WeaponType.Gun:
                GameObject.FindGameObjectWithTag("Sword").gameObject.SetActive(false);
                GameObject.FindGameObjectWithTag("Gun").gameObject.SetActive(true);
                break;
        }
    }


    public static WeaponType GetActiveWeapon()
    {
        return activeWeapon;
    }
}
