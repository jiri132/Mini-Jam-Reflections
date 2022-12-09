using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public enum movementType { Undefined,Keyboard, Controller }

    public movementType _movementType = movementType.Undefined;
    public MovementManager movementManager;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_movementType == movementType.Undefined)
        {
            GetMovementType();
            return;
        }

        transform.position += (Vector3)movementManager.Direction() * movementManager.Speed * Time.deltaTime;
    }

    public void GetMovementType()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _movementType = movementType.Keyboard;
            movementManager = new KeyboardControls();
        }
        else if (Input.GetButton("Submit"))
        {
            _movementType = movementType.Controller;
            movementManager = new ControllerControls();
        }
    }
}
