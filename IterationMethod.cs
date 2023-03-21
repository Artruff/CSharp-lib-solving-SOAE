using System;

namespace CSharp_lib_solving_SOAE
{
    public class IterationMethod
    {
        public float[] SolveMatrix(float[,] argumentsMatrix, float[] valuesMatrix)
        {
            if (argumentsMatrix.GetLength(1) != valuesMatrix.Length && argumentsMatrix.GetLength(1) != argumentsMatrix.GetLength(0))
                throw new ArgumentException("Матрица аргументов и матрица значений разного размера.");
            float[] result = new float[valuesMatrix.Length];
            float[,] tmpMatrix = (float[,])argumentsMatrix.Clone();
            float[] tmpValues = (float[])valuesMatrix.Clone();
            double eps = 0.001;
            (int startPosition, int maxPosition) position = (0, 0);

            for (int j = 0; j < argumentsMatrix.GetLength(0); j++)
            {
                bool flag = true;
                float maxValue = tmpMatrix[j, j];
                for (int i = j + 1; i < argumentsMatrix.GetLength(1); i++)
                {
                    if (maxValue < tmpMatrix[i, j])
                    {
                        flag = false;
                        maxValue = tmpMatrix[i, j];
                        position = (j, i);
                    }
                }
                if (flag == false)
                    ChangeRows(tmpMatrix, position, tmpValues);
            }

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = tmpValues[i] / tmpMatrix[i, i];
            }

            float[] Xn = new float[tmpValues.Length];

            do
            {
                for (int i = 0; i < Xn.Length; i++)
                {
                    Xn[i] = tmpValues[i] / tmpMatrix[i, i];
                    for (int j = 0; j < Xn.Length; j++)
                    {
                        if (i == j)
                            continue;
                        else
                        {
                            Xn[i] -= tmpMatrix[i, j] / tmpMatrix[i, i] * result[j];
                        }
                    }
                }

                bool flag = true;
                for (int i = 0; i < Xn.Length - 1; i++)
                {
                    if (Math.Abs(Xn[i] - result[i]) > eps)
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

        private void ChangeRows<T>(T[,] array, (int, int) Position, float[] tmpValues)
        {
            T[] Row = new T[array.GetLength(0)];
            float tmpValue;
            for (int j = 0; j < array.GetLength(0); j++)
            {
                Row[j] = array[Position.Item2, j];
                array[Position.Item2, j] = array[Position.Item1, j];
                array[Position.Item1, j] = Row[j];
            }
            tmpValue = tmpValues[Position.Item2];
            tmpValues[Position.Item2] = tmpValues[Position.Item1];
            tmpValues[Position.Item1] = tmpValue;
        }
    }
}