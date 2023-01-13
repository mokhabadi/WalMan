using System;
using System.Threading.Tasks;

namespace WalMan
{
    internal class Command
    {
        public Func<Task> Action { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Command(Func<Task> action, string? description = null)
        {
            Action = action;
            Name = action.Method.Name;
            Description = description ?? Name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
