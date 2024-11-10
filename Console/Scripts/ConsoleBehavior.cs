using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Game.Meta.Console.Scripts.GUI;
using Game.Meta.Heuristics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Meta.Console.Scripts
{
    public class ConsoleBehavior : MonoBehaviour
    {
        //todo: store some values that used only in console
        public readonly Dictionary<string, CommandsByPrefix> prefixesDict
            = new Dictionary<string, CommandsByPrefix>();

        private List<string> inputHistory;

        private StringBuilder sbCopyable;
        
        private StringBuilder sbFormatted;

        private sbyte historyIndex;

        private const string errPrefixRT = "<color=#f53214ff>[ERRORLOG]</color>";
        private const string warnPrefixRT = "<color=#ffeb04ff>[WARNLOG]</color>";
        private const string defaultPrefixRT = "<color=#82d2bcff>[LOG]</color>";
        
        private const string evaluableStart = "f*";
        private const string evaluableEnd = "*";
        private const string relativeParamChar= "#";

        private const string blankOutput = "<color=#808080FF>] </color>";

        /*
        const string errPrefix = "[ERRORLOG] ";
        const string warnPrefix = "[WARNLOG] ";
        const string defaultPrefix = "[LOG] ";
        const uint logRedHEX = 0xF53214FF;
        const uint logYellowHEX = 0xFFEB04FF;
        const uint logGreyHEX = 0x82D2BCFF;
        */

        public static ConsoleBehavior instance;
        private char value;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("ConsoleBehavior has already presented");
                return;
            }
            instance = this;
            DontDestroyOnLoad(this);

            inputHistory = new List<string>(50);
            
            sbCopyable = new StringBuilder(0,75000);
            
            sbFormatted = new StringBuilder(0,75000);
            
            AddCommandsByPref();
            
            
        }
        
        private void AddCommandsByPref()
        {
            prefixesDict.Add(cn_init.cn_Commands.prefix, cn_init.cn_Commands);
            prefixesDict.Add(h_init.h_Commands.prefix, h_init.h_Commands);
            prefixesDict.Add(so_init.so_Commands.prefix, so_init.so_Commands);
        }

        public void UsersInput(string textSentByUser)
        {
            ConsoleGUIBase.instance.consoleInput.text = null;
            ResetConsoleHistory();

            if (string.IsNullOrWhiteSpace(textSentByUser))
            {
                WriteInOutput(blankOutput);
                return;
            }

            inputHistory.Insert(0, textSentByUser.Trim());
            WriteInOutput(blankOutput + textSentByUser.NoParseRT());

            string[] splittedText = textSentByUser.Split(' ');
            string potentialCommandFullName = splittedText[0];
            
            if (prefixesDict.TryGetValue(CommandsByPrefix.GetPrefixInFullNameCommand(potentialCommandFullName), 
                    out CommandsByPrefix cmds))
            { ExecuteCommand(cmds.commandFullNames[potentialCommandFullName], splittedText); }

            ConsoleGUIBase.instance.consoleInput.ActivateInputField();
        }

        private void ExecuteCommand(ConsoleCommand command, string[] splittedText)
        {
            try { if (SeekForUniversalParameter(command, splittedText[1])) { return; } }
            catch { /* */ }

            byte commandParamCount = command.paramCount;
            string[] args = GetCommandArguments(splittedText, commandParamCount);

            bool hasRelParam = args.Any(arg => arg.Contains(relativeParamChar));
            
            if (hasRelParam && command is IUseRelativeParameter relativeCommand)
            {
                for (int i = 0; i < commandParamCount; i++)
                {
                    if (!args[i].Contains(relativeParamChar)) { continue; }
                    
                    args[i] = args[i].Replace(relativeParamChar,
                        relativeCommand.ChangeRelativeParameters(args, i));
                }
                
            }
            
            args = args.Select(TryEvaluate).ToArray();
            
            command.RunCommand(args, out string runOutput);
            WriteInOutput(command.CommandOutput(args, runOutput));
        }

        private static string[] GetCommandArguments(string[] splittedText, byte commandParamCount)
        {
            var args = Enumerable.Repeat(string.Empty, commandParamCount).ToArray();
            int j = 0;

            for (int i = 1; i < splittedText.Length && j < commandParamCount; i++)
            {
                if (string.IsNullOrWhiteSpace(splittedText[i])) { continue; }
                args[j++] = splittedText[i];
            }
            return args;
        }
        
        private static bool SeekForUniversalParameter(ConsoleCommand command, string @param)
        {
            switch (@param)
            {
                case "help":
                    instance.WriteInOutput(command.helpMsg);
                    break;
                case "status":
                    instance.WriteInOutput(command.status);
                    break;
                default:
                    return false;
            }

            return true;
        }

        private static string TryEvaluate(string input)
        {
            if (!input.StartsWith(evaluableStart) || !input.EndsWith(evaluableEnd)) { return input; }
            
            input = input.Remove(input.Length - 1).Remove(0, 2);

            if (ExpressionEvaluator.Evaluate(input, out float result))
            {
                return result.ToString(CultureInfo.InvariantCulture);
            }

            Debug.LogWarning("An error occured while trying calculate expression: \""+ input + "\"");
            return ConsoleCommand.notDefined;

        }
        
        public void PushHistoryForward(InputAction.CallbackContext c)
        {
            if (inputHistory.Count < 1) { return; }

            historyIndex = (sbyte) ((historyIndex + 1) % inputHistory.Count);
            ConsoleGUIBase.instance.consoleInput.SetTextWithoutNotify
                (inputHistory[historyIndex]);

            ConsoleGUIBase.instance.consoleInput.MoveToEndOfLine(false,false);
        }
        
        public void PushHistoryBackward(InputAction.CallbackContext c)
        {
            if (inputHistory.Count < 1) { return; }

            if (historyIndex == -1) { historyIndex = 0;}

            historyIndex = (sbyte) ((historyIndex - 1 + inputHistory.Count) % inputHistory.Count);
            ConsoleGUIBase.instance.consoleInput.SetTextWithoutNotify
                (inputHistory[historyIndex]);
            
            ConsoleGUIBase.instance.consoleInput.MoveToEndOfLine(false,false);
        }
        
        public void ResetConsoleHistory() { historyIndex = -1; }
        
        public static void CatchLogs(string condition, string trace, LogType type)
        {
            string context  = DateTime.Now.ToString("HH:mm:ss.f") + " / " + condition;

            string message = type switch
            {
                LogType.Error => errPrefixRT,
                LogType.Exception => errPrefixRT,
                LogType.Warning => warnPrefixRT,
                LogType.Log => defaultPrefixRT,
                LogType.Assert => defaultPrefixRT,
                _ => ConsoleCommand.notDefined
            };

            instance.WriteInOutput(message + " " + context);

        }
        
        private void WriteInOutput(string text)
        {
            if (string.IsNullOrEmpty(text)) { return;}
            try
            {
                sbFormatted.AppendLine(text);
                sbCopyable.AppendLine( HRichText.RemoveRichTextTags(text) );
                
                ConsoleGUIBase.instance.consoleOutputFormattedText.text = sbFormatted.ToString();
                ConsoleGUIBase.instance.consoleOutputCopyableText.text = sbCopyable.ToString();
            }
            catch
            {
                ClearConsole();
                
                try
                {
                    sbFormatted.AppendLine(text);
                    sbCopyable.AppendLine( HRichText.RemoveRichTextTags(text) );

                }
                catch (Exception e)
                {
                    ClearConsole();
                    Debug.LogError(e);
                
                }
                
            }
            
        }

        public void ClearConsole()
        {
            sbFormatted.Clear();
            ConsoleGUIBase.instance.consoleOutputFormattedText.text = string.Empty;
            
            sbCopyable.Clear();
            ConsoleGUIBase.instance.consoleOutputCopyableText.text = string.Empty;
            
        }

    }
    
}
