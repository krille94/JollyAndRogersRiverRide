using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControls : MonoBehaviour
{
    Color normal = new Color(1, 0.75f, 0);
    Color highlighted = Color.white;
    public List<GameObject> buttons = new List<GameObject>();

    public int menuOption=0;
    private bool alreadyMoved=false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in buttons)
            obj.GetComponent<TextMesh>().color = normal;

        buttons[menuOption].GetComponent<TextMesh>().color = highlighted;
    }

    // Update is called once per frame
    void Update()
    {
        if (!buttons[menuOption].activeInHierarchy)
        {
            buttons.RemoveAt(menuOption);
            if (menuOption >= buttons.Count)
                menuOption--;
            buttons[menuOption].GetComponent<TextMesh>().color = highlighted;
        }

        if (buttons.Count > 1)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].GetComponent<MouseHover>().GetSelected())
                {
                    buttons[i].GetComponent<MouseHover>().SetSelected(false);

                    buttons[menuOption].GetComponent<TextMesh>().color = normal;
                    buttons[i].GetComponent<TextMesh>().color = highlighted;
                    menuOption = i;
                }
            }

            if (Input.GetAxis("Player_One_Joystick_Vertical") > 0)
            {
                if (!alreadyMoved)
                {
                    alreadyMoved = true;

                    buttons[menuOption].GetComponent<TextMesh>().color = normal;
                    menuOption++;
                    if (menuOption >= buttons.Count)
                        menuOption = 0;

                    buttons[menuOption].GetComponent<TextMesh>().color = highlighted;
                }
            }
            else if (Input.GetAxis("Player_One_Joystick_Vertical") < 0)
            {
                if (!alreadyMoved)
                {
                    alreadyMoved = true;
                    buttons[menuOption].GetComponent<TextMesh>().color = normal;
                    menuOption--;
                    if (menuOption < 0)
                        menuOption = buttons.Count-1;
                    buttons[menuOption].GetComponent<TextMesh>().color = highlighted;
                }
            }
            else
                alreadyMoved = false;
        }


        if (Input.GetButtonUp("Player_One_Paddle_Back"))
        {
            buttons[menuOption].GetComponent<MenuButtons>().PressButton();
        }/*
        if (Input.GetButtonUp("Player_One_Paddle_Back"))
        {
            if(buttons[buttons.Count-1].name=="Return")
                buttons[buttons.Count - 1].GetComponent<MenuButtons>().PressButton();
        }*/
    }
}
