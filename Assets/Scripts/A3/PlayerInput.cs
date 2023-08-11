using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector] public delegate void PlayerAxisInputEventHandler(Vector2 input);
    [HideInInspector] public event PlayerAxisInputEventHandler SetDirectionalInputEvent;

    [HideInInspector] public delegate void PlayerKeyInputEventHandler();
    [HideInInspector] public event PlayerKeyInputEventHandler OnShiftInputDownEvent;
    [HideInInspector] public event PlayerKeyInputEventHandler OnShiftInputUpEvent;

    [HideInInspector] public event PlayerKeyInputEventHandler OnJumpInputDownEvent;
    [HideInInspector] public event PlayerKeyInputEventHandler OnJumpInputUpEvent;

    private void LateUpdate()
    {
        if (SetDirectionalInputEvent != null)
            SetDirectionalInputEvent(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));

        if (Input.GetKeyDown(KeyCode.LeftShift) && OnShiftInputDownEvent != null) OnShiftInputDownEvent();

        if (Input.GetKeyUp(KeyCode.LeftShift) && OnShiftInputUpEvent != null) OnShiftInputUpEvent();

        if (Input.GetKeyDown(KeyCode.Z) && OnJumpInputDownEvent != null) OnJumpInputDownEvent();

        if (Input.GetKeyUp(KeyCode.Z) && OnJumpInputUpEvent != null) OnJumpInputUpEvent();

        if (Input.GetKeyDown(KeyCode.Tab)) Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
    }



}
