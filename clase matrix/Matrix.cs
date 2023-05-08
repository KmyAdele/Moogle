using System.ComponentModel;
namespace Nueva_carpeta
{
    public class Matrix
    {
        /*
                columnas
                |
        filas_  0 0 0 0
                0 0 0 0
                0 0 0 0
                0 0 0 0
        */

        public int rows {get; private set;} 
        public int columns {get; private set;}
        public double[,] matrix_coef; 


        public Matrix(double[,]matrix)
        {
            this.rows = matrix.GetLength(0);
            this.columns = matrix.GetLength(1);
            this.matrix_coef = matrix;
        }

        public static void Multiply_by_escalar(Matrix matrix,double e)
        {
            for(int i=0; i<matrix.rows;i++)
            {
                for(int j=0; j<matrix.columns; j++)
                {
                    matrix.matrix_coef[i,j] = matrix.matrix_coef[i,j]*e;
                }
            }
        }

        public static Matrix Sum_Matrix(Matrix a, Matrix b)
        {
            double[,] sum = new double[a.rows,a.columns];
            if(!Same_Size(a,b))
            {
                Console.ForegroundColor  = ConsoleColor.Red;
                throw new Exception ("Las matrices no tienen el mismo tamano para realizar la suma");
                //Console.ForegroundColor = ConsoleColor.White;
               
            }
            else
            {
                for(int i=0; i<a.rows ;i++)
                {
                    for(int j=0; j<a.columns; j++)
                    {
                        sum[i,j] = a.matrix_coef[i,j] + b.matrix_coef[i,j];
                    }
                }
            }
            
            Matrix sum_matrix = new Matrix(sum);
            return sum_matrix;
        }

        private static bool Same_Size(Matrix a, Matrix b)
        {
            if((a.rows == b.rows) && (a.columns == b.columns))
            {
                return true;
            }
            return false;
        }

        public static Matrix Multiply_Matrix(Matrix a, Matrix b)
        {
            double[,] Product_Matrix = new double[a.rows,b.columns];
            if(!(a.columns == b.rows))
            {
               throw new Exception ("No se puede realizar la operacion");
            }
            else
            {
                for(int i=0; i<a.rows; i++)
                {
                    for(int j=0; j<b.columns; j++)
                    {
                        double product = 0;
                        for(int k=0; k<a.columns; k++)//a.columns = b.rows
                        {
                            product+= a.matrix_coef[i,k] * b.matrix_coef[k,j];
                        }
                        Product_Matrix[i,j] = product;
                    }
                }
                
            }
            Matrix M = new Matrix(Product_Matrix);
            return M;
        }

        public static double Determinant(Matrix matrix)
        {
            if(!(matrix.rows == matrix.columns))
            {
                throw new Exception ("La matriz no es cuadrada");
            }
            
            return Matrix_Det(matrix.matrix_coef);
            
        }
        public static double Matrix_Det(double[,]Matrix)
        {
            double determinant = 0;
            int rows = Matrix.GetLength(0);
            int columns = Matrix.GetLength(1);
            if((rows == 1) && (columns==1))
            {
                return Matrix[0,0];
            }

            for(int i=0; i<rows; i++)
            {
                if(i%2==0)
                {
                    determinant+= Matrix[i,0] * Matrix_Det(SubMatrix(Matrix,i,0));
                }
                else
                {
                    determinant += (-1)*Matrix[i,0] * Matrix_Det(SubMatrix(Matrix,i,0));
                }
                
            }
            return determinant;
            
        }
        public static double[,] SubMatrix(double[,]matrix,int i,int j) //(i,j) principio --- (k,h) final
        {
            double[,] SubMatrix = new double[matrix.GetLength(0)-1,matrix.GetLength(1)-1];
            int dup_i=0;
            int dup_j=0;
            for(int r =0; r<SubMatrix.GetLength(0);r++)
            {
                if(dup_i == i)
                {
                    dup_i++;
                }
                for(int c=0; c<SubMatrix.GetLength(1); c++)
                {
                    if(dup_j == j)
                    {
                        dup_j++;
                    }
                    SubMatrix[r,c] = matrix[dup_i,dup_j];
                    dup_j++;
                }
                dup_i++;
                dup_j=0;
                
            }
            return SubMatrix;
        }

        public static Matrix Traspuesta(Matrix matrix)
        {
            double[,] T = new double[matrix.columns,matrix.rows];
            for(int i=0; i<T.GetLength(0); i++ )
            {
                for(int j=0; j<T.GetLength(1); j++)
                {
                    T[i,j] = matrix.matrix_coef[j,i];
                }
            }
            Matrix Traspuesta = new Matrix(T);
            return Traspuesta;
        }

        private static Matrix Identidad (int n)
        {
            double[,] Identidad= new double[n,n];
            for(int i=0; i<n; i++)
            {
                Identidad[i,i] = 1;
            } 
            Matrix I = new Matrix(Identidad);
            return I;
        }

        public static Matrix Gauss(Matrix M)
        {
            double[,]matrix = M.matrix_coef;
            int rowCount = matrix.GetLength(0);
            int colCount = matrix.GetLength(1);
            int minCount = Math.Min(rowCount, colCount - 1);

            for (int i = 0; i < minCount; i++)
            {
                // Buscando maximo en la columna
                double maxEl = Math.Abs(matrix[i, i]);
                int maxRow = i;
                for (int k = i + 1; k < rowCount; k++)
                {
                    if (Math.Abs(matrix[k, i]) > maxEl)
                    {
                        maxEl = Math.Abs(matrix[k, i]);
                        maxRow = k;
                    }
                }

                // Cambiar fila con el maximo a la fila actual(columna por columna)
                for (int k = i; k < colCount; k++)
                {
                    double tmp = matrix[maxRow, k];
                    matrix[maxRow, k] = matrix[i, k];
                    matrix[i, k] = tmp;
                }

                // Hacer todas las filas por debajo de este 0 en la columna actual
                for (int k = i + 1; k < rowCount; k++)
                {
                    double c = -matrix[k, i] / matrix[i, i];
                    for (int j = i; j < colCount; j++)
                    {
                        if (i == j)
                        {
                            matrix[k, j] = 0;
                        }
                        else
                        {
                            matrix[k, j] += c * matrix[i, j];
                        }
                    }
                }
            }
            Matrix gauss = new Matrix(matrix);
            return gauss;
        }
            
        public static Matrix InverseMatrix(Matrix M)
        {
            double[,]matrix = M.matrix_coef;
            int n = matrix.GetLength(0);
            double[,] aumentedMatrix = new double[n, 2 * n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    aumentedMatrix[i, j] = matrix[i, j];
                }

                aumentedMatrix[i, i + n] = 1;
            }

            //  Gauss-Jordan 
            for (int i = 0; i < n; i++)
            {
                // Find pivot row
                int pivot = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (Math.Abs(aumentedMatrix[j, i]) > Math.Abs(aumentedMatrix[pivot, i]))
                    {
                        pivot = j;
                    }
                }

                if (pivot != i)
                {
                    for (int j = 0; j < 2 * n; j++)
                    {
                        double temp = aumentedMatrix[i, j];
                        aumentedMatrix[i, j] = aumentedMatrix[pivot, j];
                        aumentedMatrix[pivot, j] = temp;
                    }
                }

                
                double pivotElement = aumentedMatrix[i, i];
                if (pivotElement != 0)
                {
                    for (int j = 0; j < 2 * n; j++)
                    {
                        aumentedMatrix[i, j] /= pivotElement;
                    }
                }

                
                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                    {
                        double factor = aumentedMatrix[j, i];
                        for (int k = 0; k < 2 * n; k++)
                        {
                            aumentedMatrix[j, k] -= factor * aumentedMatrix[i, k];
                        }
                    }
                }
            }

           // Coger la matrtiz inversa
            double[,] inverseMatrix = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    inverseMatrix[i, j] = aumentedMatrix[i, j + n];
                }
            }

            Matrix gauss_jordan = new Matrix(inverseMatrix);
            return gauss_jordan;
        }


        
        public static void Print_Matrix(Matrix matrix)
        {
            for(int i=0; i<matrix.rows ;i++)
            {
                for(int j=0; j<matrix.columns; j++)
                {
                    Console.Write("{0} ",matrix.matrix_coef[i,j]);
                }
                Console.WriteLine(" ");
            }
            Console.WriteLine(" ");
        }

        public static void Print(double[,] matrix)
        {
            for(int i=0; i<matrix.GetLength(0) ;i++)
            {
                for(int j=0; j<matrix.GetLength(1); j++)
                {
                    Console.Write("{0} ",matrix[i,j]);
                }
                Console.WriteLine(" ");
            }
            Console.WriteLine(" ");
        }



    }
}