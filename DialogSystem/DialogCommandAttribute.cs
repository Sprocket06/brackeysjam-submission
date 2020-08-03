using System;

namespace Projection.DialogSystem
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DialogCommandAttribute : Attribute
    {
        public string CommandName { get; }

        public DialogCommandAttribute(string commandName)
        {
            CommandName = commandName;
        }
    }
}