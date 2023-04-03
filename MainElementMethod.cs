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
        public float[] SolveMatrix(float[][] argumentsMatrix, float[] valuesMatrix)
        {
            if (argumentsMatrix.Length != valuesMatrix.Length && argumentsMatrix.Length != argumentsMatrix[0].Length)
                throw new ArgumentException("Матрица аргументов и матрица значений разного размера.");
            float[] result = new float[valuesMatrix.Length];
            float[][] tmpMatrix = (float[][])argumentsMatrix.Clone();
            float[] tmpValues = (float[])valuesMatrix.Clone();
            int[] changingValues = new int[valuesMatrix.Length];
            for (int i = 0; i < valuesMatrix.Length; i++)
                changingValues[i] = i;

            for (int i = 0; i < argumentsMatrix.Length; i++)
            {
                SelectMainElement(i, tmpMatrix, tmpValues, changingValues);

                for(int j = i+1; j< argumentsMatrix.Length; j++)
                {
                    float modValue = tmpMatrix[j][i] / tmpMatrix[i][i];
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
                    result[changingValues[i]] += result[changingValues[j]] * tmpMatrix[i][j];
                }

                result[changingValues[i]] = (tmpValues[i] - result[changingValues[i]])/tmpMatrix[i][i];
            }

            return result;
        }
        private void SelectMainElement(int iteration, float[][] argumentsMatrix, float[] valuesMatrix, int[] matrixX)
        {
            float maxElement = argumentsMatrix[iteration][iteration];
            (int x, int y) maxI = (iteration, iteration);
            switch (_selecting)
            {
                case SelectingMainElement.Horizontal:
                    for(int i = iteration+1; i<argumentsMatrix.Length; i++)
                    {
                        if (maxElement < Math.Abs(argumentsMatrix[iteration][i]))
                        {
                            maxElement = Math.Abs(argumentsMatrix[iteration][i]);
                            maxI.y = i;
                        }
                    }
                    break;
                case SelectingMainElement.Vertical:
                    for (int i = iteration + 1; i < argumentsMatrix.Length; i++)
                    {
                        if (maxElement < Math.Abs(argumentsMatrix[i][iteration]))
                        {
                            maxElement = Math.Abs(argumentsMatrix[i][iteration]);
                            maxI.x = i;
                        }
                    }
                    break;
                case SelectingMainElement.HorizontalAndVertical:
                    for (int i = iteration; i < argumentsMatrix.Length; i++)
                    {
                        for (int j = iteration; j < argumentsMatrix.Length; j++)
                        {
                            if (maxElement < Math.Abs(argumentsMatrix[i][j]))
                            {
                                maxElement = Math.Abs(argumentsMatrix[i][j]);
                                maxI.x = i;
                                maxI.y = j;
                            }
                        }
                    }
                    break;
            }
            if(maxI != (iteration, iteration))
            {
                if(maxI.y!= iteration)
                {
                    for (int i = 0; i < argumentsMatrix.Length; i++)
                        ChangeHorizontalElements<float>(argumentsMatrix[i], iteration, maxI.y);
                    ChangeHorizontalElements<int>(matrixX, iteration, maxI.y);
                }
                if (maxI.x != iteration)
                {
                    ChangeVerticalElements<float>(argumentsMatrix, iteration, maxI.x);
                    ChangeHorizontalElements<float>(valuesMatrix, iteration, maxI.x);
                }
            }
        }
        private void ChangeHorizontalElements<T>(T[] array, int x1, int x2)
        {
            T place = array[x1];
            array[x1] = array[x2];
            array[x2] = place;
        }
        private void ChangeVerticalElements<T>(T[][] array, int y1, int y2)
        {
            for(int j = 0; j<array.Length; j++)
            {

                T place = array[y1][j];
                array[y1][j] = array[y2][j];
                array[y2][j] = place;
            }
        }
    }
}
