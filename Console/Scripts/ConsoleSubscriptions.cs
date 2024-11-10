using System.Collections.Generic;
using Game.Meta.Console.Scripts.GUI;
using Game.Meta.Console.Scripts.Inputs;
using Game.Meta.Heuristics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Game.Meta.Console.Scripts

{
    public class ConsoleSubscriptions : MonoBehaviour
    {
        
        private SerializedDictionary<string, List<IInputsManageblaByConsole> > managableInputs;
        public static ConsoleSubscriptions instance;
        private void Start()
        {
            if (instance != null)
            {
                Debug.LogError("ConsoleSubscriptions has already instantiated in scene: \"" + gameObject.scene.name + "\"");
                return;
            }
            instance = this;
            
            managableInputs = new SerializedDictionary<string, List<IInputsManageblaByConsole>>();
            CheckForSceneWithInputs(SceneManager.GetActiveScene(), 0);
            SetupEventListeners();
            ConsoleGUIBase.instance.inputsHandler.InitializeSubscriptions();

        }

        private void OnDisable()
        {
            RemoveEventListeners();
            ConsoleGUIBase.instance.inputsHandler.RemoveSubscriptions();
        }

        

        private void OnsceneUnloaded(Scene arg0)
        {
            
            if (!managableInputs.Remove(arg0.name))
            {
                Debug.LogError((arg0.name) + " was not found in List".ColorRT(Color.gray));
            }
            
        }

        private void CheckForSceneWithInputs(Scene scene, LoadSceneMode arg1)
        {
            List<IInputsManageblaByConsole> temp = new List<IInputsManageblaByConsole>();
            foreach (GameObject sceneGameObject in scene.GetRootGameObjects())
            {
                foreach (var item in sceneGameObject.GetComponentsInChildren<IInputsManageblaByConsole>(true))
                { temp.Add(item); }
                
            }
            managableInputs.Add(scene.name, temp);
            
        }

        public void EnableOtherInputs(InputAction.CallbackContext c)
        {
            foreach (var VARIABLE in managableInputs.Values)
            {
                foreach (var VARIABLE1 in VARIABLE)
                {
                     VARIABLE1.EnableByConsole();
                }
              
            }
        }

        public void DisableOtherInputs()
        {
        
            foreach (var VARIABLE in managableInputs.Values)
            {
                foreach (var VARIABLE1 in VARIABLE)
                {
                    VARIABLE1.DisableByConsole();
                }
              
            }
        }
        
        private void SetupEventListeners()
        {
            SceneManager.sceneLoaded += CheckForSceneWithInputs;
            SceneManager.sceneUnloaded += OnsceneUnloaded;
            
            ConsoleGUIBase.instance.consoleInput.onSubmit
                .AddListener(
                    ConsoleBehavior.instance.UsersInput);
            
            ConsoleGUIBase.instance.consoleInput.onSelect
                .AddListener(delegate 
                    { ConsoleGUIBase.instance.inputsHandler.EnableHistoryActions(); });
            
            ConsoleGUIBase.instance.consoleInput.onDeselect
                .AddListener(delegate 
                    { ConsoleGUIBase.instance.inputsHandler.DisableHistoryActions(); });
            
            ConsoleGUIBase.instance.consoleInput.onValueChanged
                .AddListener(delegate 
                    {ConsoleBehavior.instance.ResetConsoleHistory(); });
            
            ConsoleGUIBase.instance.consoleInput.onSelect
                .AddListener(delegate 
                    {DisableOtherInputs(); });
            
            ConsoleGUIBase.instance.consoleInput.onDeselect
                .AddListener(delegate 
                    {EnableOtherInputs(default); });
        }
        
        private void RemoveEventListeners()
        {
            SceneManager.sceneLoaded -= CheckForSceneWithInputs;
            SceneManager.sceneUnloaded -= OnsceneUnloaded;
            
            ConsoleGUIBase.instance.consoleInput.onSubmit.RemoveAllListeners();
            ConsoleGUIBase.instance.consoleInput.onValueChanged.RemoveAllListeners();
            ConsoleGUIBase.instance.consoleInput.onSelect.RemoveAllListeners();
            ConsoleGUIBase.instance.consoleInput.onDeselect.RemoveAllListeners();
            
        }
    }
}