using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region INSPECTOR_REG
    [Header("HANDLING")]
    [SerializeField] private float turnSpeed;
    [SerializeField] private float moveSpeed;

    [Header("INPUT")]
    [SerializeField] private InputActionReference control_move;

    //System
    private Quaternion targetRotation;

    //Components
    private CharacterController controller;
    #endregion

    #region UNITY_REG
    private void Awake()
    {
        controller = this.GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        control_move.action.Enable();
    }

    private void OnDisable()
    {
        control_move.action.Disable();
    }

    private void Update()
    {
        Move();
    }
    #endregion

    #region CLASS_REG

    void Move()
    {
        Vector2 input = ReadInput(control_move.action);
        Vector3 moveInput = new Vector3(input.x, 0f, input.y);

        if (moveInput != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(moveInput);
            this.transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(this.transform.eulerAngles.y, targetRotation.eulerAngles.y, turnSpeed * Time.deltaTime);
        }

        Vector3 motion = moveInput;
        //motion *= (Mathf.Abs(moveInput.x) == 1 && Mathf.Abs(moveInput.z) == 1) ? 0.7f : 1f;
        motion *= moveSpeed;
        //motion += Vector3.up * -8;

        controller.Move(motion * Time.deltaTime);
    }

    Vector2 ReadInput(InputAction _action)
    {
        Vector2 input = _action.ReadValue<Vector2>();

        //Check for deadzone in gamepad input
        if (input.sqrMagnitude < 0.01f)
            input = Vector2.zero;

        return input;
    }
    #endregion
}
