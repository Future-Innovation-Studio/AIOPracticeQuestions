using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
namespace AIOPracticeQuestions
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            TestPart test = new TestPart(new int[]{12,15,17,19,31,52},100);
            test.generateHighestNumberPairFromArray();
            throw new IndexOutOfRangeException(@"crap!");*/
            SolutionToQuestionTwo solution = new SolutionToQuestionTwo(@"C:/test.txt");
            solution.generateSolutionText(@"C:/Users/chung/Documents/result.txt");
           
        }
    }

#region Question one
    public class SolutionToQuestionOne
    {
        private int inputNumber;
        private int count;
        
        public SolutionToQuestionOne(String inputFileDir)
        {
            //Read file and initialize variables
            StreamReader reader = new StreamReader(inputFileDir);
            //Assuming single line
            String line = reader.ReadLine();
            this.inputNumber = int.Parse(line);
            reader.Close();
        }

        public void generateSolutionText(String outputFileDir){
            //Get result
            int[] result = this.solve();
            //Write to file
            StreamWriter writer = new StreamWriter(outputFileDir);
            writer.WriteLine(result[0]);
            writer.WriteLine(result[1]);
            writer.Close();
        }

        private int[] solve()
        {
            while (this.inputNumber%2 == 0)
            {
                this.inputNumber = this.inputNumber / 2;
                count++;
            }

            int[] resultArray = { this.inputNumber, count };
            return resultArray;
        }

    }
#endregion
#region Question Two
    public class SolutionToQuestionTwo{
        private int baseNum;
        private int bottomNumCount;
        private int[] selectionArray;

        private int[][] maxPair;

        public SolutionToQuestionTwo(String inputFileDir)
        {
            //Read the file
            StreamReader reader = new StreamReader(inputFileDir);
            //prepare variables
            for (int i = 0; i < 3; i++)
            {
                switch (i){
                    case 0:
                        this.baseNum = int.Parse(reader.ReadLine());
                    break;
                    case 1:
                        this.bottomNumCount = int.Parse(reader.ReadLine());
                    break;
                    case 2:
                        String[] strArr = reader.ReadLine().Split(' ');
                        this.selectionArray = Array.ConvertAll(strArr,int.Parse);
                    break;
                    default:
                    break;
                }
            }
            reader.Close();
            //initialize max pair
            this.maxPair = new int[][]{new int[]{0}, new int[]{0}};
        }

        public void generateSolutionText(String outputFileDir)
        {
            //Solve
            this.solve();
            //Generate text
            StreamWriter writer = new StreamWriter(outputFileDir);
            writer.WriteLine(this.maxPair[0][0].ToString());
            writer.WriteLine(String.Join(" ",this.maxPair[1]));
            writer.Close();
        }

        private void solve()
        {
            //Get all the combinations and get max pair sorted out
            this.combinations(this.selectionArray, 6, 0, new int[6]);   
        }


        private void combinations(int[] array, int len, int startPos, int[] result)
        {
            if (len == 0)
            {
                foreach (IEnumerable<int> permutation in PermuteUtils.Permute<int>(result, 6))
                {
                    int[] generatedArray = new int[6];
                    int count = 0;
                    foreach (int i in permutation)
                    {
                        generatedArray.SetValue(i, count);
                        count++;
                    }
                    //Find the top value of the result array
                    int[][] newPair = this.generateHighestNumberPairFromArray(generatedArray);

                    if (newPair[0][0] > this.maxPair[0][0])
                    {
                        this.maxPair = newPair;
                    }
                    
                }
                
                return;
            }

            for (int i = startPos; i <= array.Length - len; i++)
            {
                result[result.Length - len] = array[i];
                this.combinations(array, len - 1, i + 1, result);

            }
            
        }

        
        private int[][] generateHighestNumberPairFromArray(int[] bottomArray)
        {
            //here's the result
            int result;
            //prepare the slots
            int rows = 6;
            int[][] outcome = new int[rows][];
            int rowCount = 6;
            for (int i = 0; i < outcome.Length; i++)
            {
                outcome[i] = new int[rowCount];
                rowCount--;
            }
            //fill in the base data
            outcome[0] = bottomArray;
            //climb up till the second top row
            for (int row = 1; row < outcome.Length; row++)
            {
                for (int col = 0; col < outcome[row].Length; col++)
                {
                    //check if its the top one
                    if (row == outcome.Length - 1)
                    {
                        result = outcome[row - 1][col] * outcome[row - 1][col + 1];
                        //return the result
                        return new int[][] {new int[]{result},bottomArray };
                    }
                    else
                    {
                        int temp = outcome[row - 1][col] + outcome[row - 1][col + 1];
                        if (temp >= this.baseNum)
                        {
                            temp -= this.baseNum;
                        }
                        outcome[row][col] = temp;
                    }

                }
            }

            //in case error exists
            return null;
        }

    }
#endregion

#region test case
    public class TestPart
    {
        private int[] bottomArray;
        private int[][] maxPair;
        private int baseNum;

        public TestPart(int[] bottomArr, int baseNum)
        {
            this.baseNum = baseNum;
            this.bottomArray = bottomArr;
            //initialize max pair
            this.maxPair = new int[][] { new int[] { 0 }, new int[] { 0 } };
        }

        public void generateHighestNumberPairFromArray()
        {
            //here's the result
            int result;
            //prepare the slots
            int rows = 6;
            int[][] outcome = new int[rows][];
            int rowCount = 6;
            for (int i = 0; i < outcome.Length; i++)
            {
                outcome[i] = new int[rowCount];
                rowCount--;
            }
            //fill in the base data
            outcome[0] = this.bottomArray;
            Console.WriteLine(String.Join(" ", outcome[0]));

            //climb up till the second top row
            for (int row = 1; row < outcome.Length; row++)
            {
                for (int col = 0; col < outcome[row].Length; col++)
                {
                    //check if its the top one
                    if (row == outcome.Length - 1)
                    {
                        result = outcome[row - 1][col] * outcome[row - 1][col + 1];
                        //return the result
                        this.maxPair = new int[][] { new int[] { result }, bottomArray };
                    }
                    else
                    {
                        int temp = outcome[row - 1][col] + outcome[row - 1][col + 1];
                        if (temp >= this.baseNum)
                        {
                            temp -= this.baseNum;
                        }
                        outcome[row][col] = temp;
                    }
                }
                Console.WriteLine(String.Join(" ", outcome[row]));

            }
        }

       
    }
#endregion
#region PermuUtility
    public class PermuteUtils
    {
        // Returns an enumeration of enumerators, one for each permutation
        // of the input.
        public static IEnumerable<IEnumerable<T>> Permute<T>(IEnumerable<T> list, int count)
        {
            if (count == 0)
            {
                yield return new T[0];
            }
            else
            {
                int startingElementIndex = 0;
                foreach (T startingElement in list)
                {
                    IEnumerable<T> remainingItems = AllExcept(list, startingElementIndex);

                    foreach (IEnumerable<T> permutationOfRemainder in Permute(remainingItems, count - 1))
                    {
                        yield return Concat<T>(
                            new T[] { startingElement },
                            permutationOfRemainder);
                    }
                    startingElementIndex += 1;
                }
            }
        }

        // Enumerates over contents of both lists.
        public static IEnumerable<T> Concat<T>(IEnumerable<T> a, IEnumerable<T> b)
        {
            foreach (T item in a) { yield return item; }
            foreach (T item in b) { yield return item; }
        }

        // Enumerates over all items in the input, skipping over the item
        // with the specified offset.
        public static IEnumerable<T> AllExcept<T>(IEnumerable<T> input, int indexToSkip)
        {
            int index = 0;
            foreach (T item in input)
            {
                if (index != indexToSkip) yield return item;
                index += 1;
            }
        }
    }
#endregion
}