using System;
using System.Text;

namespace Game.Meta.Console.Scripts
{
    static class h_init
    {
        public static readonly CommandsByPrefix h_Commands = new 
        (
            new ConsoleCommand[]
            {
                new HelpCmd(),
                new FeaturesCmd(),
                new ApplicationInfoCmd()
                 
            },
            "h",
            "Info about things, guides and other"
        );
    }

    internal class HelpCmd : ConsoleCommand
    {
        //TODO: make this part with json
        public override string command { get; }
            = "help";

        public override string helpMsg { get; }
            = "Put in console simple info about accessible console commands of chosen prefix. " +
              Environment.NewLine +  "[prefix_commandName help] may provide extra info";

        public override byte paramCount { get; }
            = 1;

        public override string status { get; set; }

        public override void RunCommand(string[] @params, out string runOutput)
        { runOutput = null;}

        public override string CommandOutput(string[] @params, string runOutput = null)
        {
            if (string.IsNullOrWhiteSpace(@params[0])) { return null; }

           
            return ConsoleBehavior.instance.prefixesDict.TryGetValue(@params[0], out CommandsByPrefix output) 
                ? PasteInfoByPrefix(output) 
                : notDefined;
        }   

        private static string PasteInfoByPrefix(CommandsByPrefix CommandsByPrefix)
        {
            StringBuilder sb = new StringBuilder(850);
            sb.Append("|---[" + CommandsByPrefix.prefix + "]" + Environment.NewLine + CommandsByPrefix.helpAboutPrefix + Environment.NewLine);
          
            foreach (var item in CommandsByPrefix.commandFullNames.Values)
            { sb.Append("I-  " + item.command + Environment.NewLine); }
            
            return sb.ToString();
        }
        
    }

    internal class FeaturesCmd : ConsoleCommand
    {
        public override string command { get; }
            = "features";

        public override string helpMsg { get; }
            = "Put in console not very obvious info about console's functionality";

        public override byte paramCount { get; }
            = 0;
        public override string status { get; set; }
        
        public override void RunCommand(string[] @params, out string runOutput)
        { runOutput = null; }

        public override string CommandOutput(string[] @params, string runOutput = null)
        {
            return "A. Window resize." + Environment.NewLine + 
                   "   1. You can resize console window using click-n-drag button in the top left corner." + Environment.NewLine + 
                   "   2. You also can resize console window from center by holding [Shift]." + Environment.NewLine + 
                   "B. Window moving." + Environment.NewLine + 
                   "   1. You can move console window on your screen by holding console's upper grey bar and dragging it." + Environment.NewLine + 
                   "C. Console's input history." + Environment.NewLine +
                   "   1. Get previous item - by pressing [Tab] or [UpArrow]" + Environment.NewLine +
                   "   2. Get next item - by double pressing [Tab] or by pressing [DownArrow]" + Environment.NewLine +
                   "D. RichText tags." + Environment.NewLine +
                   "   1. To use RichText tags if command's output can handle it, try leaving the \"</noparse>\" tag at the beggining of your text" + Environment.NewLine + 
                   "E. Computing in input." + Environment.NewLine + 
                   "   1. You can use \"f*EXPRESSION*\" construction to make some calculations in command's input." + Environment.NewLine + 
                   "   2. Expression can handle parentheses, arithmetic, power, modulo operators, sqrt(), floor(), ceil(), round(), cos(), sin(), tan(), pi" + Environment.NewLine +
                   "F. Univeras first parameter." + Environment.NewLine + 
                   "   1. \"help\" to get some info about command function, its parameters and output." + Environment.NewLine + 
                   "   2. \"status\" to get info about statuses that command functionality handles. " + Environment.NewLine +
                   "G. Relative argument." + Environment.NewLine +
                   "   1. Some commands that works with objects in scene and use arguments may use \"#\" as an argument." + Environment.NewLine +
                   "   2. Using \"#\" will use corresponding  object's parameter as an argument if possible. Possibility should be described in the \"help\" " +
                   "of the command (e.g. \"command [x]# [y]# [z]#\")" + Environment.NewLine +
                   "   3. You can use \"#\" in expressions that have been described in the \"E.\" of this guide." + Environment.NewLine +
                   "   3.1. Example. We have command that changes objects position (e.g x=3.0, y=2.0, z=-1.5) by using command \"obj_setpos [x] [y] [z]\". " +
                   "We can  type \"obj_setpos f*#*2* f*#-1* f*75+#*\" and object position will be (x=6.0, y=1.0, z=73.5) "
                ;
        }
        
    }
    
    internal class ApplicationInfoCmd : ConsoleCommand
    {
        public override string command { get; }
            = "app";

        public override string helpMsg { get; }
            = "Put some info about application ";

        public override byte paramCount { get; }
            = 0;
        
        public override string status { get; set; }
        
        public override void RunCommand(string[] @params, out string runOutput)
        {
            throw new NotImplementedException();
        }

        public override string CommandOutput(string[] @params, string runOutput = null)
        {
            throw new NotImplementedException();
        }
    }
}