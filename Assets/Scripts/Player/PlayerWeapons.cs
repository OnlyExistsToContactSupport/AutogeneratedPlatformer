using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public enum WeaponType
    {
        Punch,
        Sword,
        Gun
    }
    // Arma que o jogador tem
    private WeaponType activeWeapon;
    
    // Start is called before the first frame update
    void Start()
    {
        activeWeapon = WeaponType.Punch;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveWeapon(WeaponType type)
    {
        activeWeapon = type;
    }


    public WeaponType GetActiveWeapon()
    {
        return activeWeapon;
    }
}
