using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformTypes : MonoBehaviour
{
    public GameObject grassPlatform;
    public GameObject waterPlatform;
    public GameObject firePlatform;

    private void Start()
    {
        grassPlatform = Resources.Load("EndPlatform").GameObject();
    }
}
