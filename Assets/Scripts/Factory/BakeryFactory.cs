using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeryFactory : ScriptableObject
{
    public GameObject GenerateBakery()
    {
        return Instantiate(Resources.Load("Platforms/WalkableSpaceBakery"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
    }
}
