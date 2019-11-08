using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureAnimation : MonoBehaviour
{
    public Texture[] animTextures;
    public float animSpeed;

    public int currentFrame;
    public float frameTimer;

    new private Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        frameTimer += Time.deltaTime;
        if(frameTimer>=animSpeed)
        {
            frameTimer = 0;
            currentFrame++;
            if (currentFrame >= animTextures.Length) currentFrame = 0;
            renderer.material.mainTexture = animTextures[currentFrame]; //.SetTexture("frame" + currentFrame, animTextures[currentFrame]);
        }
    }
}
