using System;
using Game.Meta.Heuristics;
using UnityEngine;

namespace Game.Meta.Console.Scripts
{
    static class cn_init
    {
        public static readonly CommandsByPrefix cn_Commands = new CommandsByPrefix
        (
            new ConsoleCommand[]
            {
                new LogSampleCmd(),
                new ClearCmd(),
                new LoggingOutputting(),
                new PutLoremImpsum()
                
            },
            "cn",
            "Commands that were used to test console implementation, GUI behavior"
        );
    }
    
    internal class LogSampleCmd : ConsoleCommand 
    {
        public override string command { get;  } 
            = "ls";

        public override string helpMsg { get; } 
            = "Write in console example of chosen log type." + Environment.NewLine +
              "command [type] [msg]" + Environment.NewLine +
              "type: [w] [l] [a] [e] [er]" + Environment.NewLine +
              "[w]: warning." + Environment.NewLine +
              "[l]: log." + Environment.NewLine +
              "[a]: assertion." + Environment.NewLine +
              "[e]: exception." + Environment.NewLine +
              "[er]: error." + Environment.NewLine +
              "msg: some text, could be empty ";

        public override byte paramCount { get; }
            = 2;

        public override string status { get; set; }

        public override void RunCommand(string[] @params, out string runOutput)
        {
            string type = @params[0];
            runOutput = null;
            string message = string.IsNullOrEmpty(@params[1]) 
                ? "Sample message" 
                : @params[1];
            

            switch (type)
            {
                case "w":
                    Debug.LogWarning(message);
                    break;
                case "l":
                    Debug.Log(message);
                    break;
                case "a":
                    Debug.LogAssertion(message);
                    break;
                case "e":
                    Debug.LogException(new Exception(message));
                    break;
                case "er":
                    Debug.LogError(message);
                    break;
            }

        }

        public override string CommandOutput(string[] strings, string runOutput = null) 
        { return null; }
        
    }

    internal class LoggingOutputting : ConsoleCommand 
    {
        public override string command { get; }
            = "lop";

        public override string helpMsg { get; }
            = "Toggle insertion of all log types in console." + onlyBooleanArgument;

        public override byte paramCount { get; }
            = 1;

        public override string status { get; set; }
            = "false";

        public override void RunCommand(string[] @params, out string runOutput)
        {
            runOutput = null;
            if (!@params[0].TryConvertToBoolean(out bool result) || Boolean.Parse(status) == result) { return; }
            if (result)
            {
                Application.logMessageReceived += ConsoleBehavior.CatchLogs;
                status = "true";
            }
            else 
            {
                Application.logMessageReceived -= ConsoleBehavior.CatchLogs;
                status = "false";
            }


        }

        public override string CommandOutput(string[] @params, string runOutput = null)
        {
            if (runOutput == null) { return null; }
            return notDefined;
            
        }
        
    }
    
    internal class ClearCmd : ConsoleCommand 
    {
        public override string command { get; }
            = "clear";

        public override string helpMsg { get; }
            = "Clear console's history";

        public override byte paramCount { get; }
            = 0;

        public override string status { get; set; }

        public override void RunCommand(string[] @params, out string runOutput)
        { ConsoleBehavior.instance.ClearConsole(); runOutput = null; }

        public override string CommandOutput(string[] @params, string runOutput = null)
        { return null; }
    }

    internal class PutLoremImpsum : ConsoleCommand
    {
        public override string command { get; }
            = "loremipsum";
        public override string helpMsg { get; }
            = "Write in console dummy text of chosen size" + Environment.NewLine +
        "command [size]" + Environment.NewLine +
        "type: [1] [2] [3] [4]";

        public override byte paramCount { get; }
            = 1;

        public override string status { get; set; }

        public override void RunCommand(string[] @params, out string runOutput)
        { runOutput = null; }

        public override string CommandOutput(string[] @params, string runOutput = null)
        {
            string size = @params[0];

            switch (size)
            {
                case "1":
                    return LoremIpsum.OnePar;
                case "2":
                    return LoremIpsum.TwoPar;
                case "3":
                    return LoremIpsum.ThreePar;
                case "4":
                    return LoremIpsum.FourPar;
                default:
                    return null;
            }
            
        }
        
    }
    
}