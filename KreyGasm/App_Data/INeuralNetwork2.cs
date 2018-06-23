using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KreyGasm
{
    public interface INeuralNetwork2
    {
        INeuron2[] Neurons { get; set; }

        int GetAnswer(int[,] input);

        void Study(int Neuron, int[,] input, int factor);

        void Degradate(int Neuron, int[,] input, int factor);
    }
}
