using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Controls controls;
    
    public bool HelpInput {get; set;}
    public Vector2 MovePosition { get; private set; }
    public Vector2 LookPosition { get; private set; }
    public bool JumpInput { get; set; } = false;
    public bool RunInput { get; private set; } = false;
    public bool LeftClickPressed { get; set; } = false;
    public bool RightClickPressed { get; set; } = false;
    public Controls Controls { get { return controls; } }
    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
            controls.Player.Move.performed += i => MovePosition = i.ReadValue<Vector2>();
             controls.Player.Look.performed += i => LookPosition = i.ReadValue<Vector2>();
             controls.Player.Run.performed += i => RunInput = true;
             controls.Player.Run.canceled += i => RunInput = false;
             controls.UI.Help.performed += i => HelpInput = true;
            // controls.Player.Jump.performed += i => JumpInput = true;
            // controls.Player.LeftClick.performed += i => LeftClickPressed = true;
            // controls.Player.LeftClick.canceled += i => LeftClickPressed = false;
            // controls.Player.RightClick.performed += i => RightClickPressed = true;
            // controls.Player.RightClick.canceled += i => RightClickPressed = false;
            
        }
        controls.Player.Enable();
        controls.UI.Enable();
    }

    

    private void OnDisable()
    {
        controls.Player.Disable();
        controls.UI.Disable();
    }
}