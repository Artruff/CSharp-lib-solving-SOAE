
using System;

namespace CSharp_lib_solving_SOAE
{
    public class IterationMethod
    {
        public float[] SolveMatrix(float[][] argumentsMatrix, float[] valuesMatrix)
        {
            if (argumentsMatrix.Length != valuesMatrix.Length && argumentsMatrix[0].Length != argumentsMatrix.Length)
                throw new ArgumentException("Матрица аргументов и матрица значений разного размера.");
            float[] result = new float[valuesMatrix.Length];
            float[][] tmpMatrix = (float[][])argumentsMatrix.Clone();
            float[] tmpValues = (float[])valuesMatrix.Clone();
            double eps = 0.001;

            BringingToDiagonalDominance(ref tmpMatrix, tmpValues);
            float MatrixNorm = GetMatrixNorm(tmpMatrix);

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = tmpValues[i] / tmpMatrix[i][i];
            }

            float[] Xn = new float[tmpValues.Length];

            do
            {
                for (int i = 0; i < Xn.Length; i++)
                {
                    Xn[i] = tmpValues[i] / tmpMatrix[i][i];
                    for (int j = 0; j < Xn.Length; j++)
                    {
                        if (i == j)
                            continue;
                        else
                        {
                            Xn[i] -= tmpMatrix[i][j] / tmpMatrix[i][i] * result[j];
                        }
                    }
                }

                bool flag = true;
                for (int i = 0; i < Xn.Length - 1; i++)
                {
                    if (!CheckEndCondition(MatrixNorm, Xn[i], result[i], eps))
                    {
                        flag = false;
                        break;
                    }
                }

                for (int i = 0; i < Xn.Length; i++)
                {
                    result[i] = Xn[i];
                }

                if (flag)
                    break;
            } while (true);

            return result;
        }

        private void BringingToDiagonalDominance(ref float[][] tmpMatrix, float[] tmpValues)
        {
            (int startPosition, int maxPosition) position = (0, 0);

            for (int j = 0; j < tmpMatrix.Length; j++)
            {
                bool flag = true;
                float maxValue = tmpMatrix[j][j];
                for (int i = j + 1; i < tmpMatrix[0].Length; i++)
                {
                    if (Math.Abs(maxValue) < Math.Abs(tmpMatrix[i][j]))
                    {
                        flag = false;
                        maxValue = tmpMatrix[i][j];
                        position = (j, i);
                    }
                }
                if (flag == false)
                    ChangeRows(ref tmpMatrix, position, tmpValues);
            }
        }

        private void ChangeRows<T>(ref T[][] array, (int, int) Position, float[] tmpValues)
        {
            T[] Row = new T[array.Length];
            float tmpValue;
            for (int j = 0; j < array.Length; j++)
            {
                Row[j] = array[Position.Item2][j];
                array[Position.Item2][j] = array[Position.Item1][j];
                array[Position.Item1][j] = Row[j];
            }
            tmpValue = tmpValues[Position.Item2];
            tmpValues[Position.Item2] = tmpValues[Position.Item1];
            tmpValues[Position.Item1] = tmpValue;
        }

        private float GetMatrixNorm(float[][] tmpMatrix)
        {
            float SumСoefficients = 0;
            for (int i = 0; i < tmpMatrix.Length; i++)
            {
                for (int j = 0; j < tmpMatrix[0].Length; j++)
                {
                    if (j != i)
                        SumСoefficients += (float)Math.Pow(tmpMatrix[i][j] / tmpMatrix[i][i], 2);
                }
            }
            float MatrixNorm = (float)Math.Sqrt(SumСoefficients);
            return MatrixNorm;
        }

        private bool CheckEndCondition(float MatrixNorm, float Xn, float prevXn, double eps)
        {
            bool flag = true;
            if (MatrixNorm <= 0.5)
            {
                if (Math.Abs(Xn - prevXn) > eps)
                    flag = false;
            }
            else
            {
                if (Math.Abs(Xn - prevXn) > ((1 - MatrixNorm) / MatrixNorm) * eps)
                    flag = false;
            }
            return flag;
        }
    }
}
