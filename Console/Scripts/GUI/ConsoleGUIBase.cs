using Game.Meta.Console.Scripts.Inputs;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Meta.Console.Scripts.GUI
{
    public class ConsoleGUIBase : MonoBehaviour
    {
        [HideInInspector] public Canvas consoleCanvas;
         
        [HideInInspector] public RectTransform baseRectTransform;
        [HideInInspector] public GameObject baseGameObject;
        
        [HideInInspector] public RectTransform outlineRectTransform;
        [HideInInspector] public GameObject outlineGameObject;

//
//-----------------------------------
//text vars

        public TMP_InputField consoleInput;
        
        public TMP_InputField consoleOutputCopyableText;

        public TMP_Text consoleOutputFormattedText;

        public TMP_InputField  consoleSelectedObjectInfo;

//
//-----------------------------------
//GUI instances

        public static ConsoleGUIBase instance;

        public ConsoleResizer resizerHandler;

        public ConsoleDragHandler dragHandler;
        
//
//-----------------------------------
//GUI instances
        public ConsoleInputsHandler inputsHandler;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("Console has already presented in scene \"" + gameObject.scene.name + "\"");
                return;
            }
            instance = this;
            
            consoleCanvas = GetComponentInParent<Canvas>();

            baseGameObject = transform.GetChild(0).gameObject;
            baseRectTransform = baseGameObject.GetComponent<RectTransform>();
            
            outlineGameObject = transform.GetChild(1).gameObject;
            outlineRectTransform = outlineGameObject.GetComponent<RectTransform>();

        }

        private void Start()
        {
            resizerHandler.Init(instance,
               new Vector2(200f, 150f),
               new Vector2(1000f, 1000f));
            
            dragHandler.Init(instance);

            inputsHandler = new ConsoleInputsHandler();
            inputsHandler.inputs = new ConsoleInputs();
            inputsHandler.inputs.Enabler.Enable();

        }

        public void SwitchActiveState(InputAction.CallbackContext c)
        {
            if (baseGameObject.activeSelf == false)
            {
                baseRectTransform.pivot = new Vector2(1f, 1f);
                baseRectTransform.anchoredPosition = Vector2.zero;
                
                outlineRectTransform.pivot = new Vector2(1f, 1f);
                outlineRectTransform.anchoredPosition = Vector2.zero;

                baseGameObject.SetActive(true);
                
                inputsHandler.inputs.History.Enable();
                inputsHandler.inputs.OtherGUI.Enable();
            }
            else
            {
                baseGameObject.SetActive(false);
                
                inputsHandler.inputs.History.Disable();
                inputsHandler.inputs.OtherGUI.Disable();

            }
            
        }

    }
    
}