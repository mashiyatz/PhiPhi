using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateRotation : MonoBehaviour
{
    public CycleThroughInstructions instructions;
    private Sprite[] rotateImageList;
    private float timeSinceChange;
    private int index;
    public Image currentImage;

    void Start()
    {
        rotateImageList = instructions.rotateImageList;
        timeSinceChange = Time.time;
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - timeSinceChange > 0.25f)
        {
            currentImage.sprite = rotateImageList[index];
            index += 1;
            if (index >= rotateImageList.Length) index = 0;
            timeSinceChange = Time.time;
        }
    }
}
