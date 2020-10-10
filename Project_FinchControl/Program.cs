using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using FinchAPI;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control - Menu Starter
    // Description: Finch Robot App
    // Application Type: Console
    // Author: Samuel, Hoekwater
    // Dated Created: 10/1/2020
    // Last Modified: 10/1/2020
    //
    // **************************************************

    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            SetTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Main Menu                                 *
        /// *****************************************************************
        /// </summary>
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        DisplayTalentShowMenuScreen(finchRobot);
                        break;

                    case "c":
                        DataRecorderDisplayMenuScreen(finchRobot);
                        break;

                    case "d":

                        break;

                    case "e":

                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        #region DATA RECORDER
        /// <summary>
        /// *****************************************************************
        /// *                     Data Recorder Menu                          *
        /// *****************************************************************
        /// </summary>
        static void DataRecorderDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            int numberOfDataPonts = 0;
            double FrequencyOfDataPointSeconds = 0;
            double[] temprturesC =null;
            
            do
            {
                DisplayScreenHeader("Data Recorder Menu");

                //
                // get user menu choice
                //

                Console.WriteLine("\ta) Get the Number of Data Points");
                Console.WriteLine("\tb) Get the Freqency of Data points");
                Console.WriteLine("\tc) Get the Data set  ");
                Console.WriteLine("\td) Display the Data set  ");
                Console.WriteLine("\te) Get Data in Fahrenheit");
                Console.WriteLine("\tq) Main Menu ");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        numberOfDataPonts =  DataRecorderDisplayGetNumberOfDataPoints();
                        break;

                    case "b":
                        FrequencyOfDataPointSeconds = DisplayRecorderDisplayGetFrequencyOfDataPoints();
                        break;

                    case "c":
                        if (numberOfDataPonts == 0 || FrequencyOfDataPointSeconds == 0 )
                        {
                            Console.WriteLine();
                            Console.WriteLine("\tPlease Enter the frequency and number of data points first");
                            DisplayContinuePrompt();
                        }
                        else
                        {
                            temprturesC = DataRecorderDisplayGetDataSet(numberOfDataPonts, FrequencyOfDataPointSeconds, finchRobot);
                        }
                        break;

                    case "d":
                        DataRecorderDisplayDataSet(temprturesC);
                        break;

                    case "e":
                        double TempInF;
                        double TempInC;
                        string userResponse;

                        DisplayScreenHeader("Convert Temperature Of Celcius To Fahrenheit");
                        Console.WriteLine();
                        DisplayContinuePrompt();



                        DisplayScreenHeader(" Temperature Converter");

                        //
                        // get temp
                        //
                        Console.Write("\tEnter Temperature :");
                        userResponse = Console.ReadLine();
                        double.TryParse(userResponse, out TempInC);
                        //
                        // echo back
                        //
                        Console.WriteLine();
                        Console.WriteLine(" You have chosen {0} for your Temperature.", userResponse);
                        DisplayContinuePrompt();
                        TempInF = TempInC + 32;

                        Console.WriteLine();
                        Console.WriteLine(" Your Temperature in Fahrenheit is {0}.", TempInF);
                        DisplayContinuePrompt();


                        break;


                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }
        /// <summary>
        /// ***********
        /// Display The Data Table
        /// ***********
        /// </summary>
        /// <param name="finchRobot"></param>
        static void DataRecorderDisplayDatatable(double[] temprturesC)
        {
            
            
            Console.WriteLine("" +
               "Reading #".PadLeft(15) +
               "Temperature".PadLeft(15) 
               

              );

            for (int index = 0; index < temprturesC.Length; index++)
            {
                Console.WriteLine("" +
              (index + 1).ToString().PadLeft(15) +
               temprturesC[index].ToString("n2").PadLeft(15)

               );
            }
        }
        /// <summary>
        /// ***********
        /// Display The Data Set
        /// ***********
        /// </summary>
        /// <param name="finchRobot"></param>
        static void DataRecorderDisplayDataSet(double[] temprturesC)
        {
            DisplayScreenHeader("Data Set");

            DataRecorderDisplayDatatable(temprturesC);

            DisplayContinuePrompt();
        }

        /// <summary>
        /// ***********
        /// Get The Data Points
        /// ***********
        /// </summary>
        /// <param name="finchRobot"></param>
        static double[] DataRecorderDisplayGetDataSet(int numberOfDataPonts, double frequencyOfDataPointSeconds, Finch finchRobot)
        {
            double[] temperatures = new double[numberOfDataPonts];

            DisplayScreenHeader("Get Data Set");

            Console.WriteLine($"\tNumber of data points {numberOfDataPonts}");
            Console.WriteLine($"\tFrequency of data points {frequencyOfDataPointSeconds}");
            Console.WriteLine();

            Console.WriteLine("\t The finch is redy to record temperature data: Press any key to continue");
            Console.ReadKey();


            double temp;
            int FrequencyOfDataPointsInMilSeconde;
            for (int index = 0; index < numberOfDataPonts; index++)
            {
                temp = finchRobot.getTemperature();
                Console.WriteLine($"\t Temperature Reading: {index + 1}: {temp}");
                temperatures[index] = temp;
                FrequencyOfDataPointsInMilSeconde = (int)(frequencyOfDataPointSeconds * 1000);
                finchRobot.wait(FrequencyOfDataPointsInMilSeconde);
            }

            DisplayContinuePrompt();

            return temperatures;
        }

        /// <summary>
        /// ***********
        /// Get Frequency Of Data Points
        /// ***********
        /// </summary>
        /// <param name="finchRobot"></param>
        static double DisplayRecorderDisplayGetFrequencyOfDataPoints()
        {
            double frequencyOfDataPionts;
            
            DisplayScreenHeader("Frequency  of Data Points");
            Console.Write("Enter Frequency of Data Points:");
            double.TryParse(Console.ReadLine(), out frequencyOfDataPionts);

            Console.WriteLine();
            Console.WriteLine($"Frequency of Data Points {frequencyOfDataPionts}");

            DisplayContinuePrompt();

            return frequencyOfDataPionts;
        }

        /// <summary>
        /// ***********
        /// Data Recorder > Get Number of data points
        /// ***********
        /// </summary>
        /// <param name="finchRobot"></param>
        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            int numberOfDataPoints;
            DisplayScreenHeader(" Number of Data Points");
            Console.Write("Enter Number of Data Points:");
            int.TryParse(Console.ReadLine(), out numberOfDataPoints);

            Console.WriteLine();
            Console.WriteLine($"Number of Data Points {numberOfDataPoints}");

            DisplayContinuePrompt();

            return numberOfDataPoints;
        }
        #endregion

        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void DisplayTalentShowMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Light and Sound and movement");
                Console.WriteLine("\tb) Dance with finch Robot");
                Console.WriteLine("\tc) Sing a song with robot ");
                Console.WriteLine("\td) Control LED lights ");
                Console.WriteLine("\te) Control Robot speed ");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayLightAndSoundAndMovement(finchRobot);
                        break;

                    case "b":
                        DisplayDanceWithFinch(finchRobot);
                        break;

                    case "c":
                        DisplaySongWithFinch(finchRobot);

                        break;

                    case "d":
                        DispayLightControleWithFinch(finchRobot);
                        break;

                    case "e":
                        DispaySpeedControleWithFinch(finchRobot);
                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }
        /// <summary>
        /// ***********
        /// Speed
        /// ***********
        /// </summary>
        /// <param name="finchRobot"></param>
         static void DispaySpeedControleWithFinch(Finch finchRobot)
        {  
            bool validResponse2;
            string userResponse;
            int speed;

            DisplayScreenHeader("Speed Control with Finch Robot");
            Console.WriteLine();
            DisplayContinuePrompt();

           
            //
            // get the speed
            //
            Console.WriteLine();
            validResponse2 = false;
            do
            {
                Console.WriteLine("\t Enter a speed 0-255 ");
                userResponse = Console.ReadLine();

                if (int.TryParse(userResponse, out speed) && speed > 0 && speed < 256)
                {

                    validResponse2 = true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("\tplease enter a real number");
                    Console.WriteLine();
                }
            } while (!validResponse2);

            Console.WriteLine();
            Console.WriteLine("\t you have chosen {0} for your speed", speed);
            DisplayContinuePrompt();

            finchRobot.setMotors(speed, speed);
            finchRobot.wait(5000);
            finchRobot.setMotors(0, 0);






        }

        /// <summary>
        /// Light Controls
        /// </summary>
        /// <param name="finchRobot"></param>
        static void DispayLightControleWithFinch(Finch finchRobot)
        {
            bool quitLightShowMenu = true;
             string colorChoice;
            do
            {
                DisplayScreenHeader("Light Control with Finch Robot");
                Console.WriteLine();
                DisplayContinuePrompt();

               

                DisplayScreenHeader("Choose A LED Light Color");

                //
                // get user light
                //
                Console.WriteLine("\tred");
                Console.WriteLine("\tgreen");
                Console.WriteLine("\tblue");
                Console.Write("\t\tEnter Choice:");
                colorChoice = Console.ReadLine().ToLower();
                //
                // echo back
                //
                Console.WriteLine();
                Console.WriteLine(" you have chosen {0} for your LED color light.", colorChoice);
                DisplayContinuePrompt();

                switch (colorChoice)
                {
                    case "red":
                        finchRobot.setLED(255, 0, 0);
                        finchRobot.wait(4000);
                        finchRobot.setLED(0, 0, 0);
                        break;

                    case "blue":
                        finchRobot.setLED(0, 0, 255);
                        finchRobot.wait(4000);
                        finchRobot.setLED(0, 0, 0);
                        break;

                    case "green":
                        finchRobot.setLED(0, 255, 0);
                        finchRobot.wait(4000);
                        finchRobot.setLED(0, 0, 0);
                        break;



                    default:
                        Console.WriteLine();
                        Console.WriteLine("\t Sorry this is not a option Please enter a color from the menu choice.");
                        DisplayContinuePrompt();
                        quitLightShowMenu = false;
                        break;



                }
            } while (!quitLightShowMenu);
           
        }
        /// <summary>
        /// **********
        ///   Song with bot
        /// *********
        /// </summary>
        /// <param name="finchRobot"></param>
        static void DisplaySongWithFinch(Finch finchRobot)
        {
            DisplayScreenHeader("Song With Finch Robot");
            Console.WriteLine();
            Console.WriteLine("\tThe Finch robot will now sing for you!");
            DisplayContinuePrompt();

            finchRobot.noteOn(587);
            finchRobot.wait(1000);
            finchRobot.noteOff();

            finchRobot.noteOn(784);
            finchRobot.wait(2000);
            finchRobot.noteOff();

            finchRobot.noteOn(988);
            finchRobot.wait(250);
            finchRobot.noteOff();

            finchRobot.noteOn(880);
            finchRobot.wait(250);
            finchRobot.noteOff();

            finchRobot.noteOn(784);
            finchRobot.wait(250);
            finchRobot.noteOff();

            finchRobot.noteOn(988);
            finchRobot.wait(2000);
            finchRobot.noteOff();

            finchRobot.noteOn(988);
            finchRobot.wait(250);
            finchRobot.noteOff();

            finchRobot.noteOn(880);
            finchRobot.wait(250);
            finchRobot.noteOff();

            finchRobot.noteOn(784);
            finchRobot.wait(2000);
            finchRobot.noteOff();

            finchRobot.noteOn(659);
            finchRobot.wait(1000);
            finchRobot.noteOff();

            finchRobot.noteOn(587);
            finchRobot.wait(2000);
            finchRobot.noteOff();

            finchRobot.noteOn(587);
            finchRobot.wait(1000);
            finchRobot.noteOff();

            finchRobot.noteOn(784);
            finchRobot.wait(2000);
            finchRobot.noteOff();

            finchRobot.noteOn(988);
            finchRobot.wait(250);
            finchRobot.noteOff();

            finchRobot.noteOn(880);
            finchRobot.wait(250);
            finchRobot.noteOff();

            finchRobot.noteOn(784);
            finchRobot.wait(250);
            finchRobot.noteOff();

            finchRobot.noteOn(988);
            finchRobot.wait(2000);
            finchRobot.noteOff();

            finchRobot.noteOn(880);
            finchRobot.wait(250);
            finchRobot.noteOff();

            finchRobot.noteOn(988);
            finchRobot.wait(250);
            finchRobot.noteOff();

            finchRobot.noteOn(1174);
            finchRobot.wait(2000);
            finchRobot.noteOff();





        }

        /// <summary>
        /// ******************
        ///        Dance
        /// *****************
        /// </summary>
        /// <param name="finchRobot"></param>
        static void DisplayDanceWithFinch(Finch finchRobot)
        {
            DisplayScreenHeader("Dance With Finch Robot");
            Console.WriteLine();
            Console.WriteLine("\tThe Finch robot will now Dance for you!");
            DisplayContinuePrompt();

            for (int i = 0; i < 13; i++)
            {
                finchRobot.setMotors(255, 128);
                finchRobot.wait(500);
                finchRobot.setMotors(128, 255);
                finchRobot.wait(500);
                finchRobot.setMotors(0, 0);
                finchRobot.setMotors(-128, 128);
                finchRobot.wait(500);
                finchRobot.setMotors(0, 0);


            }


            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Light and Sound  > Move                *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayLightAndSoundAndMovement(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Light and Sound");

            Console.WriteLine("\tThe Finch robot will now show off its glowing talent!");
            DisplayContinuePrompt();

            for (int lightSoundLevel = 0; lightSoundLevel < 255; lightSoundLevel++)
            {
                finchRobot.setMotors(-120, 120);
                finchRobot.setLED(lightSoundLevel, lightSoundLevel, lightSoundLevel);
                finchRobot.noteOn(lightSoundLevel * 100);
                finchRobot.setMotors(0, 0);
                finchRobot.noteOff();
                
            }

            DisplayMenuPrompt("Talent Show Menu");
        }

        #endregion

        #region FINCH ROBOT MANAGEMENT

        /// <summary>
        /// *****************************************************************
        /// *               Disconnect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tAbout to disconnect from the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine("\tThe Finch robot is now disconnect.");

            DisplayMenuPrompt("Main Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *                  Connect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();

            robotConnected = finchRobot.connect();

            if (robotConnected)
            {
                Console.WriteLine("\t Robot Conected.");
                finchRobot.setLED(0,255,0);
                finchRobot.noteOn(261);
                finchRobot.wait(1000);
                finchRobot.setLED(0, 0, 0);
                finchRobot.noteOff();
            }
            else
            {
                Console.WriteLine("\t There was a problem connecting plese try again");
            }

            DisplayMenuPrompt("Main Menu");

            //
            // reset finch robot
            //
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            return robotConnected;
        }

        #endregion

        #region USER INTERFACE

        /// <summary>
        /// *****************************************************************
        /// *                     Welcome Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Closing Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}
