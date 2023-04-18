using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_lib_solving_SOAE
{
    public interface ISolverMatrix
    {
        public float[] SolveMatrix(float[][] argumentsMatrix, float[] valuesMatrix);
    }
}
