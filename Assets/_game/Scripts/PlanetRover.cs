using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRover : MonoBehaviour
{
    public float timeBetweenCommands = 0.2f;
    public float movementAmount = 1f;
    public float movementSpeed = 0.4f;
    public bool isRunning = false;

    private PlanetGravity planetGravity;
    private RoverInput roverInput;
    private bool executing = false;
    private int index;

    #region Unity Functions
    private void Start()
    {
        planetGravity = FindObjectOfType<PlanetGravity>();
        roverInput = GetComponent<RoverInput>();
    }

    private void Update()
    {
        if (isRunning)
        {
            if (!executing && index < roverInput.commandsArray.Length)
            {
                executing = true;
                SetState(roverInput.commandsArray[index]);
            }
            else if (index >= roverInput.commandsArray.Length)
            {
                isRunning = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
            StartBehavior();
    }

    private void FixedUpdate()
    {
        if(planetGravity)
        {
            planetGravity.Attract(transform);
        }
    }
    #endregion

    public void StartBehavior()
    {
        isRunning = true;
        index = 0;
    }
   
    public void SetState(string _stateId)
    {
        switch (_stateId)
        {
            case "f":
                StartCoroutine(MovePosition(transform.position + (transform.forward * movementAmount), movementSpeed));
                break;
            case "b":
                StartCoroutine(MovePosition(transform.position + (-transform.forward * movementAmount), movementSpeed));
                break;
            case "l":
                StartCoroutine(RotatePosition(transform.rotation.eulerAngles + (transform.up * -90f), movementSpeed));
                break;
            case "r":
                StartCoroutine(RotatePosition(transform.rotation.eulerAngles + (transform.up * 90f), movementSpeed));
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
