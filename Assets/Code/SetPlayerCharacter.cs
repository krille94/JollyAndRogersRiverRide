using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerCharacter : MonoBehaviour
{
    public enum PlayerIndexTypes { One, Two }
    public PlayerIndexTypes playerIndex = 0;

    // Start is called before the first frame update
    void Awake()
    {
        // Set the player

        string charName="";

        if (playerIndex.ToString() == "One") charName = PlayerData.player1Character;
        else if (playerIndex.ToString() == "Two") charName = PlayerData.player2Character;

        GameObject newObj = new GameObject(charName);

        GameObject charObj=null;
        if (charName=="Jolly")
            charObj = Resources.Load("Models/player2model") as GameObject;
        else
            charObj = Resources.Load("Models/player1model") as GameObject;

        //charObj = Resources.Load("Models/"+charName+"_model") as GameObject;


        if (charObj != null)
        {
            newObj = Instantiate(charObj, gameObject.transform);
            if (playerIndex.ToString() == "One")
                newObj.transform.localPosition = new Vector3(0, 0, 0);
            else if (playerIndex.ToString() == "Two")
                newObj.transform.localPosition = new Vector3(0, 0, -0.25f);
            newObj.transform.localScale = new Vector3(90, 125, 90);
            if (charName == "Jolly")
                newObj.transform.localEulerAngles = new Vector3(0, -90, 0);
            else
                newObj.transform.localEulerAngles = new Vector3(-45, 0, 0);
        }

        /*newObj.transform.parent = gameObject.transform;*/
    }

}
