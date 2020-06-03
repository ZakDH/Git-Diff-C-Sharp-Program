using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Assignment
{
    class Program
    {
        static void Main(string[] args) // user inputs "diff " + two txt files and the application finds which file is larger than the other
        {
            Console.WriteLine("Input 2 files in the following format: diff file1 file2, with .txt at the end.\n");
            Console.Write(">: [Input] "); // implemented to make the application look more like the git diff command
            string userinput = Console.ReadLine();
            string[] Split = userinput.Split(" "); // splits the string by spaces 
            while (Split[0] != "diff" || Split.Length != 3) //will run the code in this loop if the first word doesn't equal "diff" and if the length is not equal to 3
            {
                Console.WriteLine("\nYou didn't enter the files in the correct format. Please try again!\n");
                Console.Write(">: [Input] ");
                string userinput_loop = Console.ReadLine();
                Split = userinput_loop.Split(" ");
            }
            FileDifference(Split); // calls the "FileDifference" method
        }
        static void FileDifference(string[] Split)
        {
            string File1 = File.ReadAllText(Split[1]);
            string File2 = File.ReadAllText(Split[2]);
            try // try, catch exception handling to maintain application security
            {                
                if (File1 == File2)
                {
                    Console.WriteLine("\n>: [Output] {0} and {1} have no difference.\n", Split[1], Split[2]); // if files are equal then this is output to the user
                    StringBuilder sb = new StringBuilder(); // instantiates a new object from StringBuilder function
                    sb.Append("Log File ");
                    string timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff"); // assigns the date/time at the time to timeStamp variable
                    sb.Append("\n"); // appends a new line to the file
                    sb.Append(timeStamp); // appends the date/time to the file
                    sb.Append("\n");
                    sb.Append("No Differences in file.\n");
                    sb.Append("\n");
                    File.AppendAllText("logfile.txt", sb.ToString()); // appends all text and names the file "logfile" to a string
                }
                else if (File1 != File2)
                {
                    Console.WriteLine("\n>: [Output] {0} and {1} are different.\n", Split[1], Split[2]); // if files aren't equal in length then "FileAnalyse" method is called
                    FileAnalyse(File1, File2);

                }
            }
            catch (FileNotFoundException) // if the file cannot be found then the error is 'caught'
            {
                Console.WriteLine("There is an error in the input (File may not exist)\n");
                Environment.Exit(1); // exits the application 
            }
        }
        static void FileAnalyse(string File1, string File2)
        {            
            string[] File1_Split = File1.Split(" ");
            string[] File2_Split = File2.Split(" ");
            try
            {
                if (File1_Split.Length < File2_Split.Length) // if file 1 length is less than file 2 length
                {
                    List<string> Data = FileComparison(File1_Split, File2_Split); // "FileComparison" method is called and a string list is returned
                    string[] File2Split = File2.Split("\n");
                    foreach (string item in Data) // takes each value in the data list
                    {                     
                        int LineNum = LineSearch(File2Split, item);
                        int LineNumber = LineNum + 1;
                        string lineContent = GetLine(File2, LineNum);
                        Console.WriteLine(">: [Output] Line: {0}", LineNumber); // outputs the line number to resemble the git diff command                 
                        Console.Write(">: [Output] + ");
                        ColourOutput(lineContent, item, "Green"); // if file 2 is larger than file 1 then there is extra data, this data is coloured in green

                    }
                }
                if (File1_Split.Length > File2_Split.Length) // if file 1 length is greater than file 2 length
                {
                    List<string> Data = FileComparison(File2_Split, File1_Split);
                    string[] File1Split = File1.Split("\n");
                    foreach (string item in Data)
                    {
                        /*foreach content is the same as the previous string apart from the colour red is assigned to the "ColourOutput" method rather than green*/
                        int LineNum = LineSearch(File1Split, item);
                        int LineNumber = LineNum + 1;
                        string lineContent = GetLine(File1, LineNum);
                        Console.WriteLine(">: [Output] Line: {0}", LineNumber);
                        Console.Write(">: [Output] - "); // if there is missing data then a "-" is output. If data is added then a "+" is output.
                        ColourOutput(lineContent, item, "Red");
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("There seems to be a problem with the program!");
                Environment.Exit(1);
            }
        }

        static void ColourOutput(string Content, string item, string Colour)
        {
            OutputFile file1 = new OutputFile // instantiates an object from the class "OutputFile"
            {
                Content = Content, // assigns file content to the class variable content
                item = item,
                Colour = Colour
            };
            StringBuilder sb = new StringBuilder();
            sb.Append("Log File ");
            string timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff");
            sb.Append(timeStamp);
            sb.Append("\n");
            string[] result = file1.Content.Split(file1.item); // file is split from the item 
            string result2 = result[0]; // assigns the first part to result2
            string result3 = result[1];
            Console.Write(result2);
            if (file1.Colour == "Red") // if the colour referred is red 
            {
                Console.ForegroundColor = ConsoleColor.Red; // console foreground is changed to red then the item in the data list is output to the console
                Console.Write(file1.item);
                Console.ForegroundColor = ConsoleColor.White; // console foreground colour is changed back to white
                Console.Write(result3);
                Console.WriteLine("\n");
                sb.Append(result2);
                sb.Append(file1.item);
                sb.Append(result3);
                sb.Append("\n");
                sb.Append("\n");
                File.AppendAllText("logfile.txt", sb.ToString());
            }
            if (file1.Colour == "Green")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(file1.item);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(result3);
                Console.WriteLine("\n");
                sb.Append(result2);
                sb.Append(file1.item);
                sb.Append(result3);
                sb.Append("\n");
                sb.Append("\n");
                File.AppendAllText("logfile.txt", sb.ToString());
            }
        }    

        static int LineSearch(string[] FileSplit, string Data) // method returns an int data type
        {           
            int i;
            for(i = 0; i<FileSplit.Count(); i++) // if i is less than the file count then i is incremented
            {
                if (FileSplit[i].Contains(Data)) // if i is contained in the data list then statement is broken
                {
                    break;
                }
            }           
            return i++; // returns the line number 
        }
        static string GetLine(string FileName, int linenum) // gets the specific line number that the content is contained in
        {
            var lines = FileName.Split('\n');
            return lines[linenum]; // returns the the content in the specified line number
        }
        static List<string> FileComparison(string[] File1, string[] File2)
        {
            List<string> Data = new List<string>(); // creates a new string list called Data           
            for (int i = 0; i < File1.Length; i++) // creates an int variable - i, if i is less than file 1 length i is incremented
            {
                if (!File1.Contains(File2[i])) // if file 2 value isnt contained in file 1
                {
                    Data.Add(File2[i]); // file value is added to Data list
                }              
            } return Data; // data list is returned                                                                                
        }             
    }
    public class OutputFile // makes a new public class called "OutputFile"
    {
        public string Content; // makes a new public string called Content which is instantiated to use later on
        public string item;
        public string Colour;       
    }
}
