using System;

namespace CSharp_lib_solving_SOAE
{
    public enum SelectingMainElement
    {
        Horizontal,
        Vertical,
        HorizontalAndVertical
    }
    public class MainElementMethod
    {
        SelectingMainElement _selecting;
        SelectingMainElement selecting
        {
            get => _selecting;
        }
        public MainElementMethod(SelectingMainElement selecting) => _selecting = selecting;
        public decimal[] SolveMatrix(decimal[,] argumentsMatrix, decimal[] valuesMatrix)
        {
            if (argumentsMatrix.GetLength(1) != valuesMatrix.Length && argumentsMatrix.GetLength(1) != argumentsMatrix.GetLength(0))
                throw new ArgumentException("Матрица аргументов и матрица значений разного размера.");
            decimal[] result = new decimal[valuesMatrix.Length];
            decimal[,] tmpMatrix = (decimal[,])argumentsMatrix.Clone();
            decimal[] tmpValues = (decimal[])valuesMatrix.Clone();
            int[] changingValues = new int[valuesMatrix.Length];
            for (int i = 0; i < valuesMatrix.Length; i++)
                changingValues[i] = i;

            for (int i = 0; i < argumentsMatrix.GetLength(0); i++)
            {
                for(int j = i+1; j< argumentsMatrix.GetLength(0); j++)
                {
                    decimal modValue = tmpMatrix[j, i] / tmpMatrix[i, i];
                    for (int n = i; n < argumentsMatrix.GetLength(1); n++)
                    {
                        tmpMatrix[j, n] -= modValue *tmpMatrix[i, n];
                    }
                    tmpValues[j] -= modValue * tmpValues[i];
                }
            }

            for(int i = result.Length-1; i>=0; i--)
            {
                for(int j = result.Length-1; j>i; j--)
                {
                    result[i] += result[j] * tmpMatrix[i, j];
                }

                result[i] = (tmpValues[i] - result[i])/tmpMatrix[i,i];
            }

            return result;
        }
        private void SelectMainElement(int iteration, decimal[,] argumentsMatrix, decimal[] valuesMatrix, int[] changingValues)
        {
            (int x, int y) mainElement = (iteration, iteration);
            for (int i = iteration; i < argumentsMatrix.GetLength(1); i++)
            {
                switch (selecting)
                {
                    case SelectingMainElement.Vertical:
                        if(Math.Abs(argumentsMatrix[i, iteration]) > Math.Abs(argumentsMatrix[mainElement.x, mainElement.y]))
                        {
                            ChangeElements<int>(changingValues, i, iteration);
                            ChangeElements<decimal>((decimal[])argumentsMatrix.GetValue(i), i, iteration);
                        }
                        break;
                }
            }
        }
        private void ChangeElements<T>(T[] array, int x, int y)
        {
            T place = array[x];
            array[x] = array[y];
            array[y] = place;
        }
    }
}
