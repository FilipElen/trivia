using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trivia
{
    class ConsoleOutputter : IOutputter
    {
        public void GenerateOutput(string message)
        {
            Console.WriteLine(message);
        }
    }
}
