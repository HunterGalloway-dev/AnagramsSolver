using System;
using System.Collections.Generic;
using System.IO;

namespace AnagramSolver
{
    /// <summary>
    ///     Class designed to read a text file dictionary where each line is a singular
    ///     word. One an Object of type AnagramSolver has been instaniated generating all possible
    ///     words based of an input list of words using the method genwords.
    /// </summary>
    class AnagramSolver 
    {
        /// <summary>
        ///     Word map used to store sorted words to their list of corresponding words
        /// </summary>
        private Dictionary<string, List<String>> wordMap;
        /// <summary>
        /// Min word length to be added to the wordMap
        /// </summary>
        private int MinWordLength;
        /// <summary>
        /// Max word length to be added to the wordMap
        /// </summary>
        private int MaxWordLength;
        
        /// <summary>
        ///     Constructor of class used to generate and fill wordMap with the coresponding
        ///     sorted word key -> list of words 
        /// </summary>
        /// <param name="fileName"></param>
        ///     Minimum length of words that can be added to the wordMap
        /// <param name="minWordLength"></param>
        ///     Maximum lenght of words that can be added to the wordMap
        /// <param name="maxWordLength"></param>
        public AnagramSolver(string fileName, int minWordLength, int maxWordLength)
        {
            wordMap = new Dictionary<string, List<string>>();
            MinWordLength = minWordLength;
            MaxWordLength = maxWordLength;

            string line;

            StreamReader file = new StreamReader(fileName);

            while((line = file.ReadLine()) != null)
            {
                if(line.Length >= MinWordLength && line.Length <= MaxWordLength)
                {
                    line = line.Trim().ToLower();
                    string wordSorted = SortString(line);

                    if(wordMap.ContainsKey(wordSorted))
                    {
                        wordMap[wordSorted].Add(line);
                    } else
                    {
                        wordMap.Add(wordSorted, new List<string>() {line});
                    }
                }
            }
        }

        /// <summary>
        ///     Sorts the characters of a string inorder
        /// </summary>
        /// <param name="word">
        ///     word to be sorted
        /// </param>
        /// <returns> sorted word inorder </returns>
        public static string SortString(string word)
        {
            char[] charArr = word.ToCharArray();
            Array.Sort(charArr);

            return new String(charArr);
        }

        /// <summary>
        ///     Returns all possible words inside of the current dictionary based off the input
        /// </summary>
        /// <param name="input">letters given during anagrams</param>
        public List<string> GenWords(List<string> input)
        {
            List<string> possKeys = GenCombos(input,MinWordLength,MaxWordLength);
            List<string> words = new List<string>();

            foreach (string possKey in possKeys)
            {
                if(wordMap.ContainsKey(possKey))
                {
                    words.AddRange(wordMap[possKey]);
                }
            }

            return words;
        }

        /// <summary>
        ///  Generates all possible inorder combinations based of off input with the 
        ///  length of the words being in the range [MinWordLength, MaxWordLength]
        /// </summary>
        /// <param name="input">list coresponding to the letters given during anagrams</param>
        /// <returns> returns inorder combinations of input </returns>
        public static List<String> GenCombos(List<string> inputs, int min, int max)
        {
            List<string> combos = new List<string>();

            for(int length = max; length >= min; length--)
            {
                GenCombo(inputs, length, -1, length, "", combos);
            }

            return combos;
        }
        
        /// <summary>
        /// Generates all possible inorder combinations based off the input for a set length
        /// </summary>
        /// <param name="inputs">letters given from anagram</param>
        /// <param name="length">set length of all combination</param>
        /// <param name="curIndex">used in the algorithm to generate the trees</param>
        /// <param name="depth">use in algorithm to determine what combinations have not been used</param>
        /// <param name="curCombo">current combo</param>
        /// <param name="combos"> a list used to store all possible combinations when this reaches the end of the recursion</param>
        public static void GenCombo(List<string> inputs, int length, int curIndex, int depth, string curCombo, List<string> combos) {
            // Determines if the current combination iteration is at the desired length
            if(curCombo.Length < length)
            {
                // Moves to next input letter
                curIndex++;
                
                // Loops through each un touched letter utilizing depth to determine if we need to continue and runs recursion
                // to explore the combination tree
                while(curIndex < inputs.Count && depth > 0)
                {
                    GenCombo(inputs, length, curIndex, depth, curCombo+inputs[curIndex],combos);

                    curIndex++;
                    depth--;
                }

            } else
            {
                // Current combination is at the desired length so we add it to combos
                combos.Add(curCombo);
            }
        }
        
        /// <summary>
        ///     Main method used to access anagrams solver via command line
        /// </summary>
        /// <param name="args">
        ///     Do not matter as I do not use them
        /// </param>
        static void Main(string[] args)
        {
            Console.Write("Enter minimum length of words to be generated: ");
            int minWordLength = Int32.Parse(Console.ReadLine().Trim());

            Console.Write("Enter maximum length of words to be generated: ");
            int maxWordLength = Int32.Parse(Console.ReadLine().Trim());
            
            Console.WriteLine("Generating dictionary of words...");
            AnagramSolver anagramSolver = new AnagramSolver("C:/Users/14403/Desktop/C#/AnagramSolver/words_alpha.txt",minWordLength,maxWordLength);
            
            Console.Write("Done generating dictionary \n Enter input for Anagrams as a single string: ");
            string inputStr = Console.ReadLine();
            
            inputStr = SortString(inputStr);

            List<string> input = new List<string>(new string[inputStr.Length]);
            
            for(int i = 0; i < inputStr.Length; i++)
            {
                input[i] = "" + inputStr[i];
            }

            List<string> words = anagramSolver.GenWords(input);

            foreach (string word in words)
            {
                Console.WriteLine(word);
            }
        }
    }
}
