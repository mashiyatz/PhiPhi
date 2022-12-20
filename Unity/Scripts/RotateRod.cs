using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateRod : MonoBehaviour
{
    public int score;
    public Text scoreBox;
    public Text resultsBox;
    public GameObject stateSetter;
    
    public Sprite[] spriteArray;
    // using: https://www.freepik.com/free-vector/expression-emotion-concept-set-cat-character-diffetent-animal-emotions_18779007.htm

    public Image catImg;
    private bool justMeasured;
    private bool isMeasuring;
    private float timeLastMeasured;

    void Start()
    {
        timeLastMeasured = Time.time;
        resultsBox.text = "";
        scoreBox.text = "Score: 0";
        isMeasuring = false;
    }

    void Update()
    {
        if (!isMeasuring) resultsBox.text = "Press Start to begin feeding the cat!";

        if (!justMeasured)
        {
            if (isMeasuring) resultsBox.text = string.Format("{0:0.##}s remaining", 5 - (Time.time - timeLastMeasured));

            Vector3 orientation = new Vector3(SerialCommunication.pitch, SerialCommunication.yaw, SerialCommunication.roll);
            transform.eulerAngles = orientation;

            float angleFromZAxis = Vector3.Angle(stateSetter.transform.up, transform.up);
            if (angleFromZAxis > 90) Camera.main.backgroundColor = new Color(Mathf.Lerp(0, 1, (angleFromZAxis - 90) / 90) * 0.8f, 0, 0, 1);
            else if (angleFromZAxis < 90) Camera.main.backgroundColor = new Color(0, 0, Mathf.Lerp(1, 0, angleFromZAxis / 90 * 0.8f), 1);

            if (angleFromZAxis < 45) catImg.sprite = spriteArray[1];
            else if (angleFromZAxis >= 45 && angleFromZAxis <= 135) catImg.sprite = spriteArray[0];
            else if (angleFromZAxis > 135) catImg.sprite = spriteArray[3];

            if (Time.time - timeLastMeasured > 5 && isMeasuring)
            {
                float randNumber = Random.Range(0, 100f) / 100f;
                float randAngle = Mathf.Lerp(0f, 180f, randNumber);
                /// needs a more accurate calculation for the math! 
                /// probability works differently 

                // Debug.Log(angleFromZAxis);
                // Debug.Log(randNumber);
                // Debug.Log(randAngle);
                if (angleFromZAxis < randAngle)
                {
                    score += 1;
                    scoreBox.text = string.Format("Score: {0}", score);
                    Camera.main.backgroundColor = Color.blue;
                    catImg.sprite = spriteArray[4];
                }
                else
                {
                    Camera.main.backgroundColor = Color.red;
                    catImg.sprite = spriteArray[2];
                }
                justMeasured = true;
                timeLastMeasured = Time.time;

                float prob = 100 - Mathf.Lerp(0f, 100f, angleFromZAxis / 180); 
                resultsBox.text = string.Format("You had a {0:0.##}% chance of feeding the cat!", prob);
            }
        } else
        {
            if (Time.time - timeLastMeasured > 2)
            {
                justMeasured = false;
                timeLastMeasured = Time.time;
                resultsBox.text = "";
                Vector3 stateOrientation = new Vector3(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f));
                stateSetter.transform.eulerAngles = stateOrientation;
            }
        }
    }

    public void StartStopMeasuring()
    {
        isMeasuring = !isMeasuring;
        if (isMeasuring)
        {
            timeLastMeasured = Time.time;
            resultsBox.text = "";
        }
    }
}
