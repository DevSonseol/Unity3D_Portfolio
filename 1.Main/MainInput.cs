using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public partial class Main : MonoBehaviour
{

    private bool _leftPressed = false;
    private bool _rightPressed = false;

    private float _leftPressedTime = 0;
    private float _rightPressedTime = 0;

    public static Action<GlobalEnum.MouseEvent> MouseLeftAction = null;
    public static Action<GlobalEnum.MouseEvent> MouseRightAction = null;
    public static Action KeyAction = null;

    public void OnUpdateInput(float dt)
    {
        if (gameState != GameState.InGame)
            return;
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.anyKey && KeyAction != null)
        {
            KeyAction.Invoke();
        }

        if (MouseLeftAction != null)
        {
            //좌클릭
            if (Input.GetMouseButton(0))
            {
                if (!_leftPressed)
                {
                    MouseLeftAction.Invoke(GlobalEnum.MouseEvent.LeftPointerDown);
                    _leftPressedTime = Time.time;
                }
                MouseLeftAction.Invoke(GlobalEnum.MouseEvent.LeftPress);
                _leftPressed = true;
            }
            else
            {
                if (_leftPressed)
                {
                    if (Time.time < _leftPressedTime + 0.2f)
                        MouseLeftAction.Invoke(GlobalEnum.MouseEvent.LeftClick);
                    MouseLeftAction.Invoke(GlobalEnum.MouseEvent.LeftPointerUp);
                }
                _leftPressed = false;
                _leftPressedTime = 0;
            }
        }
        if (MouseRightAction != null)
        {
            if (_leftPressed)
                return;
            
            // 우클릭
            if (Input.GetMouseButton(1))
            {
                if (!_rightPressed)
                {
                    MouseRightAction.Invoke(GlobalEnum.MouseEvent.RightPointerDown);
                    _rightPressedTime = Time.time;
                }
                MouseRightAction.Invoke(GlobalEnum.MouseEvent.RightPress);
                _rightPressed = true;
            }
            else
            {
                if (_rightPressed)
                {
                    if (Time.time < _rightPressedTime + 0.2f)
                        MouseRightAction.Invoke(GlobalEnum.MouseEvent.RightClick);
                    MouseRightAction.Invoke(GlobalEnum.MouseEvent.RightPointerUp);
                }
                _rightPressed = false;
                _rightPressedTime = 0;
            }
        }

    }
    
    void EndInput()
    {
        KeyAction = null;
        MouseLeftAction = null;
        MouseRightAction = null;
    }
}
