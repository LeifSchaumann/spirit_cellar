using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerInput { }

public class MoveInput : PlayerInput
{
    public Vector2Int dir;

    public MoveInput(Vector2Int dir)
    {
        this.dir = dir;
    }
}

public class PossessInput : PlayerInput
{
    public Vector2Int dir;
    public PossessInput(Vector2Int dir)
    {
        this.dir = dir;
    }

}

public class ResetInput : PlayerInput { }

public class InputManager : MonoBehaviour
{
    public static InputManager main;

    private List<InputAction> moveActions;
    private InputAction shiftAction;
    private InputAction resetAction;

    public readonly List<Vector2Int> dirs = new List<Vector2Int>() 
    {Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};

    private void Awake()
    {
        main = this;

        moveActions = new List<InputAction>
        {
            InputSystem.actions.FindAction("Down"),
            InputSystem.actions.FindAction("Up"),
            InputSystem.actions.FindAction("Left"),
            InputSystem.actions.FindAction("Right")
        };

        shiftAction = InputSystem.actions.FindAction("Shift");
        resetAction = InputSystem.actions.FindAction("Reset");

    }

    void Update()
    {
        if (GameManager.main.acceptingInput)
        {
            if (resetAction.WasPressedThisFrame())
            {
                _ = GameManager.main.HandleInput(new ResetInput());
            } else
            {
                for (int i = 0; i < dirs.Count; i++)
                {
                    if (moveActions[i].WasPressedThisFrame())
                    {
                        if (shiftAction.IsPressed())
                        {
                            _ = GameManager.main.HandleInput(new PossessInput(dirs[i]));
                        }
                        else
                        {
                            _ = GameManager.main.HandleInput(new MoveInput(dirs[i]));
                        }
                        break;
                    }
                }
            }
        }
    }
}
