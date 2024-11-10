using Game.Meta.Console.Scripts.GUI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Meta.Console.Scripts.Inputs
{
    public class ConsoleInputsHandler
    {
        public ConsoleInputs inputs;

        public void EnableHistoryActions()
        { inputs.History.Enable();  }
        
        public void DisableHistoryActions()
        { inputs.History.Disable(); }
        
        public void EnableMouseActions()
        { inputs.ObjectPick.Enable();  }
        
        public void DisableMouseActions()
        { inputs.ObjectPick.Disable(); }

        
        public void InitializeSubscriptions()
        {
            inputs.Enabler.EnableConsole.performed += ConsoleGUIBase.instance.SwitchActiveState;
            inputs.Enabler.EnableConsole.performed += ConsoleSubscriptions.instance.EnableOtherInputs;
            
            inputs.History.HistoryForward.performed += ConsoleBehavior.instance.PushHistoryForward;
            inputs.History.HistoryBackward.performed += ConsoleBehavior.instance.PushHistoryBackward;
            
            inputs.OtherGUI.ResizeFromCenter.performed += ConsoleGUIBase.instance.resizerHandler.TurnOnResizeFromCenter;
            inputs.OtherGUI.ResizeFromCenter.canceled += ConsoleGUIBase.instance.resizerHandler.TurnOffResizeFromCenter;

            inputs.ObjectPick.Click.performed += OnClickOnperformed;
        }

        private void OnClickOnperformed(InputAction.CallbackContext c)
        {
            SelectObjectCmdUtils.TryToFindGameObject(inputs.ObjectPick.Mouse.ReadValue<Vector2>());
        }

        public void RemoveSubscriptions()
        {
            inputs.Enabler.EnableConsole.performed -= ConsoleGUIBase.instance.SwitchActiveState;
            inputs.Enabler.EnableConsole.performed -= ConsoleSubscriptions.instance.EnableOtherInputs;
            
            inputs.History.HistoryForward.performed -= ConsoleBehavior.instance.PushHistoryForward;
            inputs.History.HistoryBackward.performed -= ConsoleBehavior.instance.PushHistoryBackward;
            
            inputs.OtherGUI.ResizeFromCenter.performed -= ConsoleGUIBase.instance.resizerHandler.TurnOnResizeFromCenter;
            inputs.OtherGUI.ResizeFromCenter.canceled -= ConsoleGUIBase.instance.resizerHandler.TurnOffResizeFromCenter;
            
            inputs.ObjectPick.Click.performed -= OnClickOnperformed;
        }
    }
}