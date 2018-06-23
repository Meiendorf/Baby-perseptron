using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace KreyGasm
{
    public interface INeuron2
    {
        int[,] weights { get; set; }

        int GetResult(int[,] input);

        void Study(int[,] input, int factor);

        void Degradate(int[,] input, int factor);

    }
}
