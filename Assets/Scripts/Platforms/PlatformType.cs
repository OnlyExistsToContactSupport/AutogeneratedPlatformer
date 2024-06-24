using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformTypes : ScriptableObject
{
    public GameObject basePlatform;
    public GameObject grassPlatform;
    public GameObject waterPlatform;
    public GameObject firePlatform;
    public GameObject endPlatform;

    public void LoadPlatforms()
    {
        basePlatform = Resources.Load("Platforms/BasePlatform").GameObject();
        grassPlatform = Resources.Load("Platforms/GrassPlatform").GameObject();
        waterPlatform = Resources.Load("Platforms/WaterEnemyPlatform").GameObject();
        firePlatform = Resources.Load("Platforms/FirePlatform").GameObject();
        endPlatform = Resources.Load("Platforms/EndPlatform").GameObject();
    }
}
