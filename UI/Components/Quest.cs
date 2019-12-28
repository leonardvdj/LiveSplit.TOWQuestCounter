using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.UI.Components
{
    class Quest
    {
        public Quest(string _name, MultiPointer[] _pointers)
        {
            Name = _name;
            Pointers = _pointers;
        }
        public MultiPointer[] Pointers { get; set; }
        public string Name { get; set; }
        public bool Completed { get; set; }
    }

    class MultiPointer
    {
        public MultiPointer(DeepPointer _pointer, Comparison[] _comparisons)
        {
            Pointer = _pointer;
            Comparisons = _comparisons;
        }
        public DeepPointer Pointer { get; set; }
        public bool Completed { get; set; }
        public Comparison[] Comparisons { get; set; }
    }

    class Comparison
    {
        public const int EQUALS = 0;
        public const int NOT_EQUALS = 1;
        public const int GREATER_THAN = 2;
        public const int GREATER_THAN_OR_EQUAL = 3;
        public const int LESS_THAN = 4;
        public const int LESS_THAN_OR_EQUAL = 5;
        public Comparison(int _completionState, int _comparator)
        {
            if (_comparator == EQUALS || _comparator == NOT_EQUALS)
            {
                CompletionStates = new int[] { _completionState };
                Comparator = _comparator;
            }
            else
            {
                CompletionState = _completionState;
                Comparator = _comparator;
            }
        }
        public Comparison(int[] _completionStates, int _comparator)
        {
            CompletionStates = _completionStates;
            Comparator = _comparator;
        }
        public int[] CompletionStates { get; set; }
        public int CompletionState { get; set; } = -1;
        public bool Completed { get; set; }
        public int Comparator { get; set; }
    }
}
