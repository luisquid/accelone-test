using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverInput : MonoBehaviour
{
    public string[] commandsArray;

    Vector3 initialPos;

    public void ReceiveSpawnInfo(int _x, int _y, string _dir)
    {
        transform.rotation = Quaternion.identity;
        
        initialPos = new Vector3(_x, transform.position.y, _y);
        transform.position = initialPos;
        
        switch (_dir)
        {
            case "N":
                transform.rotation = Quaternion.identity;
                break;
            case "S":
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                break;
            case "E":
                transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                break;

            case "W":
                transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                break;

        }
    }

    public void ReceiveCommands(string _arrayText)
    {
        commandsArray = _arrayText.Split(',');
    }
}
