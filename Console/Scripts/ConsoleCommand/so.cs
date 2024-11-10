using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Game.Meta.Console.Scripts.GUI;
using Game.Meta.Heuristics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Meta.Console.Scripts
{
    static class so_init
    {
        public static readonly CommandsByPrefix so_Commands = new CommandsByPrefix
        (
            new ConsoleCommand[]
            {
                new TryGetObjectCmd(),
                new RemoveSelectionCmd(),
                new DeleteObjectCmd(),
                new GetInfoCmd(),
                new SwitchCollisionCmd(),
                new ObjectsRendererCmd(),
                new CloneObjectCmd()
                
            },
            "so",
            "Commands that about manipulations with some selected object. The key command is \""
            + "so_getsel\"" + Environment.NewLine +
            "Object with Collision component are only available for selection."
            
        );
        
    }

    static class SelectObjectCmdUtils 
    {
        internal static GameObject selectedGameObject;
        internal static string identifier;

        internal static bool isObjectSelected;
        
        public static void TryToFindGameObject( Vector2 vect)
        {
            Ray ray = Camera.main!.ScreenPointToRay(vect);

            Physics.Raycast(Vector3.down, Vector3.back, 59f);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 69f))
            { SetSelection(raycastHit.transform.gameObject); }
            else { RemoveSelection(); }
            
            ConsoleGUIBase.instance.inputsHandler.DisableMouseActions();
        }

        public static void RemoveSelection()
        {
            ConsoleGUIBase.instance.consoleSelectedObjectInfo.gameObject.SetActive(false);
            selectedGameObject = null;
            isObjectSelected = false;
        }

        private static void SetSelection(GameObject go)
        {
            ConsoleGUIBase.instance.consoleSelectedObjectInfo.gameObject.SetActive(true);
            selectedGameObject = go;
            isObjectSelected = true;
            
            identifier = go.name +
                         "(S=" + go.isStatic +
                         " T=" + go.tag +
                         " L=" + LayerMask.LayerToName(go.layer) +")";
            
            ConsoleGUIBase.instance.consoleSelectedObjectInfo.text = identifier;
            
        }
        
    }
    
    internal class TryGetObjectCmd : ConsoleCommand
    {
        public override string command { get; }
            = "getsel";

        public override string helpMsg { get; }
            = "Use your mouse to click on some object on the scene. An object must have a collider to be available to for detection";

        public override byte paramCount { get; }
            = 0;
        
        public override string status { get; set; }

        public override void RunCommand(string[] @params, out string runOutput)
        {
            runOutput = null;
            ConsoleGUIBase.instance.inputsHandler.EnableMouseActions();
        }

        public override string CommandOutput(string[] @params, string runOutput = null)
        { return null; }

    }
    
    internal class RemoveSelectionCmd : ConsoleCommand
    {
        public override string command { get; }
            = "remsel";

        public override string helpMsg { get; }
            = "Use to remove selection";

        public override byte paramCount { get; }
            = 0;
        
        public override string status { get; set; }
        
        public override void RunCommand(string[] @params, out string runOutput)
        {
            runOutput = null;
            SelectObjectCmdUtils.RemoveSelection();
        }

        public override string CommandOutput(string[] @params, string runOutput = null)
        { return null; }
        
    }
    
    internal class DeleteObjectCmd : ConsoleCommand
    {
        public override string command { get; }
            = "deletefromscene";

        public override string helpMsg { get; }
            = "Dangerous command, deletes object and its children. " + onlyBooleanArgument 
                                                                     + "False does nothing actually";

        public override byte paramCount { get; }
            = 1;
        public override string status { get; set; }
        
        public override void RunCommand(string[] @params, out string runOutput)
        {
            runOutput = null;
            if (!SelectObjectCmdUtils.isObjectSelected) { return; }
            
            if (!@params[0].TryConvertToBoolean(out bool result)) 
            { runOutput = invalidArg; return; }

            if (result != true) { return; }
            
            Object.Destroy(SelectObjectCmdUtils.selectedGameObject);
            Debug.LogWarning(
                DateTime.Now.ToString("HH:mm:ss.f") + " / "
                                                    + SelectObjectCmdUtils.selectedGameObject.name
                                                    + " successfully deleted in the scene : " + SelectObjectCmdUtils.selectedGameObject.scene.name);
            SelectObjectCmdUtils.RemoveSelection();
        }

        public override string CommandOutput(string[] @params, string runOutput = null)
        { return null ; }
        
    }
    
    internal class GetInfoCmd : ConsoleCommand
    {
        public override string command { get; }
            = "info";

        public override string helpMsg { get; }
            = "Put some info about selected gameobject in console" + Environment.NewLine +
        "command [param]" + Environment.NewLine +
        "param: [transf] [tree] [comp] [instid] [hash]" + Environment.NewLine +
        "transf: various global and local transform data"+ Environment.NewLine +
        "tree: parent-child hierarchy (max 7 parents)" + Environment.NewLine +
        "comp: list of components, attached to selected object" + Environment.NewLine +
        "instid: gets the instance ID of the object" + Environment.NewLine +
        "hash: gets the instance ID of the object";

        public override byte paramCount { get; }
            = 1;
        
        public override string status { get; set; }
        
        public override void RunCommand(string[] @params, out string runOutput)
        { runOutput = null; }

        public override string CommandOutput(string[] @params, string runOutput = null)
        {
            if (!SelectObjectCmdUtils.isObjectSelected) return null;

            StringBuilder output = new StringBuilder();
            switch (@params[0])
            {
                case "tree":
                    const byte maxParent = 8;
                    int parPos = 0;

                    List<string> parentsHierachy = new List<string>();
                    Transform lastParent = SelectObjectCmdUtils.selectedGameObject.transform;
                    
                    output.Append("Parent-child tree:" + Environment.NewLine);
                    
                    while (parPos < maxParent)
                    {
                        try
                        {
                            parentsHierachy.Insert(0, lastParent.name);
                            lastParent = lastParent.parent;
                            parPos++;
                            if (parPos >= maxParent) { parentsHierachy.Insert(0, "..."); }
                        }
                        catch { break; }
                    }
                    
                    for (int i = 0; i < parPos; i++)
                    {
                        output.Append(parentsHierachy[i] + Environment.NewLine +
                                      (new string(' ', i*2)) + "L");
                    }

                    output.Append("[" + SelectObjectCmdUtils.selectedGameObject.name + "]" + Environment.NewLine);
            
                    foreach (Transform child in SelectObjectCmdUtils.selectedGameObject.transform)
                    { output.Append((new string(' ', parPos*2)) + "L" + child.name + Environment.NewLine); }
                    
                    break;
                case "comp":
                    var components = SelectObjectCmdUtils.selectedGameObject.GetComponents<Component>();
                    output.Append("Components:" + Environment.NewLine);
            
                    for (int i = 0; i < components.Length; i++)
                    { output.Append("[" + (i + 1) + "]. " + components[i].GetType().Name + Environment.NewLine); }
                    break;
                case "transf":
                    Transform t = SelectObjectCmdUtils.selectedGameObject.transform;
                    return SelectObjectCmdUtils.identifier + Environment.NewLine +
                           "P: " + t.position + ", R: " + t.rotation + " S: " + t.lossyScale + Environment.NewLine +
                           "LP: " + t.localPosition + ", LR: " + t.localRotation + " LS: " + t.localScale;
                case "instid":
                    return SelectObjectCmdUtils.selectedGameObject.GetInstanceID().ToString();
                case "hash":
                    return SelectObjectCmdUtils.selectedGameObject.GetHashCode().ToString();
            }
                
            return output.ToString();
        }
    }
    
    internal class SwitchCollisionCmd : ConsoleCommand
    {
        public override string command { get; }
            = "col";

        public override string helpMsg { get; }
            = "Trying to switch state of collision of gameobject" + onlyBooleanArgument;

        public override byte paramCount { get; }
            = 1;

        public override string status
        {
            get
            {
                return SelectObjectCmdUtils.selectedGameObject.TryGetComponent(out Collider collider)
                    ? collider.enabled.ToString() 
                    : notDefined;
            }
            set { throw new NotImplementedException(); }
        }


        public override void RunCommand(string[] @params, out string runOutput)
        {
            runOutput = null;
            if (!SelectObjectCmdUtils.isObjectSelected) { return; }

            if (!SelectObjectCmdUtils.selectedGameObject.TryGetComponent(out Collider collider))
            {
                runOutput = "Object doesn't have collider";
                return;
            }

            if (@params[0].TryConvertToBoolean(out bool result))
            { collider.enabled = result; }
            else { runOutput = invalidArg; }

        }

        public override string CommandOutput(string[] @params, string runOutput = null)
        { return runOutput; }
        
    }
    
    internal class ObjectsRendererCmd : ConsoleCommand
    {
        public override string command { get; }
            = "rend";
        
        public override string helpMsg { get; }
            = "Modify or retrieve information about the Renderer component of the selected GameObject." + Environment.NewLine +
              "command [param] [arg]" + Environment.NewLine +
              "param: [color] [isvisible] [enable]" + Environment.NewLine +
              "color: sets the color of the material. [arg]: single word (e.g. red, cyan) or #RRGGBBAA format of color." + Environment.NewLine +
              "isvisible: checks if the object is currently visible in the scene view or camera." + noArg + Environment.NewLine +
              "enable : switch the state of the Renderer. " + boolArg;

        public override byte paramCount { get; }
            = 2;
        
        public override string status { get; set; }

        public override void RunCommand(string[] @params, out string runOutput)
        {
            runOutput = null;
            if (!SelectObjectCmdUtils.isObjectSelected) { return; }

            if (!SelectObjectCmdUtils.selectedGameObject.TryGetComponent(out Renderer renderer)) return;
            
            switch (@params[0])
            {
                case "color":
                    if (ColorUtility.TryParseHtmlString(@params[1], out Color color))
                    { renderer.material.color = color; }
                    else { runOutput = "Failed to parse input."; }
                    break;
                case "isvisible":
                    runOutput = renderer.isVisible.ToString();
                    break;
                case "enable":
                    if (@params[1].TryConvertToBoolean(out bool result)) 
                    { renderer.enabled = result; }
                    else { runOutput = invalidArg; }
                        
                    break;
            }
        }

        public override string CommandOutput(string[] @params, string runOutput = null)
        { return runOutput; }
        
    }
    
    internal class CloneObjectCmd : ConsoleCommand, IUseRelativeParameter
    {
        public override string command { get; }
            = "clone";

        public override string helpMsg { get; }
            = "Creates clone of selected object." + Environment.NewLine +
              "command [x]# [y]# [z]#" + Environment.NewLine +
              "[x]# [y]# [z]# - float. Use only command to clone inside selected object.";

        public override byte paramCount { get; }
            = 3;
        public override string status { get; set; }
        public override void RunCommand(string[] @params, out string runOutput)
        {
            runOutput = null;
            if (!SelectObjectCmdUtils.isObjectSelected) { return; }
            
            if (string.IsNullOrWhiteSpace(@params[0])) { Object.Instantiate(SelectObjectCmdUtils.selectedGameObject); }
            else
            {
                Vector3 pos = new Vector3();
                for (int i = 0; i < 3; i++)
                {   
                    if (float.TryParse(@params[i], NumberStyles.Float,CultureInfo.InvariantCulture, out float num)) 
                    { pos[i] = num; }
                    else
                    {
                        runOutput = invalidArg;
                        return;
                    }
                    
                }
                
                Object.Instantiate(SelectObjectCmdUtils.selectedGameObject, pos, Quaternion.identity);
            }
            
        }

        public override string CommandOutput(string[] @params, string runOutput = null)
        { return runOutput; }

        public string ChangeRelativeParameters(string[] @params, int index)
        {
            return SelectObjectCmdUtils.selectedGameObject.transform.position[index]
                .ToString(CultureInfo.InvariantCulture);
        }
        
    }
}
