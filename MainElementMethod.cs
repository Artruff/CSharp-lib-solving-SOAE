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
        public decimal[] SolveMatrix(decimal[][] argumentsMatrix, decimal[] valuesMatrix)
        {
            if (argumentsMatrix.Length != valuesMatrix.Length && argumentsMatrix.Length != argumentsMatrix[0].Length)
                throw new ArgumentException("Матрица аргументов и матрица значений разного размера.");
            decimal[] result = new decimal[valuesMatrix.Length];
            decimal[][] tmpMatrix = (decimal[][])argumentsMatrix.Clone();
            decimal[] tmpValues = (decimal[])valuesMatrix.Clone();
            int[] changingValues = new int[valuesMatrix.Length];
            for (int i = 0; i < valuesMatrix.Length; i++)
                changingValues[i] = i;

            for (int i = 0; i < argumentsMatrix.Length; i++)
            {
                SelectMainElement(i, tmpMatrix, tmpValues, changingValues);

                for(int j = i+1; j< argumentsMatrix.Length; j++)
                {
                    decimal modValue = tmpMatrix[j][i] / tmpMatrix[i][i];
                    for (int n = i; n < argumentsMatrix.Length; n++)
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
                    result[changingValues[i]] += result[j] * tmpMatrix[i][j];
                }

                result[changingValues[i]] = (tmpValues[i] - result[changingValues[i]])/tmpMatrix[i][i];
            }

            return result;
        }
        private void SelectMainElement(int iteration, decimal[][] argumentsMatrix, decimal[] valuesMatrix, int[] matrixX)
        {
            (int x, int y) mainElement = (iteration, iteration);
            for (int i = iteration; i < argumentsMatrix.Length; i++)
            {
                switch (selecting)
                {
                    case SelectingMainElement.Horizontal:
                        if(Math.Abs(argumentsMatrix[i][iteration]) > Math.Abs(argumentsMatrix[mainElement.x][mainElement.y]))
                        {
                            ChangeHorizontalElements<decimal>(valuesMatrix, iteration, i);
                            ChangeHorizontalElements<decimal>(argumentsMatrix[i], i, iteration);
                        }
                        break;
                    case SelectingMainElement.Vertical:
                        if (Math.Abs(argumentsMatrix[iteration][i]) > Math.Abs(argumentsMatrix[mainElement.x][mainElement.y]))
                        {
                            ChangeVerticalElements<decimal>(argumentsMatrix, iteration, i, iteration);
                            ChangeHorizontalElements<int>(matrixX, i, iteration);
                        }
                        break;
                    case SelectingMainElement.HorizontalAndVertical:
                        for (int j = iteration; j < argumentsMatrix.Length; j++)
                            if (Math.Abs(argumentsMatrix[i][j]) > Math.Abs(argumentsMatrix[mainElement.x][mainElement.y]))
                            {
                                ChangeHorizontalElements<decimal>(valuesMatrix, j, i);
                                ChangeHorizontalElements<decimal>(argumentsMatrix[i], i, j);
                                ChangeVerticalElements<decimal>(argumentsMatrix, j, i, j);
                                ChangeHorizontalElements<int>(matrixX, i, j);
                            }
                        break;
                }
            }
        }
        private void ChangeHorizontalElements<T>(T[] array, int x1, int x2)
        {
            T place = array[x1];
            array[x1] = array[x2];
            array[x2] = place;
        }
        private void ChangeVerticalElements<T>(T[][] array,int column, int y1, int y2)
        {
            T place = array[y1][column];
            array[y1][column] = array[y2][column];
            array[y2][column] = place;
        }
    }
}
