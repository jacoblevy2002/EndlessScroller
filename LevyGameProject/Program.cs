using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndlessScroller
{
    class Program
    {
        static Boolean quitApp;
        static void Main(string[] args)
        {
            Console.Title = "Levy Game Project";
            Boolean runOnce = false;
            quitApp = false;
            while (!quitApp)
            {
                Console.BufferHeight = Console.WindowHeight;
                Console.BufferWidth = Console.WindowWidth;
                Console.CursorVisible = false;
                if (runOnce)
                {
                    StartGame();
                } else
                {
                    runOnce = !runOnce;
                }
                OpenStartMenu(0);
            }
        }
        static ConsoleKey inputFunc() // input for a ConsoleKey var
        {
            return Console.ReadKey(true).Key;
        }
        static void menuSystem(Int32 menuID, ConsoleKey keyInput, ref Int32 currentValue, String[] menu, ref Boolean closeCurrentMenu, Int32 valueMax = Int32.MaxValue, Int32 valueMin = Int32.MinValue)
        // navigates a menu system. arguments are: ID of the menu (for use when pressing ENTER), user input (UP | DOWN | ENTER | ESCAPE), option thats currently selected, the menu (array, each option is a new value)
        // maximum and minimum values that can be selected (if not every value in the menu array can be selected)
        {
            if (menuID == 1) { GenerateControlMenu(out menu); }
            switch (keyInput)
            {
                case ConsoleKey.DownArrow:
                    if (currentValue < menu.Length - 1 && currentValue < valueMax)
                    {
                        currentValue++;
                        PrintCurrentMenu(menu, currentValue);
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (currentValue > 0 && currentValue > valueMin)
                    {
                        currentValue--;
                        PrintCurrentMenu(menu, currentValue);
                    }
                    break;
                case ConsoleKey.Enter:
                    EnterOnMenu(currentValue, menuID, ref closeCurrentMenu);
                    Console.Clear();
                    if (menuID == 1) { GenerateControlMenu(out menu); }
                    PrintCurrentMenu(menu, currentValue);
                    break;
                case ConsoleKey.Escape:
                    EscapeOnMenu(menuID, currentValue, ref closeCurrentMenu);
                    break;
                default:
                    break;
            }
        }
        static void GenerateControlMenu(out String[] menu) // generates the control menu (so that the properties.settings.. are up-to-date)
        {
            menu = new string[] { "Current Controls", $"Up: {Properties.Settings.Default.upInput}   ", $"Down: {Properties.Settings.Default.downInput}   ", $"Increase Speed: {Properties.Settings.Default.speedUp}   ", $"Decrease Speed: {Properties.Settings.Default.speedDown}   ", "Reset Controls to Default   ", "\nPress enter while a control is selected to modify it\nPress ESC to return to the Start Menu" };
        }
        static void EscapeOnMenu(Int32 menuID, Int32 currentValue, ref Boolean closeCurrentMenu) // if a user presses ESCAPE while on a menu. sorts through menuID and currentValue to figure out what action to take
        {
            switch (menuID)
            {
                case 0:
                    break;
                case 1:
                    closeCurrentMenu = true;
                    break;
            }
        }
        static void EnterOnMenu(Int32 currentValue, Int32 menuID, ref Boolean closeCurrentMenu) // if a user presses ENTER while on a menu. sorts through menuID and currentValue to figure out what was selected
        {
            switch (menuID)
            {
                case 0: // Start Menu
                    switch (currentValue)
                    {
                        case 0: // start
                            closeCurrentMenu = true;
                            break;
                        case 1: // controls
                            OpenControlMenu();
                            break;
                        case 2: // faq
                            OpenQuestionMenu();
                            break;
                        case 3: // highscore reset
                            Properties.Settings.Default.highscore = 0;
                            Properties.Settings.Default.Save();
                            OpenStartMenu(3);
                            break;
                        case 4: // quit
                            closeCurrentMenu = true;
                            quitApp = true;
                            break;
                        default:
                            break;
                    }
                    break;
                case 1: // Controls Menu
                    switch (currentValue)
                    {
                        case 1:
                            Properties.Settings.Default.upInput = ControlChange("UP");
                            break;
                        case 2:
                            Properties.Settings.Default.downInput = ControlChange("DOWN");
                            break;
                        case 3:
                            Properties.Settings.Default.speedUp = ControlChange("SPEED UP");
                            break;
                        case 4:
                            Properties.Settings.Default.speedDown = ControlChange("SPEED DOWN");
                            break;
                        case 5:
                            Properties.Settings.Default.upInput = "W";
                            Properties.Settings.Default.downInput = "S";
                            Properties.Settings.Default.speedDown = "LeftArrow";
                            Properties.Settings.Default.speedUp = "RightArrow";
                            break;
                        default:
                            break;
                    }
                    Properties.Settings.Default.Save();
                    break;
            }
        }
        static String ControlChange(String controlAction) // to change the value for a control. triggered when ENTER is pressed on any value in the Control Menu
        {
            Console.Clear();
            Console.WriteLine($"Press a new value for the \"{controlAction}\" action, or press ESC to return to the Controls Menu.");
            Boolean inputValid = false;
            String input = String.Empty;
            while (!inputValid)
            {
                input = Convert.ToString(inputFunc());
                if (input == Properties.Settings.Default.upInput || input == Properties.Settings.Default.downInput || input == Properties.Settings.Default.speedUp || input == Properties.Settings.Default.speedDown)
                {
                    Console.WriteLine("That button interferes with an existing setting. Please decide a new value:");
                }
                else if (input == "Escape")
                {
                    inputValid = true;
                    input = Properties.Settings.Default.upInput;
                }
                else
                {
                    inputValid = true;
                }
            }
            return input;
        }
        static void OpenStartMenu(Int32 menuValue = 0) // Opens the Start Menu. menuValue can be set to any value for the user to start with a specified option selected, or left empty for default
        {
            Boolean closeStartMenu = false;
            ConsoleKey keyInput = ConsoleKey.NoName;
            const Int32 startMenuID = 0;
            String[] startMenu = new string[] { "Start Game   ", "Controls   ", "What's the point of the game?   ", "Reset high score   ", "Quit   ", $"\nCurrent High Score: {Properties.Settings.Default.highscore}\n\nUse the Arrow keys to navigate menus, and press Enter to select an option" };
            Console.Clear();
            PrintCurrentMenu(startMenu, menuValue);
            while (!closeStartMenu)
            {
                Console.CursorVisible = false;
                keyInput = inputFunc();
                menuSystem(startMenuID, keyInput, ref menuValue, startMenu, ref closeStartMenu, startMenu.Length - 2);
            }
        }
        static void ColourWrite(String words, ConsoleColor color) // writes "words" in whatever colour is specified - mostly just cleans up code wherever colour is used
        {
            ConsoleColor precolor = Console.BackgroundColor;
            Console.BackgroundColor = color;
            Console.Write(words);
            Console.BackgroundColor = precolor;
        }
        static void OpenQuestionMenu() // opens the "what is this game?" menu. kind of a mess but i wanted the colours to line up with in-game
        {
            Console.Clear();
            String[] questionMenu = new string[] { "The goal of the game is to get as far as possible without touching the ", "\nYou start with 3 Lives. Touching a ", " will heal 1 Life. 1 ", $" will appear on every 3rd level.\nYou control the > symbol, which moves continuously to the left.\n{Properties.Settings.Default.speedUp} and {Properties.Settings.Default.speedDown} will increase and decrease the speed of your character,\nwhile {Properties.Settings.Default.upInput} and {Properties.Settings.Default.downInput} will allow you to move up and down.\nYour number of Lives, the Screen you're currently on, and your current Speed are all listed at the bottom of the screen.\nThe game will attempt to spawn 20 ", " on screen per level, no matter the window size - so the bigger the window size, the easier the game!\n[Please only change the window size while on a menu]\n\nPress Enter to return to the Main Menu." };
            Console.Write(questionMenu[0]);
            ColourWrite(" ", ConsoleColor.DarkRed);
            Console.Write(questionMenu[1]);
            ColourWrite(" ", ConsoleColor.Green);
            Console.Write(questionMenu[2]);
            ColourWrite(" ", ConsoleColor.Green);
            Console.Write(questionMenu[3]);
            ColourWrite(" ", ConsoleColor.DarkRed);
            Console.Write(questionMenu[4]);
            ConsoleKey keyInput = ConsoleKey.NoName;
            while (keyInput != ConsoleKey.Enter)
            {
                Console.CursorVisible = false;
                keyInput = Console.ReadKey(true).Key;
            }
        }
        static void OpenControlMenu(Int32 controlValue = 1) // Opens the Control Menu. controlValue can be set to any value for the user to start with a specified option selected, or left empty for default
        {
            Boolean closeControlMenu = false;
            ConsoleKey keyInput = ConsoleKey.NoName;
            const Int32 controlMenuID = 1;
            String[] controlMenu;
            GenerateControlMenu(out controlMenu);
            Console.Clear();
            PrintCurrentMenu(controlMenu, controlValue);
            while (!closeControlMenu)
            {
                Console.CursorVisible = false;
                keyInput = inputFunc();
                menuSystem(controlMenuID, keyInput, ref controlValue, controlMenu, ref closeControlMenu, controlMenu.Length - 2, 1);
            }
        }
        static void PrintCurrentMenu(String[] menu, Int32 currentValue = 0) // prints out a menu - just used to clean up menu functions
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < menu.Length; i++)
            {
                if (i == currentValue)
                {
                    Console.Write(" > ");
                }
                Console.WriteLine(menu[i]);
            }
        }
        static void StartGame() // Starts & Runs Game
        {
            Console.Clear();
            Int32 MeX = 0, MeY = 0, MeSpeed = 100, Screen = 0, Lives = 3; // X coord, Y coord, default frequency of AutoMove (in ms), starting Screen, default Lives
            Boolean checkedLife = false; // checks if gameOver has checked the current tile (whenever gameOver is run, it sets it to true, so it won't remove extra lives for being on a space with a target. AutoMove sets it back to false)
            Int32[] hC;
            const Int32 upDownSpeed = 100, customYBorder = 3;
            Int32[,] tC; // stores coords for the targets. the only way i could think of checking if you were touching a target was using multidimensional arrays, so i read through the MSDN page for them and tried to get a decent-ish understanding of them (i considered using "if (tX.Contains(MeX) && tY[Array.IndexOf(tX, MeX)] == MeY)", but Array.IndexOf apparently always uses the first time it shows up, so that doesn't work
            NewScreen(MeSpeed, ref MeX, ref Screen, out tC, out hC, Lives, customYBorder); // runs NewScreen function for the first screen
            Char MeDir = 'L'; // L is just a random char, i used it in MeMove too. just so that you don't glide up/down
            const Char meChar = '>';
            Console.SetCursorPosition(0, 0);
            Console.Write(meChar);
            DateTime meTimeStamp = DateTime.Now, eTimeStamp = DateTime.Now, upDownTimeStamp = DateTime.Now;
            while (!gameOver(MeX, MeY, tC, hC, ref Lives, ref checkedLife, MeSpeed, Screen))
            {
                //Lives++;
                Console.CursorVisible = false;
                MeKeystroke(ref MeDir, ref MeSpeed, Lives, Screen);
                if (wait(upDownSpeed, ref upDownTimeStamp))
                {
                    MeMove(ref MeX, ref MeY, ref MeDir, meChar, customYBorder);
                }
                if (wait(MeSpeed, ref meTimeStamp))
                {
                    AutoMove(MeSpeed, ref MeX, MeY, meChar, ref Screen, ref tC, ref hC, Lives, ref checkedLife, customYBorder);
                }
            }
            afterGame(Screen);
        }
        static void afterGame(Int32 Screen) // runs after a gameOver
        {
            Console.Clear();
            Console.SetCursorPosition((Console.WindowWidth / 2) - 5, Console.WindowHeight / 2); // 5 comes from next line, chars divided into 2
            Console.Write("Game Over!");
            Console.SetCursorPosition((Console.WindowWidth / 2) - 5, (Console.WindowHeight / 2) + 1);
            Console.Write("Score: " + (Screen - 1));
            Console.SetCursorPosition((Console.WindowWidth / 2) - 19, (Console.WindowHeight / 2) + 2); // 19 comes from next line, chars divided into 2 | 2 is just so that it's 2 lines below GameOver message
            Console.Write("Press ENTER to return to the Main Menu");
            ConsoleKey keyInput = ConsoleKey.NoName;
            if (Screen - 1 > Properties.Settings.Default.highscore)
            {
                Properties.Settings.Default.highscore = Screen - 1;
                Properties.Settings.Default.Save();
            }
            while (keyInput != ConsoleKey.Enter)
            {
                keyInput = Console.ReadKey(true).Key;
            }
        }
        static void AutoMove(Int32 MeSpeed, ref Int32 MeX, Int32 MeY, char meChar, ref Int32 Screen, ref Int32[,] tC, ref Int32[] hC, Int32 Lives, ref bool checkedLife, Int32 customYBorder)
        //function to move constantly to left. vars are (frequency) (x coord) (y coord) (player avatar) (screen #) (target coords) (healing coords) (# lives) (bool to verify if checked space) (bottom indent)
        {
            if (MeX < Console.WindowWidth - 1)
            {
                Console.SetCursorPosition(MeX, MeY);
                Console.Write(" ");
                MeX++;
                Console.SetCursorPosition(MeX, MeY);
                Console.Write(meChar);
            }
            else
            {
                NewScreen(MeSpeed, ref MeX, ref Screen, out tC, out hC, Lives, customYBorder);
            }
            checkedLife = false;
        }
        static void MeKeystroke(ref char MeDir, ref Int32 MeSpeed, Int32 Lives, Int32 Screen) // Reads user Keystroke for MeMove, & Increases/Decreases user Speed
        {
            const Int32 highSpeed = 150/* 10000*/, lowSpeed = 50, speedChange = 50;
            if (Console.KeyAvailable)
            {
                String keyInput = Convert.ToString(Console.ReadKey(true).Key);
                if (keyInput == Properties.Settings.Default.upInput)
                {
                    MeDir = 'N';
                }
                else if (keyInput == Properties.Settings.Default.downInput)
                {
                    MeDir = 'S';
                }
                else if (keyInput == Properties.Settings.Default.speedUp)
                {
                    if (MeSpeed > lowSpeed)
                    {
                        MeSpeed -= speedChange;
                    }
                }
                else if (keyInput == Properties.Settings.Default.speedDown)
                {
                    if (MeSpeed < highSpeed) { MeSpeed += speedChange; }
                }
                bottomMessages(MeSpeed, Lives, Screen);
            }
        }
        static void bottomMessages(Int32 MeSpeed, Int32 Lives, Int32 Screen) // generates the messages at the bottom of the screen
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.Write("Lives: " + Lives);
            String currentSpeed = String.Empty;
            switch (MeSpeed)
            {
                case 50:
                    currentSpeed = " Current Speed: Fast ";
                    break;
                case 100:
                    currentSpeed = "Current Speed: Medium";
                    break;
                case 150:
                    currentSpeed = " Current Speed: Slow ";
                    break;
                default:
                    currentSpeed = "Current Speed: Custom ";
                    break;
            }
            Console.SetCursorPosition((Console.WindowWidth / 2) - (currentSpeed.Length / 2), Console.WindowHeight - 2);
            Console.Write(currentSpeed);
            String screenMsg = $"Current Screen: {Screen}  ";
            Console.SetCursorPosition(Console.WindowWidth - screenMsg.Length, Console.WindowHeight - 2);
            Console.Write(screenMsg);
        }
        static void MeMove(ref Int32 MeX, ref Int32 MeY, ref Char MeDir, Char meChar, Int32 customYBorder) // moves user on screen. vars are X coord, Y coord, input key, & character avatar
        {
            Console.SetCursorPosition(MeX, MeY);
            Console.Write(" ");
            if (MeDir == 'N' || MeDir == 'S' || MeDir == 'E' || MeDir == 'W' || MeDir == ' ')
            {
                switch (MeDir)
                {
                    case 'S':
                        if (MeY < Console.WindowHeight - customYBorder)
                        {
                            MeY++;
                        }
                        else
                        {
                            MeY = 0;
                        }
                        break;
                    case 'N':
                        if (MeY != 0)
                        {
                            MeY--;
                        }
                        else
                        {
                            MeY = Console.WindowHeight - customYBorder;
                        }
                        break;
                }
                MeDir = 'L';
            }
            Console.SetCursorPosition(MeX, MeY);
            Console.Write(meChar);
        }
        static void NewScreen(Int32 MeSpeed, ref Int32 MeX, ref Int32 Screen, out Int32[,] tC, out Int32[] hC, Int32 Lives, Int32 customYBorder) // triggers upon user reaching left side. sends user to right side & spawns new targets. vars are user X coord, user Y coord, screen #, and target coords
        {
            MeX = 0;
            Screen++;
            Console.Clear();
            bottomMessages(MeSpeed, Lives, Screen);
            newTargets(Screen, out tC, customYBorder);
            newHeal(Screen, out hC, customYBorder);
        }
        static Boolean wait(Int32 sec, ref DateTime stamp) // wait function. this was pulled directly from the notes
        {
            if (Math.Abs(DateTime.Now.Millisecond - stamp.Millisecond) < sec)
            {
                return false;
            }
            else
            {
                stamp = DateTime.Now;
                return true;
            }
        }
        static void newHeal(Int32 Screen, out Int32[] hC, Int32 customYBorder)
        {
            const Int32 noHealZone = 5, healFrequency = 3;
            if (Screen % healFrequency == 0)
            {
                Random heals = new Random();
                hC = new int[2];
                hC[0] = heals.Next(noHealZone, Console.WindowWidth);
                hC[1] = heals.Next(0, Console.WindowHeight - customYBorder);
                Console.SetCursorPosition(hC[0], hC[1]);
                ColourWrite(" ", ConsoleColor.Green);
            }
            else
            {
                hC = new int[2];
                hC[1] = Console.WindowHeight + 10; // the 10 is just so that it's spawned out-of-bounds -> it could be literally any positive number and it would work fine, so im not assigning it to a const
                hC[0] = Console.WindowWidth + 10;
            }
        }
        static Boolean gameOver(Int32 MeX, Int32 MeY, Int32[,] tC, Int32[] hC, ref Int32 Lives, ref bool checkedLife, Int32 MeSpeed, Int32 Screen) // detects game overs. checks if user is touching a target
        {
            Boolean gameOver = false;
            if (hC[0] == MeX && hC[1] == MeY && !checkedLife)
            {
                Lives++;
                bottomMessages(MeSpeed, Lives, Screen);
            }
            for (int i = 0; i < tC.GetLength(0); i++)
            {
                if (tC[i, 0] == MeX && tC[i, 1] == MeY)
                {
                    gameOver = true;
                    break;
                }
            }
            if (gameOver && !checkedLife)
            {
                if (Lives == 1)
                {
                    return true;
                }
                else
                {
                    Lives--;
                    bottomMessages(MeSpeed, Lives, Screen);
                }
            }
            checkedLife = true;
            return false;
        }
        static void newTargets(Int32 Screen, out Int32[,] tC, Int32 customYBorder) // summons new targets. for each screen, the targets increase by 20
        {
            const Int32 targetsPerLevel = 20, noTargetZone = 10;
            tC = new int[Screen * targetsPerLevel, Screen * targetsPerLevel];
            Random targets = new Random();
            for (int i = 0; i < tC.GetLength(0); i++)
            {
                tC[i, 0] = targets.Next(noTargetZone, Console.WindowWidth - 1);
                tC[i, 1] = targets.Next(0, Console.WindowHeight - customYBorder);
                Console.SetCursorPosition(tC[i, 0], tC[i, 1]);
                ColourWrite(" ", ConsoleColor.DarkRed);
            }
        }
    }
}