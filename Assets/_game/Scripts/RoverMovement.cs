using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RoverMovement : MonoBehaviour
{
    public float timeBetweenCommands = 0.2f;
    public float movementAmount = 1f;
    public float movementSpeed = 0.4f;

    private RoverInput roverInput;
    private bool isMoving = false;
    private bool startMoving = false;
    private int index;

    #region Unity Functions
    private void Start()
    {
        roverInput = GetComponent<RoverInput>();
    }

    private void Update()
    {
        if(startMoving)
        {
            if(!isMoving && index < roverInput.commandsArray.Length)
            {
                isMoving = true;
                SetState(roverInput.commandsArray[index]);
            }
            else if (index > roverInput.commandsArray.Length)
            {
                startMoving = false;
            }
        }
    }
    #endregion

    public void StartBehavior()
    {
        startMoving = true;
        index = 0;
    }

    public void SetState(string _stateId)
    {
        switch(_stateId)
        {
            case "f":
                StartCoroutine(MovePosition(transform.position + (transform.forward * movementAmount), movementSpeed));
                break;
            case "b":
                StartCoroutine(MovePosition(transform.position + (-transform.forward * movementAmount), movementSpeed));
                break;
            case "l":
                StartCoroutine(RotatePosition(Vector3.up * 90f, movementSpeed));
                break;
            case "r":
                StartCoroutine(RotatePosition(Vector3.down * 90f, movementSpeed));
                break;
            default:
                Debug.Log("COMMAND NOT AVAILABLE!");
                index++;
                break;
        }
    }

    IEnumerator MovePosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        StartCoroutine(WaitForCommand());
    }

    IEnumerator RotatePosition(Vector3 targetRotation, float duration)
    {
        float time = 0;
        Vector3 startRotation = transform.rotation.eulerAngles;
        while (time < duration)
        {
            transform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, targetRotation, time / duration));
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetRotation;

        StartCoroutine(WaitForCommand());
    }

    IEnumerator WaitForCommand()
    {
        //We add some time between commands
        yield return new WaitForSeconds(timeBetweenCommands);

        index++;
        isMoving = false;
    }

}
