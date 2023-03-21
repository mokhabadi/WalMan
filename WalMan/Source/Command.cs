using System;
using System.Threading.Tasks;

namespace WalMan
{
    public class Command
    {
        public string Name { get; private set; }
        public Func<Task> Action { get; private set; }

        public Command(Func<Task> action)
        {
            Action = action;
            Name = action.Method.Name.ToSentenceCase();
        }

        public Command(Func<Task> action, string name)
        {
            Action = action;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
