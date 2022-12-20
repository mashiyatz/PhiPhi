using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicsAnimations : MonoBehaviour
{
    public RotateCan rotateCan;
    public Image physicsGraphic;
    public Image backgroundPhysicsGraphic;
    public Sprite[] physicsSpriteList;

    public GameObject slide3Object;
    public Image slide3good;
    public Image slide3bad;

    public GameObject slide8Object;
    public Image slide8a;
    public Image slide8b;
    public Image slide8c;
    public Image slide8d;

    public GameObject slide7Object;
    public Slider slider;

    public GameObject slide6Object;
    public Image slide6a;
    public Image slide6b;
    public Image slide6c;

    public GameObject slides45Object;
    public Image slides45a;
    public Image slides45b;
    public Image slides45c;
    public Image slides45d;
    public Image slides45e;

    public Text qiskitCaption;

    public GameObject rotateTextIndicator;

    // public GameObject rotateAnimationObject;

    private int physicsIndex;
    private float currentAngle;
    private float lastAngle;

    public int rotationSpeed;
    private float opacityValue;
    public List<int> interactiveSlides;

    void Start()
    {
        currentAngle = rotateCan.angle;
        lastAngle = rotateCan.angle;
    }

    // Update is called once per frame
    void Update()
    {
        physicsIndex = rotateCan.physicsStringIndex;
        lastAngle = currentAngle;
        currentAngle = rotateCan.angle;

        /*        if (interactiveSlides.Contains(physicsIndex)) rotateAnimationObject.SetActive(true);
                else rotateAnimationObject.SetActive(false);*/
        if (interactiveSlides.Contains(physicsIndex)) rotateTextIndicator.SetActive(true);
        else rotateTextIndicator.SetActive(false);

        if (physicsIndex != 0)
        {
            physicsGraphic.rectTransform.localEulerAngles = new Vector3(0, 0, 0);
        }
        if (physicsIndex != 1) backgroundPhysicsGraphic.gameObject.SetActive(false);
        if (physicsIndex != 2)
        {
            physicsGraphic.gameObject.SetActive(true);
            physicsGraphic.color = new Color(1, 1, 1, 1);
            slide3Object.SetActive(false);
        }
        if (physicsIndex != 3 && physicsIndex != 4)
        {
            physicsGraphic.gameObject.SetActive(true);
            slides45Object.SetActive(false);
        }
        if (physicsIndex != 5)
        {
            physicsGraphic.gameObject.SetActive(true);
            slide6Object.SetActive(false);
        }
        if (physicsIndex != 6)
        {
            physicsGraphic.gameObject.SetActive(true);
            slide7Object.SetActive(false);
        }
        if (physicsIndex != 7)
        {
            physicsGraphic.gameObject.SetActive(true);
            slide8Object.SetActive(false);
        }
        if (physicsIndex != 8)
        {
            qiskitCaption.gameObject.SetActive(false);
        }

        if (physicsIndex == 0)
        {
            physicsGraphic.sprite = physicsSpriteList[0];
            Debug.Log(string.Format("current: {0}, last: {1}", currentAngle, lastAngle));
            // if (currentAngle > 0 && lastAngle == 0) 
            if ((currentAngle == 0 && lastAngle == 345) || (currentAngle == 15 && lastAngle == 0)) physicsGraphic.rectTransform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            else if ((currentAngle == 0 && lastAngle == 15) || (currentAngle == 345 && lastAngle == 0)) physicsGraphic.rectTransform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
            else if (currentAngle > lastAngle) physicsGraphic.rectTransform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            else if (currentAngle < lastAngle) physicsGraphic.rectTransform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
        else if (physicsIndex == 1)
        {
            backgroundPhysicsGraphic.gameObject.SetActive(true);

            physicsGraphic.sprite = physicsSpriteList[1];
            backgroundPhysicsGraphic.sprite = physicsSpriteList[2];

            /*            if ((currentAngle == 0 && lastAngle == 345) || (currentAngle == 15 && lastAngle == 0)) Mathf.Clamp(opacityValue += 0.05f, 0, 1);
                        else if ((currentAngle == 0 && lastAngle == 15) || (currentAngle == 345 && lastAngle == 0)) Mathf.Clamp(opacityValue -= 0.05f, 0, 1); 
                        else if (currentAngle > lastAngle) Mathf.Clamp(opacityValue += 0.05f, 0, 1);
                        else if (currentAngle < lastAngle) Mathf.Clamp(opacityValue -= 0.05f, 0, 1);*/

            if ((currentAngle == 0 && lastAngle == 345) || (currentAngle == 15 && lastAngle == 0)) opacityValue += 0.05f;
            else if ((currentAngle == 0 && lastAngle == 15) || (currentAngle == 345 && lastAngle == 0)) opacityValue -= 0.05f;
            else if (currentAngle > lastAngle) opacityValue += 0.05f;
            else if (currentAngle < lastAngle) opacityValue -= 0.05f;

            if (opacityValue < 0) opacityValue = 0;
            else if (opacityValue > 1) opacityValue = 1; 

            physicsGraphic.color = new Color(1, 1, 1, opacityValue);
            backgroundPhysicsGraphic.color = new Color(1, 1, 1, 1 - opacityValue);
        }
        else if (physicsIndex == 2)
        {
            physicsGraphic.gameObject.SetActive(false);
            slide3Object.SetActive(true);

            if (SerialCommunication.encoderValue % 2 == 0)
            {
                slide3good.gameObject.SetActive(true);
                slide3bad.gameObject.SetActive(false);
            } else 
            {
                slide3good.gameObject.SetActive(false);
                slide3bad.gameObject.SetActive(true);
            }
        }
        else if (physicsIndex == 3)
        {
            physicsGraphic.gameObject.SetActive(false);
            slides45Object.SetActive(true);
            slides45d.gameObject.SetActive(false);
            slides45e.gameObject.SetActive(false);

            if (currentAngle % 45 == 0)
            {
                slides45a.gameObject.SetActive(true);
                slides45b.gameObject.SetActive(false);
                slides45c.gameObject.SetActive(false);
            }
            else if (currentAngle % 45 == 15)
            {
                slides45a.gameObject.SetActive(true);
                slides45b.gameObject.SetActive(true);
                slides45c.gameObject.SetActive(false);
            }
            else if (currentAngle % 45 == 30)
            {
                slides45a.gameObject.SetActive(true);
                slides45b.gameObject.SetActive(true);
                slides45c.gameObject.SetActive(true);
            }
        }
        else if (physicsIndex == 4)
        {
            physicsGraphic.gameObject.SetActive(false);
            slides45Object.SetActive(true);
            slides45a.gameObject.SetActive(true);
            slides45b.gameObject.SetActive(true);
            slides45c.gameObject.SetActive(true);

            if (SerialCommunication.encoderValue % 2 == 0)
            {
                slides45d.gameObject.SetActive(false);
                slides45e.gameObject.SetActive(false);
            }
            else 
            {
                slides45d.gameObject.SetActive(true);
                slides45e.gameObject.SetActive(true);
            }
        }
        else if (physicsIndex == 5)
        {
            physicsGraphic.gameObject.SetActive(false);
            slide6Object.SetActive(true);
            // if (currentAngle >= 0 && currentAngle < 120)
            if (currentAngle % 45 == 0)
            {
                slide6a.gameObject.SetActive(true);
                slide6b.gameObject.SetActive(false);
                slide6c.gameObject.SetActive(false);
            }
            // else if (currentAngle >= 120 && currentAngle < 240)
            else if (currentAngle % 45 == 15)
            {
                slide6a.gameObject.SetActive(false);
                slide6b.gameObject.SetActive(true);
                slide6c.gameObject.SetActive(false);
            }
            // else if (currentAngle >= 240 && currentAngle < 360)
            else if (currentAngle % 45 == 30)
            {
                slide6a.gameObject.SetActive(false);
                slide6b.gameObject.SetActive(false);
                slide6c.gameObject.SetActive(true);
            }
        }
        else if (physicsIndex == 6)
        {
            physicsGraphic.gameObject.SetActive(false);
            slide7Object.SetActive(true);

            /*            if ((currentAngle == 0 && lastAngle == 345) || (currentAngle == 15 && lastAngle == 0)) Mathf.Clamp(opacityValue += 0.05f, 0, 1);
                        else if ((currentAngle == 0 && lastAngle == 345) || (currentAngle == 15 && lastAngle == 0)) Mathf.Clamp(opacityValue -= 0.05f, 0, 1); 
                        else if (currentAngle > lastAngle) Mathf.Clamp(opacityValue += 0.05f, 0, 1);
                        else if (currentAngle < lastAngle) Mathf.Clamp(opacityValue -= 0.05f, 0, 1);*/

            if ((currentAngle == 0 && lastAngle == 345) || (currentAngle == 15 && lastAngle == 0)) opacityValue += 0.05f;
            else if ((currentAngle == 0 && lastAngle == 15) || (currentAngle == 345 && lastAngle == 0)) opacityValue -= 0.05f;
            else if (currentAngle > lastAngle) opacityValue += 0.05f;
            else if (currentAngle < lastAngle) opacityValue -= 0.05f;

            if (opacityValue < 0) opacityValue = 0;
            else if (opacityValue > 1) opacityValue = 1;

            slider.value = opacityValue;
        }
        else if (physicsIndex == 7)
        {
            physicsGraphic.gameObject.SetActive(false);
            slide8Object.SetActive(true);

            if (currentAngle % 60 == 0)
            {
                slide8a.gameObject.SetActive(true);
                slide8b.gameObject.SetActive(false);
                slide8c.gameObject.SetActive(false);
                slide8d.gameObject.SetActive(false);
            }
            else if (currentAngle % 60 == 15)
            {
                slide8a.gameObject.SetActive(true);
                slide8b.gameObject.SetActive(true);
                slide8c.gameObject.SetActive(false);
                slide8d.gameObject.SetActive(false);
            }
            else if (currentAngle % 60 == 30)
            {
                slide8a.gameObject.SetActive(true);
                slide8b.gameObject.SetActive(true);
                slide8c.gameObject.SetActive(true);
                slide8d.gameObject.SetActive(false);
            }
            else if (currentAngle % 60 == 45)
            {
                slide8a.gameObject.SetActive(true);
                slide8b.gameObject.SetActive(true);
                slide8c.gameObject.SetActive(true);
                slide8d.gameObject.SetActive(true);
            }

        }
        else if (physicsIndex == 8)
        {
            physicsGraphic.sprite = physicsSpriteList[3];
            qiskitCaption.gameObject.SetActive(true);
        }
    }
}
