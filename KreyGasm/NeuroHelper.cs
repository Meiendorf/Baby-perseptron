using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace KreyGasm
{
    class ImageNeuron : INeuron2
    {
        public int[,] weights { get; set; }

        public int GetResult(int[,] input)
        {
            int sum = 0;
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    sum += weights[i, j] * input[i, j];
                }
            }
            return sum;
        }

        public void Study(int[,] input, int factor)
        {
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    weights[i, j] += input[i, j] * factor;
                }
            }
        }
        public void Degradate(int[,] input, int factor)
        {
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    weights[i, j] -= input[i, j] * factor;
                }
            }
        }
    }
    class ImageNeuralNetwork : INeuralNetwork2
    {
        public INeuron2[] Neurons { get; set; }

        public ImageNeuralNetwork(int neurons, int width, int heights)
        {
            Neurons = new ImageNeuron[neurons];
            for (int i = 0; i < neurons; i++)
            {
                Neurons[i] = new ImageNeuron();
                Neurons[i].weights = new int[width, heights];
            }
        }
        public int GetAnswer(int[,] input)
        {
            int max_index = -1;
            int max_res = -1;
            for (int i = 0; i < Neurons.Length; i++)
            {
                if (Neurons[i].GetResult(input) > max_res)
                {
                    max_res = Neurons[i].GetResult(input);
                    max_index = i;
                }
            }
            return max_index;
        }

        public void Study(int Neuron, int[,] input, int factor)
        {
            Neurons[Neuron].Study(input, factor);
        }
        public void Degradate(int Neuron, int[,] input, int factor)
        {
            Neurons[Neuron].Degradate(input, factor);
        }

        public void SaveWeights()
        {
            for (int i = 0; i < Neurons.Length; i++)
            {
                var fl = File.Open(String.Format("Weights/{0}.txt", i), FileMode.Create);
                fl.Close();
                using (StreamWriter sw = new StreamWriter(String.Format("Weights/{0}.txt", i)))
                {
                    for (int j = 0; j < Neurons[i].weights.GetLength(0); j++)
                    {
                        for (int k = 0; k < Neurons[i].weights.GetLength(1); k++)
                        {
                            sw.Write(Neurons[i].weights[j, k]);
                            if (k < Neurons[i].weights.GetLength(1) - 1)
                                sw.Write(",");
                        }
                        if (j < Neurons[i].weights.GetLength(0) - 1)
                            sw.Write("\n");
                    }
                }
            }
        }
        public void LoadWeights(string path)
        {
            for (int i = 0; i < Neurons.Length; i++)
            {
                using (StreamReader sr = new StreamReader(String.Format(path+"/{0}.txt", i)))
                {
                    for (int j = 0; j < Neurons[i].weights.GetLength(0); j++)
                    {
                        string[] str = sr.ReadLine().Split(',');
                        for (int k = 0; k < Neurons[i].weights.GetLength(1); k++)
                        {
                            Neurons[i].weights[j, k] = int.Parse(str[k]);
                        }
                    }
                }
            }
        }
    }
}