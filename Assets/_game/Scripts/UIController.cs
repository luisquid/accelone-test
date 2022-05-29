using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("ROVER CONFIG INPUT FIELDS")]
    public TMP_InputField xInputField;
    public TMP_InputField yInputField;
    public TMP_InputField directionInputField;
    public TMP_InputField commandsInputField;

    [Header("DATA VALIDATION ERROR LOGS")]
    public GameObject errorPanel;
    public TextMeshProUGUI errorText;

    [Header("ROVER CONTROLLER")]
    public RoverInput playerInput;
    public RoverMovement playerMovement;

    private bool xInRange, yInRange = false;

    #region Unity Functions
    private void Start()
    {
        xInputField.GetComponentInChildren<TextMeshProUGUI>().text = "X Range [" + Mathf.Ceil(-CameraProperties.width) + "," + Mathf.Floor(CameraProperties.width) + "]";
        yInputField.GetComponentInChildren<TextMeshProUGUI>().text = "Y Range [" + Mathf.Ceil(-CameraProperties.height) + "," + Mathf.Floor(CameraProperties.height) + "]";
    }
    #endregion

    #region Button Functions
    public void OnSendButtonClicked()
    {
        //If either input field is null or empty we can't continue
        if (string.IsNullOrEmpty(xInputField.text) || 
            string.IsNullOrEmpty(yInputField.text) || 
            string.IsNullOrEmpty(directionInputField.text) || 
            string.IsNullOrEmpty(commandsInputField.text))
            ShowErrorMessage("Please fill all the information");
        else
        {
            //We cache the parsed x and y value 
            int xPos = int.Parse(xInputField.text);
            int yPos = int.Parse(yInputField.text);

            //We validate the x and y value to set starting point
            if(xInRange && yInRange)
            {
                playerInput.ReceiveSpawnInfo(int.Parse(xInputField.text), int.Parse(yInputField.text), directionInputField.text);
                playerInput.ReceiveCommands(commandsInputField.text);
            }
            else
            {
                ShowErrorMessage("POSITION VALUES NOT IN RANGE!");
            }          
        } 
    }

    public void OnStartButtonClicked()
    {
        if(playerInput.commandsArray.Length <= 0)
        {
            ShowErrorMessage("COMMANDS NOT AVAILABLE!");
        }
        else if(playerMovement.IsRunning())
        {
            ShowErrorMessage("COMMANDS IN PROGRESS!");
        }
        else
        {
            playerMovement.StartBehavior();
        }
    }

    public void OnCloseButtonClicked()
    {
        errorPanel.SetActive(false);
    }
    #endregion

    public void ShowErrorMessage(string _errorLog)
    {
        errorText.text = _errorLog;
        errorPanel.SetActive(true);
    }
    
    public void VerifyXRange(string _value)
    {
        if (string.IsNullOrEmpty(_value))
            return;

        int numValue = int.Parse(_value);
        if (numValue < -CameraProperties.width || numValue > CameraProperties.width)
        {
            ShowErrorMessage("X VALUE OUT OF RANGE!");
            xInRange = false;
        }
        else
            xInRange = true;
    }

    public void VerifyYRange(string _value)
    {
        if (string.IsNullOrEmpty(_value))
            return;
        int numValue = int.Parse(_value);
        if (numValue < -CameraProperties.height || numValue > CameraProperties.height)
        {
            ShowErrorMessage("Y VALUE OUT OF RANGE!");
            yInRange = false;
        }
        else
            yInRange = true;
    }
}
