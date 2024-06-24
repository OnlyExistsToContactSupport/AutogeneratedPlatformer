using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformTypes : ScriptableObject
{
    public GameObject grassPlatform;
    public GameObject waterPlatform;
    public GameObject firePlatform;
    public GameObject ceilingPlatform;
    public GameObject endPlatform;

    public void LoadPlatforms()
    {
        grassPlatform = Resources.Load("Platforms/EndPlatform").GameObject();
        waterPlatform = Resources.Load("Platforms/EndPlatform").GameObject();
        firePlatform = Resources.Load("Platforms/EndPlatform").GameObject();
        ceilingPlatform = Resources.Load("Platforms/EndPlatform").GameObject();
        endPlatform = Resources.Load("Platforms/EndPlatform").GameObject();
    }
}
