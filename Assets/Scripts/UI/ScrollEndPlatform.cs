using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollEndPlatform : MonoBehaviour
{
    private float scrollSpeed;
    private MeshRenderer meshRenderer;
    public bool directionX;
    // Start is called before the first frame update
    void Start()
    {
        scrollSpeed = 0.1f;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(directionX)
        {
            meshRenderer.material.mainTextureOffset += new Vector2(scrollSpeed * Time.deltaTime, 0);
        }
        else
        {
            meshRenderer.material.mainTextureOffset += new Vector2(0, scrollSpeed * Time.deltaTime);
        }
    }
}
