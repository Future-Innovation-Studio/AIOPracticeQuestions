using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Data;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Dynamic;
using System.Linq.Expressions;


namespace AIOPracticeQuestions
{
    class Program
    {
        static void Main(string[] args)
        {
			String input = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments) + "/test.txt";
			String output = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments) + "/result.txt";
				
			SolutionToQuestionThreeIn2014 newSolution = new SolutionToQuestionThreeIn2014 (input);
			newSolution.generateSolutionText (output);
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

	#region external utility classes
	internal class PermuteUtils
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

	#region Practice 2014 Q1

	public class SolutionToQuestionOneIn2014{

		//variables
		private int daisyArrayCount;
		private int[] indexOfHipposArray = new int[3];

		public SolutionToQuestionOneIn2014(String inputFileDir){
			StreamReader reader = new StreamReader (inputFileDir);
		    //get number of daisies 
			int numOfDaisies = int.Parse (reader.ReadLine ());
			//set range of result array
			this.daisyArrayCount = numOfDaisies;
			//get the presence of hippos and write them to array
			int count = 0; String line;
			while ((line = reader.ReadLine ()) != null) {
				//get index of hippo
				int indexOfHippo = int.Parse (line);
				//set index in array to be 1 rather than 0 
				this.indexOfHipposArray.SetValue (indexOfHippo,count);
				count++;
			}
			//close reader
			reader.Close ();
		}

		public void generateTextForNumberOfDaisies(String outputDir){
			StreamWriter writer = new StreamWriter (outputDir);
			writer.WriteLine (this.findMaxNumToSave ());
			writer.Close ();
		}

		private int findMaxNumToSave(){
			int numberOfDaisiesToBeSaved = 0;
			int indexOfFirstHippo = indexOfHipposArray [0];
			int indexOfSecondHippo = indexOfHipposArray [1];
			int indexOfThirdHippo = indexOfHipposArray [2];

			int distanceBetweenFirstHippoAndStart = indexOfFirstHippo-1;
			int distanceBetweenFirstAndSecond = indexOfSecondHippo - indexOfFirstHippo - 1;
			int distanceBetweenSecondAndThird = indexOfThirdHippo - indexOfSecondHippo - 1;
			int distanceBetweenThridAndEnd = this.daisyArrayCount - indexOfThirdHippo;

			int[] distanceArray = new int[4]{distanceBetweenFirstHippoAndStart,distanceBetweenFirstAndSecond,distanceBetweenSecondAndThird,distanceBetweenThridAndEnd };
			Array.Sort<int>(distanceArray,
				new Comparison<int>(
					(i1, i2) => i2.CompareTo(i1)
				));
			numberOfDaisiesToBeSaved = distanceArray [0] + distanceArray [1];
			return numberOfDaisiesToBeSaved;
		}

	}

	#endregion

	#region Practice 2014 Q2
	public class SolutionToQuestionTwoIn2014{
		private List<Person> personList;
		private int numOfKnowing;
		private int numOfNotKnowing;

		public SolutionToQuestionTwoIn2014(String inputFileDir){
			StreamReader reader = new StreamReader (inputFileDir);
			//read the first line
			String[] firstlinetext = reader.ReadLine ().Split (' ');
			int[] firstline = Array.ConvertAll (firstlinetext, int.Parse);
			//set up the data
			this.personList = new List<Person>();
			int numOfPerson = firstline [0];
			for (int i = 1; i <= numOfPerson; i++) {
				Person newPerson = new Person(i);
				this.personList.Add (newPerson);
			}
			this.numOfKnowing = firstline [2];
			this.numOfNotKnowing = firstline [3];
			String line;
			while ((line = reader.ReadLine ()) != null){
				int[] eachline = Array.ConvertAll (line.Split (' '), int.Parse);
				Person selectedFirst = this.findPersonWithID (eachline [0]);
				selectedFirst.addFriend (this.findPersonWithID (eachline[1]));
				Person selectedSecond = this.findPersonWithID (eachline[1]);
				selectedSecond.addFriend (this.findPersonWithID (eachline[0]));
			}
			reader.Close ();

		}

		public void generateSolutionText(String outputDir){
		    this.solve ();
			StreamWriter writer = new StreamWriter (outputDir);
			writer.WriteLine (this.personList.ToList ().Count.ToString ());
			writer.Close ();
		}

		private void solve(){
			while (!this.validateA ()) {
				this.cleanAOptions ();
			}
			while (!this.validateB ()){
				this.cleanBOptions ();
			}

		}

		private void cleanAOptions(){

			foreach (Person person in this.personList.ToList ()) {
				if (person.getNumOfFriends () < this.numOfKnowing) {

					//remove all occurances of friendship
					foreach (Person innerPerson in this.personList.ToList ()){
						innerPerson.removeFriendWithID (person.id);
					}
					//remove itself
					this.personList.Remove (person);
				}
			}
		}

		private bool validateA(){
			foreach (Person person in this.personList.ToList ()) {
				if (person.getNumOfFriends () < this.numOfKnowing) {
					return false;
				}
			}
			return true;
		}

		private void cleanBOptions(){
			foreach (Person person in this.personList.ToList ()) {
				if (person.getNumOfFriends () > (this.personList.Count - this.numOfNotKnowing -1)) {
					//remove all occurances of friendship
					foreach (Person innerPerson in this.personList.ToList ()) {
						innerPerson.removeFriendWithID (person.id);
					}
					//remove itself
					this.personList.Remove (person);
				}

			}
		}

		private bool validateB(){
			foreach (Person person in this.personList.ToList ()) {
				if (person.getNumOfFriends () > (this.personList.Count - this.numOfNotKnowing-1)) {
					return false;
				}
			}
			return true;
		}

		private Person findPersonWithID(int id){
			foreach (Person person in this.personList.ToList ()) {
				if (person.id == id) {
					return person;
				}
			}
			return null;
		}

		//Internal class for each person
		internal class Person:ICloneable {

			public List<Person> friends{ get; set;}

			public int id { get; set;}

			public bool isNA { get; set; }

			public Person(int id){
				this.friends = new List<Person>(); 
				this.id = id;
			}
		
			public void addFriend(Person newFriend){
				this.friends.Add (newFriend);
			}

			public void removeFriendWithID(int id){
				foreach (Person person in this.friends.ToList ()) {
					if (person.id == id) {
						this.friends.Remove (person);
					}
				}
			}

			public int getNumOfFriends(){
				return this.friends.Count;
			}

		
				

			public Object Clone(){
				Person newPerson = new Person (this.id);
				newPerson.friends = this.friends;
				return newPerson;
			}
		}

	}
	#endregion

	#region Practice 2014 Q3

	public class SolutionToQuestionThreeIn2014{
		
		private int sequenceLength;
		private List<List<String>> seqCollections = new List<List<String>>();

		public SolutionToQuestionThreeIn2014(String input){
			StreamReader reader = new StreamReader (input);
			this.sequenceLength = int.Parse (reader.ReadLine ());
			String line; List<String> firstSeq = new List<String>(); List<String> secondSeq = new List<String>(); List<String> thirdSeq = new List<String>();
			int count = 0;
			while ((line = reader.ReadLine ()) != null) {
				switch (count) {
				case 0:
					{
						foreach (char c in line) {
							firstSeq.Add (c.ToString ());
						}
					}
					
					break;
				case 1:
					{
						foreach (char c in line) {
							secondSeq.Add (c.ToString ());
						}
					}
					break;
				case 2:
					{
						foreach (char c in line) {
							thirdSeq.Add (c.ToString ());
						}
					}
					break;
				default:
					break;
				}
				count++;
			}
			reader.Close ();
			for (int i = 0; i < this.sequenceLength; i++) {
				List<String> section = new List<String> (3);
				section.Add (firstSeq[i]);
				section.Add (secondSeq[i]);
				section.Add (thirdSeq[i]);
				this.seqCollections.Add (section);
			}
		}

		public void generateSolutionText(String output){
			StreamWriter writer = new StreamWriter(output);
			writer.WriteLine (this.solve ().ToString ());
			writer.Close ();
		}

		private int solve(){
			int sum = 0;
			foreach (List<String> section in seqCollections) {
				int max = section.ToLookup (x => x).Max (x => x.Count ());
				switch (max) {
				case 1:
					sum += 1;
					break;
				case 2:
					sum += 2;
					break;
				case 3:
					sum += 3;
					break;
				default:
					break;
				}
			}
			int average = sum / 3;
			return average;
		}

	}

	#endregion
}