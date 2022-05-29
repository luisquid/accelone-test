using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public TMP_InputField xInputField, yInputField, directionInputField, commandsInputField;    
    public RoverInput playerInput;
    public RoverMovement playerMovement;

    Camera mainCam;

    private float height = 0f;
    private float width = 0f;

    #region Unity Functions
    private void Start()
    {
        mainCam = Camera.main;
        height = mainCam.orthographicSize;
        width = mainCam.orthographicSize * mainCam.aspect;

        xInputField.GetComponentInChildren<TextMeshProUGUI>().text = "X Range [" + Mathf.Ceil(-width) + "," + Mathf.Floor(width) + "]";
        yInputField.GetComponentInChildren<TextMeshProUGUI>().text = "Y Range [" + Mathf.Ceil(-height) + "," + Mathf.Floor(height) + "]";
    }
    #endregion

    public void OnSendButtonClicked()
    {
        //If either input field is null or empty we can't continue
        if (string.IsNullOrEmpty(xInputField.text) || string.IsNullOrEmpty(yInputField.text) || string.IsNullOrEmpty(directionInputField.text))
            Debug.Log("Please fill all the info");
        else
        {
            //We cache the parsed x and y value 
            int xPos = int.Parse(xInputField.text);
            int yPos = int.Parse(yInputField.text);

            //We validate the x and y value to set starting point
            if (xPos < -width || xPos > width)
            {
                Debug.Log("OUT OF X RANGE!");
            }
            else if(yPos < -height || yPos > height)
            {
                Debug.Log("OUT OF Y RANGE!");
            }
            else
            {
                playerInput.ReceiveSpawnInfo(int.Parse(xInputField.text), int.Parse(yInputField.text), directionInputField.text);
                playerInput.ReceiveCommands(commandsInputField.text);
            }          
        } 
    }

    public void OnStartButtonClicked()
    {
        if(playerInput.commandsArray.Length <= 0)
        {
            Debug.Log("COMMANDS NOT AVAILABLE!");
        }
        else
        {
            playerMovement.StartBehavior();
        }
    }
}
