using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipmt.Engine.AhoCorasick
{
    public class AhoCorasickMatchCommand : StringMatchCommand
    {
        public AhoCorasickMatchCommand(List<string> textFileNames, List<string> patterns, ResultHandler.Params handlerParams) : base(textFileNames, patterns, handlerParams)
        {
        }
        
        GoToTable goTo;

        int[] fail;
        List<List<int>> stateMatches = new List<List<int>>();

        HashSet<char> uniqueChars = new HashSet<char>();
        

        const int FAIL_VAL = -1;



        protected override void Train()
        {
            foreach (var pat in Patterns)
            {
                foreach (var chr in pat)
                {
                    uniqueChars.Add(chr);
                }
            };

            stateMatches = new List<List<int>>(Patterns.Count * 2);

            BuildGoto();
            BuildFail();
        }

        private void BuildGoto()
        {
            goTo = new GoToTable(Patterns, stateMatches);
        }
 
        private void BuildFail()
        {
            fail = new int[goTo.StateCount];

            var queue = new Queue<int>();
            foreach (var chr in uniqueChars)
            {
                int next = goTo[0, chr];
                if (next != 0)
                {
                    queue.Enqueue(next);
                }
            }
            while (queue.Count > 0)
            {
                int state = queue.Dequeue();
                foreach (var chr in uniqueChars)
                {
                    int child = goTo[state, chr];
                    if (child != FAIL_VAL)
                    {
                        queue.Enqueue(child);
                        int brdr = fail[state];
                        while (goTo[brdr, chr] == FAIL_VAL)
                        {
                            brdr = fail[brdr];
                        }

                        fail[child] = goTo[brdr, chr];
                        JoinAIntoB(fail[child], child);
                    }
                }
            }
        }
        private void JoinAIntoB(int a, int b)
        {
            stateMatches[b].AddRange(stateMatches[a].Where(x => !stateMatches[b].Contains(x)));
        }


        protected override void Match(string text)
        {
            ResultHandler[] resultHandlers = new ResultHandler[Patterns.Count];
            for (int i = 0; i < Patterns.Count; i++)
            {
                resultHandlers[i] = new ResultHandler(HandlerParams);
            }


            int state = 0;
            for (int i = 0; i < text.Length; i++)
            {
                while(goTo[state, text[i]] == FAIL_VAL)
                {
                    state = fail[state];
                }
                state = goTo[state, text[i]];


                for (int j = 0; j < stateMatches[state].Count; j++)
                {
                    resultHandlers[stateMatches[state][j]].OnNewMatch(text);
                }
                if (text[i] == '\n')
                {
                    foreach (var handler in resultHandlers)
                    {
                        handler.OnNewLine(i);
                    }
                }
            }
            for (int i = 0; i < Patterns.Count; i++)
            {
                resultHandlers[i].OnAlgorithmEnd(Patterns[i]);
            }
        }
    }

    class GoToTable
    {
        List<Dictionary<char, int>> dicts;
        List<List<int>> stateMatches;

        public int StateCount { get; private set; }

        public int this[int state, char chr]
        {
            get
            {
                int gt =  GoTo(state, chr);

                if (gt == -1 && state == 0)
                    return 0;
                else
                    return gt;
            }
        }

        private int GoTo(int state, char chr)
        {
            if (!dicts[state].TryGetValue(chr, out int next))
            {
                return -1;
            }
            return next;
        }


        public GoToTable(List<string> patterns, List<List<int>> stateMatches)
        {
            this.stateMatches = stateMatches;
            dicts = new List<Dictionary<char, int>>();
            CreateRoot();
            for (int i = 0; i < patterns.Count; i++)
            {
                AddPattern(patterns[i], i);
            }
        }
        private void AddPattern(string patt, int pattId)
        {
            int state = 0;

            for (int i = 0; i < patt.Length; i++)
            {
                int next = GoTo(state, patt[i]);
                if (next == -1)
                {
                    next = AddState();
                    dicts[state].Add(patt[i], next);
                }
                state = next;
            }
            stateMatches[state].Add(pattId);
        }
        private void CreateRoot()
        {
            AddState();
        }
        private int AddState()
        {
            var newDict = new Dictionary<char, int>();
            stateMatches.Add(new List<int>());

            dicts.Add(newDict);
            StateCount++;
            return StateCount - 1;
        }

    }
}

