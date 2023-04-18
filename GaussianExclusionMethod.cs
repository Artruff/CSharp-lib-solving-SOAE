using System;

namespace CSharp_lib_solving_SOAE
{
    public class GaussianExclusionMethod : ISolverMatrix
    {
        public float[] SolveMatrix(float[][] argumentsMatrix, float[] valuesMatrix)
        {
            if (argumentsMatrix.Length != valuesMatrix.Length && argumentsMatrix.Length != argumentsMatrix[0].Length)
                throw new ArgumentException("Матрица аргументов и матрица значений разного размера.");
            float[] result = new float[valuesMatrix.Length];
            float[][] tmpMatrix = (float[][])argumentsMatrix.Clone();
            float[] tmpValues = (float[])valuesMatrix.Clone();

            for (int i = 0; i < argumentsMatrix.Length; i++)
            {
                for(int j = i+1; j< argumentsMatrix.Length; j++)
                {
                    float modValue = tmpMatrix[j][i] / tmpMatrix[i][i];
                    for (int n = i; n < argumentsMatrix[0].Length; n++)
                    {
                        tmpMatrix[j][n] -= modValue *tmpMatrix[i][n];
                    }
                    tmpValues[j] -= modValue * tmpValues[i];
                }
            }

            for(int i = result.Length-1; i>=0; i--)
            {
                for(int j = result.Length-1; j>i; j--)
                {
                    result[i] += result[j] * tmpMatrix[i][j];
                }

                result[i] = (tmpValues[i] - result[i])/tmpMatrix[i][i];
            }

            return result;
        }
    }
}
