using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CycleThroughInstructions : MonoBehaviour
{
    private enum InstructionStep { PUSH1, ROTATE, PUSH2, NumTypes }
    private InstructionStep currentInstructionStep;
    public Sprite[] pushImageList;
    public Sprite[] rotateImageList;
    public Image currentImage;
    public Text instructionTextBox;
    public Text instructionNumber;
    private string[] instructions;
    private float timeOfLastStateChange;
    private float timeOfLastFrameChange;
    private int imageIndex;

    public RotateCan rotateCan;

    void Start()
    {
        timeOfLastStateChange = 0;
        imageIndex = 0;
        timeOfLastFrameChange = 0;

        instructions = new string[3];
        instructions[0] = "Press to <color=#F7941D>Start</color>!";
        instructions[1] = "Rotate!";
        instructions[2] = "Push again to feed!";

        instructionTextBox.text = instructions[0];
        currentInstructionStep = InstructionStep.PUSH1;
    }

    void Update()
    {
       /* if (rotateCan.currentState == RotateCan.PlayStates.START)
        {
            instructionTextBox.gameObject.SetActive(true);
            instructionNumber.gameObject.SetActive(true);
            if (currentInstructionStep == InstructionStep.PUSH1)
            {
                if (Time.time - timeOfLastStateChange > 2)
                {
                    timeOfLastStateChange = Time.time;
                    timeOfLastFrameChange = Time.time;
                    imageIndex = 0;
                    currentInstructionStep = InstructionStep.ROTATE;
                    instructionTextBox.text = instructions[1];
                    instructionNumber.text = "2/3";
                }
                else if (Time.time - timeOfLastFrameChange >= 0.5f)
                {
                    imageIndex += 1;
                    if (imageIndex == pushImageList.Length) imageIndex = 0;
                    currentImage.sprite = pushImageList[imageIndex];
                    timeOfLastFrameChange = Time.time;
                }
            }
            else if (currentInstructionStep == InstructionStep.ROTATE)
            {
                if (Time.time - timeOfLastStateChange > 2)
                {
                    timeOfLastStateChange = Time.time;
                    timeOfLastFrameChange = Time.time;
                    imageIndex = 0;
                    currentInstructionStep = InstructionStep.PUSH2;
                    instructionTextBox.text = instructions[2];
                    instructionNumber.text = "3/3";
                }
                else if (Time.time - timeOfLastFrameChange >= 0.25f)
                {
                    imageIndex += 1;
                    if (imageIndex == rotateImageList.Length) imageIndex = 0;
                    currentImage.sprite = rotateImageList[imageIndex];
                    timeOfLastFrameChange = Time.time;
                }
            }
            else if (currentInstructionStep == InstructionStep.PUSH2)
            {
                if (Time.time - timeOfLastStateChange > 2)
                {
                    timeOfLastStateChange = Time.time;
                    timeOfLastFrameChange = Time.time;
                    imageIndex = 0;
                    currentInstructionStep = InstructionStep.PUSH1;
                    instructionTextBox.text = instructions[0];
                    instructionNumber.text = "1/3";
                }
                else if (Time.time - timeOfLastFrameChange >= 0.5f)
                {
                    imageIndex += 1;
                    if (imageIndex == pushImageList.Length) imageIndex = 0;
                    currentImage.sprite = pushImageList[imageIndex];
                    timeOfLastFrameChange = Time.time;
                }
            }
        }*/
        if (rotateCan.currentState == RotateCan.PlayStates.START)
        {
            instructionTextBox.gameObject.SetActive(true);
            instructionNumber.gameObject.SetActive(false);
            instructionTextBox.text = instructions[0];

            if (currentInstructionStep == InstructionStep.PUSH1)
            {
                if (Time.time - timeOfLastFrameChange >= 0.5f)
                {
                    imageIndex += 1;
                    if (imageIndex >= pushImageList.Length) imageIndex = 0;
                    currentImage.sprite = pushImageList[imageIndex];
                    timeOfLastFrameChange = Time.time;
                }
            }
            else currentInstructionStep = InstructionStep.PUSH1;
        }
        else if (rotateCan.currentState == RotateCan.PlayStates.PLAY)
        {
            instructionTextBox.gameObject.SetActive(false);
            instructionNumber.gameObject.SetActive(false);

            if (currentInstructionStep == InstructionStep.ROTATE)
            {
                if (Time.time - timeOfLastFrameChange >= 0.25f)
                {
                    imageIndex += 1;
                    if (imageIndex >= rotateImageList.Length) imageIndex = 0;
                    currentImage.sprite = rotateImageList[imageIndex];
                    timeOfLastFrameChange = Time.time;
                }
            }
            else currentInstructionStep = InstructionStep.ROTATE;

            /*if (currentInstructionStep == InstructionStep.PUSH1)
            {
                if (Time.time - timeOfLastStateChange > 2)
                {
                    timeOfLastStateChange = Time.time;
                    timeOfLastFrameChange = Time.time;
                    imageIndex = 0;
                    currentInstructionStep = InstructionStep.ROTATE;
                }
                else if (Time.time - timeOfLastFrameChange >= 0.5f)
                {
                    imageIndex += 1;
                    if (imageIndex == pushImageList.Length) imageIndex = 0;
                    currentImage.sprite = pushImageList[imageIndex];
                    timeOfLastFrameChange = Time.time;
                }
            }
            else if (currentInstructionStep == InstructionStep.ROTATE)
            {
                if (Time.time - timeOfLastStateChange > 2)
                {
                    timeOfLastStateChange = Time.time;
                    timeOfLastFrameChange = Time.time;
                    imageIndex = 0;
                    currentInstructionStep = InstructionStep.PUSH1;
                }
                else if (Time.time - timeOfLastFrameChange >= 0.25f)
                {
                    imageIndex += 1;
                    if (imageIndex == rotateImageList.Length) imageIndex = 0;
                    currentImage.sprite = rotateImageList[imageIndex];
                    timeOfLastFrameChange = Time.time;
                }
            }
            else currentInstructionStep = InstructionStep.PUSH1;*/
        }
    }
}
