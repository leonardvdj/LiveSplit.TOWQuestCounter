using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.UI.Components
{
    class Quest
    {
        public ulong Offset { get; set; }
        public string Name { get; set; }
        public bool Completed { get; set; } = false;
        public int CompletionState { get; set; }
    }
}
