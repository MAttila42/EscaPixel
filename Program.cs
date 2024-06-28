using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace EscapePixel
{
    class Program
    {
        static void FileTest()
        {
            if (!File.Exists("world1.txt") || !File.Exists("world2.txt"))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n HIBA\n");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(" Nem található meg a program összes fájlja! Ellenőrizd, hogy az alkalmazás mellett megtalálható-e az összes pálya!\n");
                Console.WriteLine(" A kilépéshez nyomj meg egy gombot...");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        static void Main()
        {
            FileTest();

            Console.WindowWidth = 82;
            Console.WindowHeight = 32;
            Console.ForegroundColor = ConsoleColor.Black;

            for (int currentLevel = 1; currentLevel <= 2; currentLevel++)
            {
                string currentWorld = "world" + currentLevel.ToString() + ".txt";
                string[] world = File.ReadAllLines(currentWorld);
                int[] playerPos = new int[2] { 20, 15 };

                byte score = 0;
                byte hammerCount = 0;

                while (true)
                {
                    Console.Clear();

                    int xPos = playerPos[0] - 20;
                    int yPos = playerPos[1] - 15;

                    for (int y = 0; y < 30; y++)
                    {
                        for (int x = 0; x < 41; x++)
                        {
                            string currentPixel = world[yPos];
                            if (currentPixel[xPos] == '0') Console.BackgroundColor = ConsoleColor.Black; // Fal
                            else if (currentPixel[xPos] == '1') Console.BackgroundColor = ConsoleColor.DarkGray; // Akadály
                            else if (currentPixel[xPos] == '2') Console.BackgroundColor = ConsoleColor.Yellow; // Érme
                            else if (currentPixel[xPos] == '3') Console.BackgroundColor = ConsoleColor.Gray; // Bontókalapács
                            else if (currentPixel[xPos] == '4') Console.BackgroundColor = ConsoleColor.DarkRed; // Láva
                            else if (currentPixel[xPos] == '5') Console.BackgroundColor = ConsoleColor.DarkMagenta; // Portál
                            else Console.BackgroundColor = ConsoleColor.White;

                            if (xPos == playerPos[0] && yPos == playerPos[1]) Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write("  ");
                            xPos++;
                        }
                        Console.WriteLine();
                        yPos++;
                        xPos = playerPos[0] - 20;
                    }

                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write("Inventory: ");
                    Console.Write($"{score} db Érme");
                    if (hammerCount >= 1) Console.WriteLine($", {hammerCount} db bontókalapács");

                    //if (score == 5) break;

                    bool delPixel = false;
                    bool delUp = false;
                    bool delDown = false;
                    bool delRight = false;
                    bool delLeft = false;

                    for (int i = 0; i < 1; i++)
                    {
                        char key = Console.ReadKey(true).KeyChar;
                        switch (key)
                        {
                            case 'w': if (world[playerPos[1] - 1].ToString()[playerPos[0]] != '0' && world[playerPos[1] - 1].ToString()[playerPos[0]] != '1') playerPos[1]--; else i--; break;
                            case 'a': if (world[playerPos[1]].ToString()[playerPos[0] - 1] != '0' && world[playerPos[1]].ToString()[playerPos[0] - 1] != '1') playerPos[0]--; else i--; break;
                            case 's': if (world[playerPos[1] + 1].ToString()[playerPos[0]] != '0' && world[playerPos[1] + 1].ToString()[playerPos[0]] != '1') playerPos[1]++; else i--; break;
                            case 'd': if (world[playerPos[1]].ToString()[playerPos[0] + 1] != '0' && world[playerPos[1]].ToString()[playerPos[0] + 1] != '1') playerPos[0]++; else i--; break;
                            case '': Environment.Exit(0); break;
                            case 'f':
                                delPixel = true;
                                if (world[playerPos[1] - 1].ToString()[playerPos[0]] == '1' && hammerCount >= 1)
                                {
                                    delUp = true;
                                    hammerCount--;
                                }
                                else if (world[playerPos[1] + 1].ToString()[playerPos[0]] == '1' && hammerCount >= 1)
                                {
                                    delDown = true;
                                    hammerCount--;
                                }
                                else if (world[playerPos[1]].ToString()[playerPos[0] + 1] == '1' && hammerCount >= 1)
                                {
                                    delRight = true;
                                    hammerCount--;
                                }
                                else if (world[playerPos[1]].ToString()[playerPos[0] - 1] == '1' && hammerCount >= 1)
                                {
                                    delLeft = true; hammerCount--;
                                }
                                else i--;
                                break;
                            default: i--; break;
                        }
                    }

                    bool coin = world[playerPos[1]].ToString()[playerPos[0]] == '2';
                    bool hammer = world[playerPos[1]].ToString()[playerPos[0]] == '3';

                    if (world[playerPos[1]].ToString()[playerPos[0]] == '4')
                    {
                        currentLevel--;
                        break;
                    }

                    if (world[playerPos[1]].ToString()[playerPos[0]] == '5') break;

                    if (delPixel || coin || hammer)
                    {
                        List<char> temporary = new List<char>();

                        if (delUp) for (int i = 0; i < playerPos[0]; i++) temporary.Add(world[playerPos[1] - 1].ToString()[i]);
                        else if (delDown) for (int i = 0; i < playerPos[0]; i++) temporary.Add(world[playerPos[1] + 1].ToString()[i]);
                        else if (delRight) for (int i = 0; i < playerPos[0] + 1; i++) temporary.Add(world[playerPos[1]].ToString()[i]);
                        else if (delLeft) for (int i = 0; i < playerPos[0] - 1; i++) temporary.Add(world[playerPos[1]].ToString()[i]);
                        else for (int i = 0; i < playerPos[0]; i++) temporary.Add(world[playerPos[1]].ToString()[i]);

                        temporary.Add(' ');

                        if (delUp) for (int i = temporary.Count; i < world[playerPos[1] - 1].Length; i++) temporary.Add(world[playerPos[1] - 1][i]);
                        else if (delDown) for (int i = temporary.Count; i < world[playerPos[1] + 1].Length; i++) temporary.Add(world[playerPos[1] + 1][i]);
                        else for (int i = temporary.Count; i < world[playerPos[1]].Length; i++) temporary.Add(world[playerPos[1]][i]);

                        string temporaryStr = new string(temporary.ToArray());

                        if (delUp) world[playerPos[1] - 1] = temporaryStr;
                        else if (delDown) world[playerPos[1] + 1] = temporaryStr;
                        else world[playerPos[1]] = temporaryStr;

                        if (coin || hammer)
                        {
                            if (coin) score++;
                            if (hammer) hammerCount++;
                        }
                    }
                }
            }
        }
    }
}
