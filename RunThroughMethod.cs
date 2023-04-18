using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_lib_solving_SOAE
{
    public class RunThroughMethod : ISolverMatrix
    {
        public float[] SolveMatrix(float[][] argumentsMatrix, float[] valuesMatrix)
        {
            if (argumentsMatrix.Length != valuesMatrix.Length && argumentsMatrix.Length != argumentsMatrix[0].Length)
                throw new ArgumentException("Матрица аргументов и матрица значений разного размера.");
            float[] result = new float[valuesMatrix.Length],
                aArguments = new float[valuesMatrix.Length],
                bArguments = new float[valuesMatrix.Length];

            aArguments[0] = 0;
            bArguments[0] = 0;

            aArguments[1] = argumentsMatrix[0][1] / argumentsMatrix[0][0];
            bArguments[1] = valuesMatrix[0] / argumentsMatrix[0][0];

            for (int i = 2; i<valuesMatrix.Length; i++)
            {
                aArguments[i] = argumentsMatrix[i - 1][i] / (argumentsMatrix[i - 1][i - 1] - argumentsMatrix[i - 1][i - 2] * aArguments[i - 1]);
                bArguments[i] = (valuesMatrix[i-1] - argumentsMatrix[i - 1][i - 2] * bArguments[i - 1]) / (argumentsMatrix[i - 1][i - 1] - argumentsMatrix[i - 1][i - 2] * aArguments[i - 1]);
            }
            int index = valuesMatrix.Length - 1;
            result[index] = (valuesMatrix[index] - argumentsMatrix[index][index - 1] * bArguments[index])
                / (argumentsMatrix[index][index] - argumentsMatrix[index][index-1] * aArguments[index]);
            for (int i = index - 1; i >= 0; i--)
                result[i] = bArguments[i + 1] - aArguments[i + 1] * result[i + 1];

            return result;
        }
    }
}
