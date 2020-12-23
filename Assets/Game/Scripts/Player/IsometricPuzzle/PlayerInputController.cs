using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    private PlayerMovementController _playerMovement;

    private IInteractable curInteractor;
    public bool isPressingInteract { get; private set; }

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _playerMovement = GetComponent<PlayerMovementController>();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Player.Interact.performed += OnInteractButton;
        _inputActions.Player.Movement.performed += OnMovement;
    }
    private void OnDisable()
    {
        _inputActions.Disable();
        _inputActions.Player.Interact.performed -= OnInteractButton;
        _inputActions.Player.Movement.performed -= OnMovement;
    }

    // gets called whenever there is playerMovement input (WASD, gamepad...)
    public void OnMovement(InputAction.CallbackContext context )
    {
        var movementInput = context.ReadValue<Vector2>();
        _playerMovement.Move(movementInput);
        //Debug.Log("Movement Input");
    }

    // gets called whenever interact button is pressed
    public void OnInteractButton( InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        isPressingInteract = value >= 0.15;
        if (isPressingInteract && curInteractor != null)
        {
            curInteractor.Interact();
            //Debug.Log("Interact Button Pressed");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if there is an interactive component
        var interactable = other.gameObject.GetComponent<IInteractable>();
        if (interactable != null)
            curInteractor = interactable;
    }

    private void OnTriggerExit(Collider other)
    {
        curInteractor = null;
    }

}
