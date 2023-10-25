﻿using WMPLib;
namespace Tjuv___Polis
{
    internal class Program
    {
        public static int citySizeX = 100;
        public static int citySizeY = 25;
        public static int prisonSize = 10;
        public static int robberyCount = 4;
        public static int arrestCount = 0;

        //Create list of prisoners
        public static List<Person> prisonList = new List<Person>();

        //Create list of people
        public static List<Person> personList = new List<Person>();
        static void Main(string[] args)
        {
            Random rng = new Random();
            //Create city map (matrix)
            Person[,] cityMap = new Person[citySizeX, citySizeY];
            //Create prison map (matrix)
            Person[,] prisonMap = new Person[prisonSize, prisonSize];
            //Create an instance of MediaPlayer for surprise!
            WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
            int vigilanteSpawnCD = 0;
            int vigilanteDespawnCD = 0;

            //Add 20 Policemen
            for (int i = 0; i < 20; i++)
            {
                personList.Add(new Police());
            }

            //Add 20 Thieves
            for (int i = 0; i < 20; i++)
            {
                personList.Add(new Thief());
            }

            //Add 30 Civilians
            for (int i = 0; i < 30; i++)
            {
                personList.Add(new Civilian());
            }
            bool showMap = true;

            //Main loop
            while (true)
            {
                //Erase previous position of people
                Array.Clear(cityMap);
                Array.Clear(prisonMap);

                //SHow map or list on key press
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.KeyChar == 'm')
                        showMap = true;
                    Console.Clear();
                    if (key.KeyChar == 'i')
                        showMap = false;
                    Console.Clear();
                }

                //Add each person's position into the city map
                foreach (Person people in personList)
                {
                    cityMap[people.PosX, people.PosY] = people;
                }

                //Show map view
                if (showMap)
                {
                    //Print map of city and people
                    Console.WriteLine("City");
                    for (int i = 0; i <= citySizeX; i++)
                    {
                        Console.Write("_");
                    }
                    Console.WriteLine();

                    for (int y = 0; y < cityMap.GetLength(1); y++)
                    {
                        for (int x = 0; x < cityMap.GetLength(0); x++)
                        {
                            if (cityMap[x, y] is Police)
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write("P");
                            }
                            if (cityMap[x, y] is Thief)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("T");
                            }
                            if (cityMap[x, y] is Civilian)
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("M");
                            }
                            if (cityMap[x, y] is Vigilante)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("V");
                            }
                            else
                            {
                                Console.Write(" ");
                            }
                        }
                        Console.WriteLine();
                    }

                    for (int i = 0; i <= citySizeX; i++)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("_");
                    }
                    Console.WriteLine();

                    Console.WriteLine();

                    //Add each person's position into the prison map
                    foreach (Person people in prisonList)
                    {
                        prisonMap[people.PosX, people.PosY] = people;
                    }

                    //Print prison
                    Console.WriteLine("Prison");
                    for (int i = 0; i <= prisonSize; i++)
                    {
                        Console.Write("_");
                    }
                    Console.WriteLine();

                    for (int y = 0; y < prisonMap.GetLength(1); y++)
                    {
                        for (int x = 0; x < prisonMap.GetLength(0); x++)
                        {
                            if (prisonMap[x, y] is Thief)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("T");
                            }
                            else
                            {
                                Console.Write(" ");
                            }
                        }
                        Console.WriteLine();
                    }
                    for (int i = 0; i <= prisonSize; i++)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("_");
                    }
                    Console.WriteLine();
                }

                //Move people around the prison
                for (int i = 0; i < prisonList.Count; i++)
                {
                    prisonList[i].Move();
                }

                //Move people around the city
                for (int i = 0; i < personList.Count; i++)
                {
                    personList[i].Move();
                }


                //Spawn a vigilante based on the criminal activity of the city. In this case, whenever the criminal activity of the city is a divisible by 5, i.e. 5, 10, 15...
                if (robberyCount % 5 == 0 && robberyCount != 0 && vigilanteSpawnCD == 0)
                {
                    //Randomize an index from a temporary list of Civilian objects
                    List<int> vigilanteList = new List<int>();
                    for (int i = 0; i < personList.Count; i++)
                    {
                        if (personList[i] is Civilian)
                        {
                            vigilanteList.Add(i);
                        }
                    }

                    //From the randomized index value of Civilian objects, spawn a Vigilante that inherits their position and held items. 
                    int vigilantePick = vigilanteList[rng.Next(vigilanteList.Count)];
                    personList[vigilantePick] = new Vigilante(personList[vigilantePick].PosX, personList[vigilantePick].PosY, ((Civilian)personList[vigilantePick]).Possessions);
                    int heroPick = rng.Next(4);

                    switch (heroPick)
                    {
                        case 0:
                            Console.WriteLine("Kriminaliteten är så hög att medborgaren Bruce Wayne tar på sig Batman-dräkten och säger: \"I'm BATMAN!\". Tjuvar, se upp!");
                            player.URL = "BatmanThemeSong.mp3";
                            player.settings.volume = 25;
                            player.settings.setMode("loop", true);
                            player.controls.play();
                            break;
                        case 1:
                            Console.WriteLine("Luke Skywalker landar med sin X-Wing och leder motståndsrörelsen mot den mörka sidan!");
                            player.URL = "TheForceThemeSong.mp3";
                            player.settings.volume = 50;
                            player.settings.setMode("loop", true);
                            player.controls.play();
                            break;
                        case 2:
                            Console.WriteLine("Spider Man svingar runt och vakar över stadens invånare!");
                            player.URL = "SpiderManThemeSong.mp3";
                            player.settings.volume = 25;
                            player.settings.setMode("loop", true);
                            player.controls.play();
                            break;
                        case 3:
                            Console.WriteLine("Iron Man tar på sig sin power suit och undsätter polisstyrkan i deras kamp mot den ökande kriminaliteten!");
                            player.URL = "IronManThemeSong.mp3";
                            player.settings.volume = 25;
                            player.settings.setMode("loop", true);
                            player.controls.play();
                            break;
                        default:
                            Console.WriteLine("En ny superhjälte har tagit på sig sin mantel!");
                            break;
                    }
                    vigilanteSpawnCD = 5;
                    Thread.Sleep(4000);

                    robberyCount++;
                }

                if (arrestCount % 5 == 0 && arrestCount != 0 && vigilanteDespawnCD == 0)
                {
                    for (int i = 0; i < personList.Count; i++)
                    {
                        if (personList[i] is Vigilante)
                        {
                            personList[i] = new Civilian();
                            Console.WriteLine("With their work being done, the vigilante hero disappears into the night, for now...");
                            vigilanteDespawnCD = 10;
                            Thread.Sleep(2000);
                            break;
                        }

                    }
                    player.controls.stop();
                }

                //Show list view
                if (!showMap)
                {
                    for (int i = 0; i < personList.Count; i++)
                    {
                        Console.Write("Person " + (i + 1) + " ");
                        personList[i].GetInfo();
                    }
                }

                Console.WriteLine();

                //Compares people's coordinates to initiate action if needed
                for (int i = 0; i < personList.Count; i++)
                {
                    for (int y = 0; y < personList.Count; y++)
                    {
                        if (personList[i].PosY == personList[y].PosY && personList[i].PosX == personList[y].PosX && personList[i] != personList[y])
                        {
                            personList[i].Action(personList[y]);
                        }
                    }
                }

                if (vigilanteSpawnCD > 0)
                {
                    vigilanteSpawnCD--;
                }

                if (vigilanteDespawnCD > 0)
                {
                    vigilanteDespawnCD--;

                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Antal rånade medborgare: {robberyCount}");
                Console.WriteLine($"Antal gripna tjuvar: {arrestCount}");
                Thread.Sleep(1000);
                Console.Clear();
            }
        }
    }
}