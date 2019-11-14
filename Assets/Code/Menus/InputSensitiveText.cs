using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSensitiveText : MonoBehaviour
{
    [TextArea]
    public string keyboardText;

    [TextArea]
    public string controllerText;

    bool usingController;
    TextMesh textmesh;

    // Start is called before the first frame update
    void OnEnable()
    {
        textmesh = GetComponent<TextMesh>();

        if(!GameController.instance.GetUsingController()&&!GameController.instance.GetUsingKeyboard())
        {
            if (Input.GetJoystickNames().Length > 0)
            {
                usingController = true;
                textmesh.text = controllerText;
                GameController.instance.SetUsingController(true);
            }
            else
            {
                usingController = false;
                textmesh.text = keyboardText;
                GameController.instance.SetUsingKeyboard(true);
            }
        }
        else if(GameController.instance.GetUsingController())
        {
            usingController = true;
            textmesh.text = controllerText;
        }
        else
        {
            usingController = false;
            textmesh.text = keyboardText;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Player_One_Joystick_Vertical") != 0 || Input.GetAxis("Player_One_Joystick_Horizontal") != 0)
        {
            if (!usingController)
            {
                usingController = true;
                textmesh.text = controllerText;
                GameController.instance.SetUsingController(true);
                GameController.instance.SetUsingKeyboard(false);
            }
        }
        else if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.Joystick1Button0) ||
                Input.GetKey(KeyCode.Joystick1Button1) ||
                Input.GetKey(KeyCode.Joystick1Button2) ||
                Input.GetKey(KeyCode.Joystick1Button3) ||
                Input.GetKey(KeyCode.Joystick1Button4) ||
                Input.GetKey(KeyCode.Joystick1Button5) ||
                Input.GetKey(KeyCode.Joystick1Button6) ||
                Input.GetKey(KeyCode.Joystick1Button7) ||
                Input.GetKey(KeyCode.Joystick1Button8) ||
                Input.GetKey(KeyCode.Joystick1Button9) ||
                Input.GetKey(KeyCode.Joystick1Button10) ||
                Input.GetKey(KeyCode.Joystick1Button11) ||
                Input.GetKey(KeyCode.Joystick1Button12) ||
                Input.GetKey(KeyCode.Joystick1Button13) ||
                Input.GetKey(KeyCode.Joystick1Button14) ||
                Input.GetKey(KeyCode.Joystick1Button15) ||
                Input.GetKey(KeyCode.Joystick1Button16) ||
                Input.GetKey(KeyCode.Joystick1Button17) ||
                Input.GetKey(KeyCode.Joystick1Button18) ||
                Input.GetKey(KeyCode.Joystick1Button19))
            {
                if (!usingController)
                {
                    usingController = true;
                    textmesh.text = controllerText;
                    GameController.instance.SetUsingController(true);
                    GameController.instance.SetUsingKeyboard(false);
                }
            }
            else 
            {
                usingController = false;
                textmesh.text = keyboardText;
                GameController.instance.SetUsingController(false);
                GameController.instance.SetUsingKeyboard(true);
            }
        }
    }
}
