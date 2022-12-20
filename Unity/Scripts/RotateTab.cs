using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateTab : MonoBehaviour
{
    enum PlayStates { PRE, START, PLAY, RESULT, PHYSICS };
    PlayStates currentState;

    public float energyLevel; // higher means more full
    private float foodToEat;
    
    public Text textBox;
    public Text energyText;
    public Text debugText;

    public GameObject hinge;
    public GameObject canTab;
    public GameObject stateSetter;
    public Sprite[] catSpriteArray;
    public Sprite[] tunaSpriteArray;

    public Image catImg;
    public Image tunaImg;

    private float lastAngle;
    private float angle;
    public float timeLimit;
    private float timeLastMeasured;
    private float timeLastMeasuredHunger;
    private float angleFromState;
    private int lastButtonState;

    private Vector3 originalPos;

    void Start()
    {
        currentState = PlayStates.PRE;
        // energyLevel = 30.0f;
        foodToEat = 5f;
        originalPos = catImg.rectTransform.position;
    }

    void UpdateHungerLevel() {
        if (foodToEat != 0)
        {
            foodToEat -= 1f;
            energyLevel += 1f;
        }

        Mathf.Clamp(energyLevel, 10, 100);
        energyText.text = string.Format("Energy: {0}%", energyLevel);
    }
    
    void Update()
    {
        angle = Mathf.Lerp(10, -90, SerialCommunication.encoderValue / 100);

        if (Time.time - timeLastMeasuredHunger > 60)
        {
            energyLevel -= 1f;
            Mathf.Clamp(energyLevel, 10, 100);
            energyText.text = string.Format("Energy: {0}%", energyLevel);
            timeLastMeasuredHunger = Time.time;
        }

        if (currentState == PlayStates.PRE) {
            if (foodToEat != 0)
            {
                UpdateHungerLevel();
            } 
            else
            {
                if (energyLevel < 30) { catImg.sprite = catSpriteArray[6]; }
                else if (energyLevel > 70) { catImg.sprite = catSpriteArray[5]; }
                else { catImg.sprite = catSpriteArray[4]; }
                tunaImg.color = new Color(1, 1, 1, 0);
                currentState = PlayStates.START;
            }
        } 
        else if (currentState == PlayStates.START)
        {
            textBox.text =
                "Help PhiPhi open a tuna can!\n" +
                "Push the <color=red>red button</color> to begin.\n" +
                "Press the <b>M key</b> to learn physics.";
            if (SerialCommunication.isButtonUp == 0 && lastButtonState != 0)
            { 
                timeLastMeasured = Time.time;
                // create new position
                stateSetter.transform.localEulerAngles = new Vector3(0, Random.Range(-90, 10), 0);
                currentState = PlayStates.PLAY;
                lastButtonState = 0;
            }
            if (Input.GetKeyDown(KeyCode.M)) currentState = PlayStates.PHYSICS;
        } 
        else if (currentState == PlayStates.PLAY)
        {
            if (Time.time - timeLastMeasured < timeLimit)
            {
                angleFromState = Vector3.Angle(stateSetter.transform.right, hinge.transform.right);

/*                float excitementLevel = Mathf.Sqrt((100 - angleFromState));

                if (catImg.rectTransform.position != originalPos) catImg.rectTransform.position = originalPos;
                else catImg.rectTransform.position = new Vector3(
                    originalPos.x + Random.Range(-excitementLevel, excitementLevel), 
                    originalPos.y + Random.Range(-excitementLevel, excitementLevel), 
                    0);*/

                textBox.text =
                  "Rotate the tab to the correct angle!\n" +
                  "Look at PhiPhi's expression for help!\n" +
                  string.Format("You have {0:0.##}s remaining.", timeLimit - (Time.time - timeLastMeasured));
                // canTab.transform.RotateAround(hinge.transform.position, hinge.transform.up, angle - lastAngle);
                // hinge.transform.Rotate(new Vector3(0, angle - lastAngle, 0));
                hinge.transform.localEulerAngles = new Vector3(0, angle, 0);

                if (angleFromState < 20) catImg.sprite = catSpriteArray[3];
                else if (angleFromState >= 20 && angleFromState <= 50) catImg.sprite = catSpriteArray[4];
                else if (angleFromState > 50) catImg.sprite = catSpriteArray[1];

                debugText.text = string.Format("Difference: {0}", angleFromState);

            } else
            {
                float randNumber = Random.Range(0, 100f);
                // float randAngle = Mathf.Lerp(-90f, 10f, randNumber);

                if (angleFromState < randNumber)
                {
                    foodToEat += 5;
                    catImg.sprite = catSpriteArray[0];
                    tunaImg.sprite = tunaSpriteArray[0];
                    tunaImg.color = new Color(1, 1, 1, 1);
                }
                else
                {
                    // energyLevel -= 5;
                    // foodToEat = 0;
                    // energyLevel -= 5;
                    catImg.sprite = catSpriteArray[2];
                    tunaImg.sprite = tunaSpriteArray[1];
                    tunaImg.color = new Color(1, 1, 1, 1);
                }

                currentState = PlayStates.RESULT;
            }    
        } 
        else if (currentState == PlayStates.RESULT)
        {
            float prob = 100 - Mathf.Lerp(0f, 100f, angleFromState / 100);
            textBox.text =
                "Nice try!\n" +
                string.Format("You had a {0:0.##}% chance of opening the can from this position!\n", prob) +
                "Press the button to go back.";
            if (SerialCommunication.isButtonUp == 0 && lastButtonState != 0)
            { 
                currentState = PlayStates.PRE;
                lastButtonState = 0;
            }
            // have to make sure button doesn't skip
            // if (Time.time - timeLastMeasured > 10) currentState = PlayStates.PRE;
        } 
        else if (currentState == PlayStates.PHYSICS)
        {
            textBox.text =
                "Ask Ding or Mashi if you want to learn\n" +
                "what all this has to do with physics!\n" +
                "Press the button to go back.";
            if (SerialCommunication.isButtonUp == 0 && lastButtonState != 0)
            {
                currentState = PlayStates.PRE;
                lastButtonState = 0;
            }
        }

        if (angle - lastAngle != 0) lastAngle = angle;
        lastButtonState = SerialCommunication.isButtonUp;
    }
}
