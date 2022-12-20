using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateCan : MonoBehaviour
{
    public enum PlayStates { PRE, START, PLAY, REVEAL, REPORT, PHYSICS };
    public PlayStates currentState;

    public float energyLevel; // higher means more full
    private float foodToEat;

    public Text textBox;
    public GameObject revealPanel;
    public GameObject reportPanel;
    public Text reportTextbox;
    public Image catResultImage;
    public GameObject physicsPanel;
    public Text physicsTextBox;
    public Slider probabilitySlider;
    public Text physicsPageNumberText;

    public AudioSource catAudioSource;
    public AudioClip[] catSounds;

    public GameObject instructionsDisplay;
    public RectTransform instructionDisplayTransform;
    public Vector2 instructionStartPosition;
    public Vector2 instructionPlayPosition;

    public Text quantumTransitionTextbox;

    public Slider longPressDisplaySlider;

    // public Text energyText;
    public Text debugText;
    public RectTransform hungerFill;

    public Sprite[] catSpriteArray;
    public RectTransform catTransform;
    public Sprite[] tunaSpriteArray;

    public Image catImg;
    public float catSpeed;

    public float angle;
    public float timeLimit;
    public float longPressDuration;
    public float pressTimeThreshold;
    private float timeLastMeasuredHunger;
    private float angleFromState;
    
    private Vector2 originalPos;
    private int goodStateAngle;

    private int buttonState;
    private int lastButtonState;
    private float timeAtButtonPress;
    private float holdTime;
    private bool isButtonJustReleased;

    private string[] physicsTextStrings;
    public int physicsStringIndex;

    private float prob;

    public Sprite[] revealImageList;
    public Image canImage;
    public Text revealCanTextbox;
    private float timeAtStateChange;
    private bool tunaExists; 

    void Start()
    {
        currentState = PlayStates.PRE;
        foodToEat = 5f;
        originalPos = catImg.rectTransform.anchoredPosition;
        buttonState = 1; // 1 means switch not pressed
        lastButtonState = 1;
        holdTime = 0;
        revealPanel.SetActive(false);
        reportPanel.SetActive(false);
        physicsPanel.SetActive(false);

        physicsTextStrings = new string[9];
        physicsTextStrings[0] =
            "WhatÅfs inside the tuna can? Is it filled with delicious tuna? Or maybe itÅfs totally empty?";
        physicsTextStrings[1] =
            "In the quantum world, we would say that until we open the can, both statements are true! We call this <color=#F7941D>quantum superposition</color>.";
        physicsTextStrings[2] =
            "But when we check whatÅfs inside, itÅfs not like we can see both ÅgfishÅh and Ågno fishÅh at the same time. We only see one or the other. Why is that?";
        physicsTextStrings[3] =
            "An object in the quantum world exists as every state it can possibly be at once.";
        physicsTextStrings[4] =
            "But when you make an <color=#F7941D>observation</color> of that object, you have a probability of seeing it in only one of its possible states.";
        physicsTextStrings[5] =
            "Physicists have a fancy name for what happens when we make this observation: <color=#F7941D>quantum collapse</color>!";
        physicsTextStrings[6] =
            "Even though you might not know what youÅfll see inside the can, in this game, you can change the probability by rotating it."; 
        physicsTextStrings[7] =
            "Physicists do the exactly same thing to real quantum objects through a phenomenon called <color=#F7941D>interference</color>.";
        physicsTextStrings[8] =
            "In quantum computing, you can change probabilities by applying <color=#F7941D>gates</color> to qubits, or quantum bits. Take Introduction to Quantum Technologies next semester to learn more!";
    }

    void UpdateHungerLevel()
    {
        if (foodToEat != 0)
        {
            energyLevel += foodToEat;
            foodToEat = 0;
        }

        Mathf.Clamp(energyLevel, 10, 100);
        hungerFill.anchoredPosition = new Vector2(0, Mathf.Lerp(-25, 45, energyLevel / 100)); 

        if (energyLevel < 30) { catImg.sprite = catSpriteArray[6]; }
        else if (energyLevel > 70) { catImg.sprite = catSpriteArray[5]; }
        else { catImg.sprite = catSpriteArray[4]; }

    }

    private void ResetTextbox()
    {
        /*textBox.text =
        "PhiPhi wants a snack!\n" +
        "But these quantum cans only have fresh tuna if they're turned the right way...\n" +
        "Let's help PhiPhi out!\n" +
        "   1. <color=blue>Press</color> the quantum can to start.\n" +
        "   2. <color=blue>Rotate</color> the can. Make sure to watch PhiPhi's reaction!\n" +
        "   3. <color=blue>Press</color> the can again to open it for PhiPhi.\n";*/

        textBox.text = "Let's feed PhiPhi quantum tuna!";
    }

    void Update()
    {
        lastButtonState = buttonState;
        buttonState = SerialCommunication.isButtonUp;

        int encoderValueCorrected = (SerialCommunication.encoderValue < 0 ? SerialCommunication.encoderValue + 24 : SerialCommunication.encoderValue);
        angle = Mathf.Lerp(0f, 360f, encoderValueCorrected / 24f);

        if (buttonState != lastButtonState)
        {
            if (buttonState == 0)
            {
                timeAtButtonPress = Time.time;
                isButtonJustReleased = false;
            }
            else if (buttonState == 1)
            {
                holdTime = Time.time - timeAtButtonPress;
                isButtonJustReleased = true;
            }
        } else
        {
            if (buttonState == 0) holdTime = Time.time - timeAtButtonPress;
            else holdTime = 0;
            isButtonJustReleased = false;
        }

/*        if (Time.time - timeLastMeasuredHunger > 30)
        {
            energyLevel -= 1f;
            Mathf.Clamp(energyLevel, 10, 100);
            hungerFill.anchoredPosition = new Vector2(0, Mathf.Lerp(-25, 45, energyLevel / 100));
            timeLastMeasuredHunger = Time.time;
        }*/

        // state machine
        if (currentState != PlayStates.START && currentState != PlayStates.PHYSICS) longPressDisplaySlider.value = 0;
        if (currentState != PlayStates.PRE && currentState != PlayStates.START && currentState != PlayStates.PLAY) instructionsDisplay.SetActive(false);

        if (currentState == PlayStates.PRE)
        {
            // UpdateHungerLevel();
            // tunaImg.color = new Color(1, 1, 1, 0);
            // quantumTransitionTextbox.text = "<color=#F7941D>Long press</color> the can to learn quantum physics!";
            quantumTransitionTextbox.gameObject.SetActive(true);
            instructionsDisplay.SetActive(true);
            ResetTextbox(); // move to pre
            timeAtStateChange = Time.time;
            currentState = PlayStates.START;
        }
        else if (currentState == PlayStates.START)
        {
            if (Time.time - timeAtStateChange > 16)
            {
                catAudioSource.PlayOneShot(catSounds[(int)Random.Range(0, catSounds.Length - 1)]);
                timeAtStateChange = Time.time;
            }

            if (isButtonJustReleased && holdTime < pressTimeThreshold)
            {
                goodStateAngle = (int)Random.Range(0, 360f);
                quantumTransitionTextbox.gameObject.SetActive(false);

                currentState = PlayStates.PLAY;
            }
/*            else if (isButtonJustReleased && holdTime >= longPressDuration)*/
            else if (holdTime < pressTimeThreshold)
            {
                quantumTransitionTextbox.text = "...or <color=#F7941D>long press</color> to learn quantum physics!";
                longPressDisplaySlider.value = 0;
            }
            else if (holdTime >= pressTimeThreshold)
            {
                quantumTransitionTextbox.text = "Let's learn quantum!";
                if (holdTime < longPressDuration) longPressDisplaySlider.value = Mathf.Lerp(0, 1, (holdTime - pressTimeThreshold) / (longPressDuration - pressTimeThreshold));
                else if (isButtonJustReleased && holdTime >= longPressDuration)
                {
                    timeAtButtonPress = Time.time; // reset count
                    physicsPanel.SetActive(true);
                    physicsStringIndex = 0;
                    currentState = PlayStates.PHYSICS;
                }
            }
            // else 
        }
        else if (currentState == PlayStates.PLAY)
        {
            angleFromState = Mathf.Abs(goodStateAngle - angle);
            if (angleFromState > 180) angleFromState = 360 - angleFromState; 

            textBox.text =
                "<color=#F7941D>Rotate</color> the can, little by little. PhiPhi will come closer the more likely there's tuna inside!\n\n" +
                "<color=#F7941D>Push</color> the can again to open it.";

/*            instructionDisplayTransform.anchoredPosition = Vector2.MoveTowards(instructionDisplayTransform.anchoredPosition, new Vector2(-270, -80), Time.deltaTime * 200);*/
            instructionDisplayTransform.anchoredPosition = Vector2.MoveTowards(instructionDisplayTransform.anchoredPosition, instructionPlayPosition, Time.deltaTime * 200);

            /*if (angleFromState < 30) catImg.sprite = catSpriteArray[3];
            else if (angleFromState >= 30 && angleFromState <= 90) catImg.sprite = catSpriteArray[4];
            else if (angleFromState > 90) catImg.sprite = catSpriteArray[1];*/

            if (angleFromState < 36) catImg.sprite = catSpriteArray[0];
            else if (angleFromState >= 36 && angleFromState <= 72) catImg.sprite = catSpriteArray[3];
            else if (angleFromState >= 72 && angleFromState <= 108) catImg.sprite = catSpriteArray[4];
            else if (angleFromState >= 108 && angleFromState <= 144) catImg.sprite = catSpriteArray[1];
            else if (angleFromState > 144) catImg.sprite = catSpriteArray[2];

            // debugText.text = string.Format("Difference: {0}", angleFromState);

            catTransform.anchoredPosition = Vector3.MoveTowards(
                    catTransform.anchoredPosition,
                    new Vector3(Mathf.Lerp(50, 250, angleFromState / 180), Mathf.Lerp(-60, 20, angleFromState / 180), 0),
                    Time.deltaTime * catSpeed 
                );

            catTransform.localScale = new Vector3(
                Mathf.Lerp(0.5f, 1.2f, (250 - catTransform.anchoredPosition.x) / 200),
                Mathf.Lerp(0.5f, 1.2f, (250 - catTransform.anchoredPosition.x) / 200),
                1
                );
            
            if (isButtonJustReleased && holdTime < longPressDuration)
            {
                float randRoll = Random.Range(0, 180f);

                if (angleFromState < randRoll)
                {
                    // foodToEat += 5;
                    // catImg.sprite = catSpriteArray[0]; // using these in stages instead
                    // tunaImg.sprite = tunaSpriteArray[0];
                    catResultImage.sprite = catSpriteArray[5];
                    tunaExists = true;
                }
                else
                {
                    // catImg.sprite = catSpriteArray[2];
                    // tunaImg.sprite = tunaSpriteArray[1];
                    catResultImage.sprite = catSpriteArray[6];
                    tunaExists = false;
                }

                // tunaImg.color = new Color(1, 1, 1, 1);
                catTransform.anchoredPosition = originalPos;
                catTransform.localScale = new Vector3(1, 1, 1);

                // reportPanel.SetActive(true);
                revealPanel.SetActive(true);
                ResetTextbox();
                catImg.sprite = catSpriteArray[4];
                currentState = PlayStates.REVEAL;

                instructionDisplayTransform.anchoredPosition = instructionStartPosition;

                timeAtStateChange = Time.time;
            }
        }
        else if (currentState == PlayStates.REVEAL)
        {
            if (Time.time - timeAtStateChange > 3.6f)
            {
                prob = 1 - (angleFromState / 180);
                probabilitySlider.value = prob;

                reportPanel.SetActive(true);
                revealPanel.SetActive(false);
                currentState = PlayStates.REPORT;
                canImage.sprite = revealImageList[0];
                timeAtStateChange = Time.time;
                // canImage.rectTransform.localScale = new Vector2(1f, 1f);
            }
            else if (Time.time - timeAtStateChange > 2.4f)
            {
                if (tunaExists)
                {
                    canImage.sprite = revealImageList[2];
                    revealCanTextbox.text = "Yum! Delicious tuna!";
                }
                else
                {
                    canImage.sprite = revealImageList[3];
                    revealCanTextbox.text = "Oh no...";
                }
                // canImage.transform.localScale = new Vector2(2f, 2f);
            }
            else if (Time.time - timeAtStateChange > 1.2f)
            {
                canImage.sprite = revealImageList[1];
                // canImage.transform.localScale = new Vector2(1.5f, 1.5f);
                revealCanTextbox.text = "What's this?!";
            }
            else 
            {
                canImage.sprite = revealImageList[0];
                // canImage.transform.localScale = new Vector2(1f, 1f);
                revealCanTextbox.text = "Let's open the can!";
                // drum roll
            }

        }
        else if (currentState == PlayStates.REPORT)
        {

            /*prob = 1 - (angleFromState / 180);
            probabilitySlider.value = prob;*/
            reportTextbox.text =
                string.Format("The can had a {0}% chance of having fresh tuna!\n", (int)(prob * 100));
            if (Time.time - timeAtStateChange > 1.2f)
            {
                reportTextbox.text =
                string.Format("The can had a {0}% chance of having fresh tuna!\n", (int)(prob * 100)) +
                "Press the can to go back.";
                if (isButtonJustReleased && holdTime < longPressDuration)
                {
                    reportTextbox.text = "";
                    currentState = PlayStates.PRE;
                    reportPanel.SetActive(false);
                }
            }
        }
        else if (currentState == PlayStates.PHYSICS)
        {
            physicsTextBox.text = physicsTextStrings[physicsStringIndex];
            physicsPageNumberText.text = string.Format("Press to continue ({0}/{1})", physicsStringIndex + 1, physicsTextStrings.Length);

            // if (isButtonJustReleased && holdTime >= longPressDuration)
            if (isButtonJustReleased && holdTime < pressTimeThreshold)
            {
                physicsStringIndex += 1;
                if (physicsStringIndex > physicsTextStrings.Length - 1) physicsStringIndex = 0;
            }
            else if (holdTime < pressTimeThreshold)
            {
                quantumTransitionTextbox.text = "<color=#F7941D>Long press</color> to go back!"; ;
                longPressDisplaySlider.value = 0;
            }
            else if (holdTime >= pressTimeThreshold)
            {
                quantumTransitionTextbox.text = "Let's feed PhiPhi!";
                if (holdTime < longPressDuration) longPressDisplaySlider.value = Mathf.Lerp(0, 1, (holdTime - pressTimeThreshold) / (longPressDuration - pressTimeThreshold));
                else if (isButtonJustReleased && holdTime >= longPressDuration)
                {
                    timeAtButtonPress = Time.time;
                    quantumTransitionTextbox.text = "...or <color=#F7941D>long press</color> to learn quantum physics!";
                    currentState = PlayStates.PRE; // 
                    physicsPanel.SetActive(false);
                }
            }
        }
    }
}
