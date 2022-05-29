using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RoverMovement : MonoBehaviour
{
    public float timeBetweenCommands = 0.2f;
    public float movementAmount = 1f;
    public float movementSpeed = 0.4f;

    private RoverInput roverInput;
    private bool executing = false;
    private bool isRunning = false;
    private int index;

    #region Unity Functions
    private void Start()
    {
        roverInput = GetComponent<RoverInput>();
    }

    private void Update()
    {
        if(isRunning)
        {
            if(!executing && index < roverInput.commandsArray.Length)
            {
                executing = true;
                SetState(roverInput.commandsArray[index]);
            }
            else if (index >= roverInput.commandsArray.Length)
            {
                isRunning = false;
            }
        }
    }
    #endregion

    public void StartBehavior()
    {
        isRunning = true;
        index = 0;
    }

    public bool IsRunning()
    {
        return isRunning;
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
                StartCoroutine(RotatePosition(transform.rotation.eulerAngles + (Vector3.down * 90f), movementSpeed));
                break;
            case "r":
                StartCoroutine(RotatePosition(transform.rotation.eulerAngles + (Vector3.up * 90f), movementSpeed));
                break;
            default:
                Debug.Log("INVALID COMMAND!");
                StartCoroutine(WaitForCommand()); 
                break;
        }
    }

    IEnumerator MovePosition(Vector3 _targetPosition, float _duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < _duration)
        {
            transform.position = Vector3.Lerp(startPosition, _targetPosition, time / _duration);
            time += Time.deltaTime;
            yield return null;
        }

        if (_targetPosition.x > CameraProperties.width || _targetPosition.x < -CameraProperties.width)
            _targetPosition.x = _targetPosition.x * -1f;

        if (_targetPosition.z > CameraProperties.height || _targetPosition.z < -CameraProperties.height)
            _targetPosition.z = _targetPosition.z * -1f;

        transform.position = _targetPosition;

        StartCoroutine(WaitForCommand());
    }

    IEnumerator RotatePosition(Vector3 _targetRotation, float _duration)
    {
        float time = 0;
        Vector3 startRotation = transform.rotation.eulerAngles;
        while (time < _duration)
        {
            transform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, _targetRotation, time / _duration));
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = Quaternion.Euler(_targetRotation);

        StartCoroutine(WaitForCommand());
    }

    IEnumerator WaitForCommand()
    {
        //We add a delay between commands
        yield return new WaitForSeconds(timeBetweenCommands);

        index++;
        executing = false;
    }

}
