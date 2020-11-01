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
    // Last Modified: 10/31/2020
    //
    // **************************************************

    /// <summary>
    /// USER COMANDS
    /// </summary>
    public enum command
    {
        NONE,
        MOVEFORWARD,
        MOVEBACKWARD,
        STOPMOTORS,
        WAIT,
        TURNRIGHT,
        TIRNLEFT,
        LEDON,
        LEDOFF,
        SPIN,
        CHARGE,
        GETTEMPERATURE,
        DONE
    }
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
                        AlarmSystemDisplayMenuScreen(finchRobot);
                        break;

                    case "e":
                        UserProgramingDisplayMenuScreen(finchRobot);
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

        #region  USER PROGRAMING
        /// <summary>
        /// *****************************************************************
        /// *                    User Programing Menu                          *
        /// *****************************************************************
        /// </summary>
        static void UserProgramingDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            (int motorSpeed, int LedBrightness, double WaitSecond) ComandPerameters;
            ComandPerameters = (0, 0, 0);
            List<command> comands = new List<command>();


            do
            {
                DisplayScreenHeader("User Programing Menu");
                //
                // get user menu choice
                //

                Console.WriteLine("\ta) Set Command Perameters ");
                Console.WriteLine("\tb) Add Commands");
                Console.WriteLine("\tc) View Commands");
                Console.WriteLine("\td) Execuit Commands ");
                Console.WriteLine("\tq) Main Menu ");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        ComandPerameters = UserProgramingDisplayGetComandPerameters();
                        break;

                    case "b":
                        UserProgramingDisplayGetComands(comands);
                        break;

                    case "c":
                        UserProgramingDisplayCommands(comands);
                        break;

                    case "d":
                        UserProgramingDisplayExicuteCommands(comands, finchRobot, ComandPerameters);
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

        static void UserProgramingDisplayExicuteCommands(List<command> comands, Finch finchRobot, (int motorSpeed, int LedBrightness, double WaitSecond) comandPerameters)
        {
            DisplayScreenHeader("Exicuit Commands");
            Console.WriteLine("\tThe finch is redy to exicuit your commands");
            DisplayContinuePrompt();
            Console.WriteLine();

            foreach (command command in comands)
            {
                switch (command)
                {
                    case Project_FinchControl.command.NONE:
                        Console.WriteLine("\tInvalid Command");
                        break;

                    case Project_FinchControl.command.MOVEFORWARD:
                        finchRobot.setMotors(comandPerameters.motorSpeed, comandPerameters.motorSpeed);
                        break;

                    case Project_FinchControl.command.MOVEBACKWARD:
                        finchRobot.setMotors(-comandPerameters.motorSpeed, -comandPerameters.motorSpeed);
                        break;

                    case Project_FinchControl.command.STOPMOTORS:
                        finchRobot.setMotors(0, 0);
                        break;

                    case Project_FinchControl.command.WAIT:
                        int waitMilliSecond = (int)(comandPerameters.WaitSecond * 1000);

                        finchRobot.wait(waitMilliSecond);
                        break;

                    case Project_FinchControl.command.TURNRIGHT:
                        
                        finchRobot.setMotors(comandPerameters.motorSpeed, 0);
                    
                        break;

                    case Project_FinchControl.command.TIRNLEFT:

                        finchRobot.setMotors(0, comandPerameters.motorSpeed);

                        break;

                    case Project_FinchControl.command.LEDON:
                        finchRobot.setLED(comandPerameters.LedBrightness,comandPerameters.LedBrightness, comandPerameters.LedBrightness);
                        
                        break;

                    case Project_FinchControl.command.LEDOFF:
                        finchRobot.setLED(0, 0, 0);
                        break;

                    case Project_FinchControl.command.SPIN:
                        finchRobot.setMotors(comandPerameters.motorSpeed, -comandPerameters.motorSpeed);
                        
                        break;

                    case Project_FinchControl.command.CHARGE:
                        finchRobot.setLED(comandPerameters.LedBrightness, comandPerameters.LedBrightness, comandPerameters.LedBrightness);
                        
                        finchRobot.noteOn(587);
                        finchRobot.wait(1000);
                        finchRobot.noteOff();
                       
                        finchRobot.noteOn(784);
                        finchRobot.wait(2000);
                        finchRobot.noteOff();

                        finchRobot.setMotors(comandPerameters.motorSpeed, comandPerameters.motorSpeed);
                        finchRobot.wait(3000);
                        


                        finchRobot.setMotors(0, 0);
                        finchRobot.setLED(0, 0, 0);

                        break;

                    case Project_FinchControl.command.GETTEMPERATURE:
                        double temp;
                        temp = finchRobot.getTemperature();
                        Console.WriteLine($"\t Temperature Reading is {temp} C");
                        break;

                    case Project_FinchControl.command.DONE:
                        break;

                    default:
                        break;
                }

                Console.WriteLine(command);
            }



            DisplayMenuPrompt("User Programing");
        }

        static void UserProgramingDisplayCommands(List<command> comands)
        {
            DisplayScreenHeader("Commands");

            foreach (command command in comands)
            {
                Console.WriteLine("\t\t" + command);
            }


            DisplayMenuPrompt("User Programing");
        }

        static void UserProgramingDisplayGetComands(List<command> comands)
        {
            command command;
            bool ValidResponce;
            bool isDoneAddingCommands = false;
            string userResponce;

            DisplayScreenHeader("\tEnter Commands");

            foreach (command commandName in Enum.GetValues(typeof(command)))
            {
                if (commandName.ToString() != "NONE")
                {
                    Console.WriteLine("\t\t" + commandName);
                }

            }

            do
            {
                ValidResponce = true;

                Console.WriteLine($"\t Enter Command ");
                userResponce = Console.ReadLine().ToUpper();

                if (userResponce != "DONE")
                {
                    if (!Enum.TryParse(userResponce, out command))
                    {
                        Console.WriteLine();
                        Console.WriteLine("\tPlese Enter Valid Command");
                        DisplayContinuePrompt();
                        ValidResponce = false;
                    }
                    else
                    {
                        comands.Add(command);
                    }
                }
                else
                {
                    isDoneAddingCommands = true;
                }


            } while (!ValidResponce || !isDoneAddingCommands);

            DisplayMenuPrompt("User Programing");
        }

        static (int motorSpeed, int LedBrightness, double WaitSecond) UserProgramingDisplayGetComandPerameters()
        {
            (int motorSpeed, int LedBrightness, double WaitSecond) CommandPerameter;
            CommandPerameter = (0, 0, 0);
            DisplayScreenHeader("Comand Perameters");

            bool ValidResponce;
            do
            {
                ValidResponce = true;

                Console.Write("\tEnter Motor Speed:");
                int.TryParse(Console.ReadLine(), out CommandPerameter.motorSpeed);

                if ( CommandPerameter.motorSpeed < 0 || CommandPerameter.motorSpeed > 255)
                {
                    Console.WriteLine();
                    Console.WriteLine("please enter a number between 0 and 255");
                    Console.WriteLine();
                    Console.WriteLine("press any key to continue");
                    Console.ReadKey();

                    ValidResponce = false;
                }

            } while (!ValidResponce);
                        
            do
            {
                ValidResponce = true;

                Console.Write("\tEnter LED Brightness:");
                int.TryParse(Console.ReadLine(), out CommandPerameter.LedBrightness);

                if (CommandPerameter.LedBrightness < 0 || CommandPerameter.LedBrightness > 255)
                {
                    Console.WriteLine();
                    Console.WriteLine("please enter a number between 0 and 255");
                    Console.WriteLine();
                    Console.WriteLine("press any key to continue");
                    Console.ReadKey();

                    ValidResponce = false;
                }

            } while (!ValidResponce);

            do
            {
                ValidResponce = true;

                Console.Write("\tEnter Wait Time [seconds]:");
                double.TryParse(Console.ReadLine(), out CommandPerameter.WaitSecond);

                if ( CommandPerameter.WaitSecond < 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("please enter a integer");
                    Console.WriteLine();
                    Console.WriteLine("press any key to continue");
                    Console.ReadKey();

                    ValidResponce = false;
                }

            } while (!ValidResponce);



            DisplayMenuPrompt("User Programing:");
            return CommandPerameter;
        }
        #region ALARM SYSTEM

        /// <summary>
        /// *****************************************************************
        /// *                     Alarm System Menu                          *
        /// *****************************************************************
        /// </summary>
        static void AlarmSystemDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            string sencorToMonotor = "";
            string rangeType = "";
            int minmaxThreshholdValue = 0;
            int timeToMonitore = 0;


            do
            {
                DisplayScreenHeader("Alarm System Menu");
                //
                // get user menu choice
                //

                Console.WriteLine("\ta) Set sensors to monitore");
                Console.WriteLine("\tb) Set Range type");
                Console.WriteLine("\tc) Set min/max Threhold value ");
                Console.WriteLine("\td) Set time to monitore ");
                Console.WriteLine("\te) Set alarm ");
                Console.WriteLine("\tq) Main Menu ");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        sencorToMonotor = AlarmSystemSetSensorsToMonitore();
                        break;

                    case "b":
                        rangeType = AlarmSystemSetRangetype();
                        break;

                    case "c":
                        minmaxThreshholdValue = AlarmSystemSetThreshholdValue(finchRobot, rangeType);
                        break;

                    case "d":
                        timeToMonitore = AlarmSystemSetTimeToMonitore();
                        break;

                    case "e":
                        if (sencorToMonotor == "" || rangeType == "" || minmaxThreshholdValue == 0 || timeToMonitore == 0)
                        {
                            Console.WriteLine("\tPlease Enter All Required Values");
                            DisplayContinuePrompt();
                        }
                        else
                        {

                            AlarmSystemSetAlarm(finchRobot, sencorToMonotor, minmaxThreshholdValue, rangeType, timeToMonitore);
                        }


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

        static void AlarmSystemSetAlarm(Finch finchRobot, string sencorToMonotor, int minmaxThreshholdValue, string rangeType, int timeToMonitore)
        {
            DisplayScreenHeader(" \t Set Alarm:");
            Console.WriteLine($"\t Sensors to Monitore: {sencorToMonotor}");
            Console.WriteLine($"\t Range Type: {rangeType}");
            Console.WriteLine($"\t Min/Max Threshold Value: {minmaxThreshholdValue}");
            Console.WriteLine($"\t Time to Monitore {timeToMonitore}");

            Console.WriteLine(" \t Press any key to set Alarm");
            Console.CursorVisible = false;
            Console.ReadKey();

            bool ThresholdExedre = false;
            for (int second = 1; second <= timeToMonitore; second++)
            {
                Console.WriteLine($" \t Time: {second}");

                ThresholdExedre = AlarmSystemThresholdExeded(finchRobot, sencorToMonotor, minmaxThreshholdValue, rangeType);

                //
                // diplay mesage if threshold is exeded
                //
                if (ThresholdExedre)
                {
                    Console.WriteLine("\t Threshold Exeded");
                    break;
                }
                finchRobot.wait(1000);

            }
            DisplayMenuPrompt(" \t Alarm System");
        }




        static bool AlarmSystemThresholdExeded(Finch finchRobot, string sencorToMonotor, int minmaxThreshholdValue, string rangeType)
        {
            int CurentLeftLightSensorValue;
            int CurentRightLightSensorValue;

            //
            // get Curent light senser values
            //
            CurentRightLightSensorValue = finchRobot.getRightLightSensor();
            CurentLeftLightSensorValue = finchRobot.getLeftLightSensor();
            //
            // test curent light sensor values
            //
            bool ThresholdExedre = false;
            switch (sencorToMonotor)
            {
                case "left":
                    if (rangeType == "minimum")
                    {


                        ThresholdExedre = CurentLeftLightSensorValue < minmaxThreshholdValue;

                    }
                    else
                    {
                        ThresholdExedre = CurentLeftLightSensorValue > minmaxThreshholdValue;
                    }
                    break;

                case "right":
                    if (rangeType == "minimum")
                    {


                        ThresholdExedre = CurentRightLightSensorValue < minmaxThreshholdValue;

                    }
                    else
                    {
                        ThresholdExedre = CurentRightLightSensorValue > minmaxThreshholdValue;
                    }
                    break;


                case "both":
                    if (rangeType == "minimum")
                    {


                        ThresholdExedre = (CurentLeftLightSensorValue < minmaxThreshholdValue) || (CurentRightLightSensorValue < minmaxThreshholdValue);

                    }
                    else
                    {
                        ThresholdExedre = (CurentLeftLightSensorValue > minmaxThreshholdValue) || (CurentRightLightSensorValue > minmaxThreshholdValue);
                    }
                    break;

                default:
                    Console.WriteLine("\t unknown sensor type");
                    break;
            }
            return ThresholdExedre;
        }

        /// <summary>
        /// ************************
        /// get Time to monitor
        /// *********************
        /// </summary>
        /// <param name="finchRobot"></param>
        /// <param name="rangeType"></param>
        /// <returns>time to monitor</returns>
        static int AlarmSystemSetTimeToMonitore()
        {
            int timeToMonitore = 0;

            DisplayScreenHeader(" \tTime To Monitor:");
            Console.Write($"\tEnter The Time you Wish to Monitor");



            if (int.TryParse(Console.ReadLine(), out timeToMonitore) && timeToMonitore > 0)
            {

                Console.WriteLine();
                Console.WriteLine($"\t You have chosen {timeToMonitore} Time to Monitor");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("\tplease enter a real number");
                Console.WriteLine();
                DisplayContinuePrompt();
            }

            return timeToMonitore;
        }


        /// <summary>
        /// ************************
        /// get Threshold Value
        /// *********************
        /// </summary>
        /// <param name="finchRobot"></param>
        /// <param name="rangeType"></param>
        /// <returns>Threshold Value</returns>
        static int AlarmSystemSetThreshholdValue(Finch finchRobot, string rangeType)
        {
            int minmaxThreshholdValue = 0;
            DisplayScreenHeader("\tMin/Max Threshold Value");


            Console.WriteLine($"\t The Ambient Left Light Sensore Value:{finchRobot.getLeftLightSensor()}");
            Console.WriteLine($"\t The Ambient Right Light Sensore Value:{finchRobot.getRightLightSensor()}");

            Console.Write($"\tEnter The {rangeType} Threshold Value");
            minmaxThreshholdValue = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine($"\tThreshold Value: {minmaxThreshholdValue} ");

            DisplayContinuePrompt();


            return minmaxThreshholdValue;
        }

        /// <summary>
        /// **************************
        /// get range type from user
        /// ***************************
        /// </summary>
        /// <returns>range type</returns>
        static string AlarmSystemSetRangetype()
        {
            string rangeType = "";

            DisplayScreenHeader(" \tRange Type ");
            Console.Write("\tEnter Range [minimum, maximum,]: ");
            rangeType = Console.ReadLine();

            if (rangeType == "minimum" || rangeType == "maximum")
            {
                Console.WriteLine();
                Console.WriteLine($"\t You have chosen {rangeType} as your Range Type");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("It appears you entered an invalid Range type. Please reenter Range type.");
                DisplayContinuePrompt();
            }




            return rangeType;
        }

        /// <summary>
        /// *************************
        /// Get sensors to monitore fow user
        /// *************************
        /// </summary>
        /// <returns> sensors to monitor </returns>
        static string AlarmSystemSetSensorsToMonitore()
        {

            string sensorsToMonitore = "";

            DisplayScreenHeader(" \tSensors To Monitore ");
            Console.Write("\tEnter Sensers to Monitor [left, right, both]: ");
            sensorsToMonitore = Console.ReadLine();

            if (sensorsToMonitore == "left" || sensorsToMonitore == "right" || sensorsToMonitore == "both")
            {
                Console.WriteLine();
                Console.WriteLine($"\t You have chosen {sensorsToMonitore} as your sensor");


            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("It appears you entered an invalid sensor type. Please reenter sensor type.");
                DisplayContinuePrompt();
            }

            return sensorsToMonitore;
        }



        #endregion

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
            double[] temprturesC = null;

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
                        numberOfDataPonts = DataRecorderDisplayGetNumberOfDataPoints();
                        break;

                    case "b":
                        FrequencyOfDataPointSeconds = DisplayRecorderDisplayGetFrequencyOfDataPoints();
                        break;

                    case "c":
                        if (numberOfDataPonts == 0 || FrequencyOfDataPointSeconds == 0)
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
                finchRobot.setLED(0, 255, 0);
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

        #endregion
    }
}
