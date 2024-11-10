using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Game.Meta.Console.Scripts
{
    
    public class CommandsByPrefix
    {
        private const string notdefinedPrefix = "nd";
        
        public readonly string prefix;
        public readonly string helpAboutPrefix;
        public readonly Dictionary<string, ConsoleCommand> commandFullNames = new Dictionary<string, ConsoleCommand>();
        
        public CommandsByPrefix(ConsoleCommand[] list, string prefix = notdefinedPrefix, string helpAboutPrefix = ConsoleCommand.notDefined)
        {
            this.prefix = prefix;
            this.helpAboutPrefix = helpAboutPrefix;
            
            for (int i = 0; i < list.Length; i++)
            {
                commandFullNames.TryAdd(
                    prefix + "_" + list[i].command, // prefix + _ + command
                    list[i]);
            }
            
        }

        public static string GetPrefixInFullNameCommand(string potentialCommandFullName)
        {
           
            int index = potentialCommandFullName.IndexOf((char) ConsoleCommand.prefixSeparator);
            return index >= 0 
                ? potentialCommandFullName.Substring(0, index) 
                : "nd";
        }

    }
    
    /// <summary>
    /// Base class for creating console commands. Call in console happen in this small scheme
    /// prefix_command params[paramCount] -> RunCommand() -> CommandOutput
    /// </summary>
    public abstract class ConsoleCommand
    {
        internal const char prefixSeparator = '_';
        
        internal const string notDefined = "N\\A";

        internal const string invalidArg = "Invalid argument";

        internal static readonly string onlyBooleanArgument = Environment.NewLine +
                                                               "command [arg]" + Environment.NewLine +
                                                               boolArg;

        internal  const string noArg = "[arg]: " + notDefined;
        
        internal  const string boolArg = "[arg]: boolean. ";
        

        /// <summary>
        /// Command name that used to call it.
        /// </summary>
        public abstract string command { get;  }
        
        /// <summary>
        /// Ouputting if first parameter is "help". Catched in ConsoleBehavior.UserInput()
        /// </summary>
        public abstract string helpMsg { get; }
        
        /// <summary>
        /// A little workaround. Used to create blank parameters list in ConsoleBehavior.UserInput()
        /// and to prevent IndexOutOfRangeException
        /// </summary>
        public abstract byte paramCount { get; }
        
        /// <summary>
        /// Ouputting if first parameter is "status".
        /// Must be used if command changes some variable in some script. Store current status of behavior.
        /// Example: true/false state of some flag
        /// </summary>
        public abstract string status { get; set; }

        /// <summary>
        /// Called first if ConsoleBehavior cathes command inputting.
        /// </summary>
        /// <param name="params">Provided by ConsoleBehavior.UserInput()</param>
        /// <param name="runOutput">Can provide some information to CommandOutput() in ConsoleBehavior.UserInput(), null by default</param>
        public abstract void RunCommand(string[] @params, [CanBeNull] out string runOutput);

        /// <summary>
        /// Called after RunCommand(). Output of the method will be written in console. 
        /// </summary>
        /// <param name="params">Provided by ConsoleBehavior.UserInput()</param>
        /// <param name="runOutput">Provided by RunCommand()</param>
        /// <returns>Left null or empty to not wirte anything in console</returns>
        public abstract string CommandOutput(string[] @params, [CanBeNull] string runOutput = null);
    }

    /// <summary>
    /// Use if console command must somehow interpret '#' Relative parameter
    /// </summary>
    public interface IUseRelativeParameter
    {
        /// <param name="params">Could be useful for context</param>
        /// <param name="index">index in the param list's element that must be changed. </param>
        public string ChangeRelativeParameters(string[] @params, int index);

    }
}