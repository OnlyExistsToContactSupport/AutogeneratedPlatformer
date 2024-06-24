using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GrabWeapon : MonoBehaviour, Interactable
{
    private Outline highlight;
    public PlayerWeapons.WeaponType weaponType;
    // Start is called before the first frame update
    void Start()
    {
        if (highlight == null)
        {
            highlight = GetComponent<Outline>();
            highlight.enabled = false;
        }
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
        switch (weaponType)
        {
            case PlayerWeapons.WeaponType.Sword:
                PlayerWeapons.SetActiveWeapon(PlayerWeapons.WeaponType.Sword);
                break;
            case PlayerWeapons.WeaponType.Gun:
                PlayerWeapons.SetActiveWeapon(PlayerWeapons.WeaponType.Gun);
                break;
            default:
                PlayerWeapons.SetActiveWeapon(PlayerWeapons.WeaponType.Punch);
                break;
        }
    }
}
