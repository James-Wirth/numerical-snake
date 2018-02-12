using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Snake
{
    public class Consts{
        public const int gridWidth = 20;
        public const int gridHeight = 10;

        public const int targetIdentifier = 999;

        public const int upDirection = 0;
        public const int downDirection = 1;
        public const int leftDirection = 2;
        public const int rightDirection = 3;
    }

    class MainClass
    {

        static bool gameInProgress = true;

	static int[,] grid = new int[30, 30];
        static int snakeLength = 1;

        static int currentRow = 0;
        static int currentCol = 0;

        static int lastx = 0;
        static int lasty = 0;

        static int targetRow = 0;
        static int targetCol = 0;

        static int direction = Consts.rightDirection;

        public static void Main(string[] args)
        {
            playGame();
        }

        public static void playGame(){
		Thread keyThread = new Thread(new ThreadStart(getInputs));
		keyThread.Start();

		generateTarget();

		grid[currentRow, currentCol] = snakeLength;
		display();

		while (gameInProgress)
		{
			//Change the position of the first block and check whether it has hit the target, or itself/the game bounds (in which case the game ends)
			switch (direction)
			{
				case 0:
					if ((currentCol == targetCol) && (currentRow - 1 == targetRow))
					{
						targetFound();
					}
					else if (currentRow == 0 || grid[currentRow - 1, currentCol] > 0)
					{
                            			gameOver();
                            			break;
					}
					grid[currentRow - 1, currentCol] = snakeLength;
					currentRow--;
					break;

				case 1:
					if ((currentCol == targetCol) && (currentRow + 1 == targetRow))
					{
						targetFound();
					}
					else if (currentCol == Consts.gridHeight-1 || grid[currentRow + 1, currentCol] > 0)
					{
						gameOver();
                           			break;
					}
					grid[currentRow + 1, currentCol] = snakeLength;
					currentRow++;
					break;

				case 2:
					if ((currentCol - 1 == targetCol) && (currentRow == targetRow))
					{
						targetFound();
					}
					else if (currentCol == 0 || grid[currentRow, currentCol - 1] > 0)
					{
						gameOver();
                            			break;
					}
					grid[currentRow, currentCol - 1] = snakeLength;
					currentCol--;
					break;

				case 3:
					if ((currentCol + 1 == targetCol) && (currentRow == targetRow))
					{
						targetFound();
					}
					else if (currentCol == Consts.gridWidth-1 || grid[currentRow, currentCol + 1] > 0)
					{
						gameOver();
                            			break;
					}
					grid[currentRow, currentCol + 1] = snakeLength;
					currentCol++;
					break;
			}

                	if (gameInProgress)
                	{
                    	//Display the updated positions
                    	display();

                    	//Decrement each visible block > 0
                    	for (int i = 0; i < Consts.gridHeight; i++)
                    	{
                        	for (int j = 0; j < Consts.gridWidth; j++)
                        	{
                            		if ((grid[i, j] > 0 && grid[i, j] != Consts.targetIdentifier))
                            		{
                                		if (grid[i, j] == 1)
                                		{
                                    			lastx = i;
                                    			lasty = j;
                                		}

                                		grid[i, j]--;
                            		}
                        	}
                    	}
                    	Thread.Sleep(200);
               	 	}
		}
        }

        public static void gameOver(){
            	Console.Clear();
            	gameInProgress = false;

            	Console.WriteLine("Your Score: " + snakeLength);
        }

        public static void targetFound(){
		snakeLength++;
		generateTarget();

		for (int i = 0; i < Consts.gridHeight; i++)
		{
			for (int j = 0; j < Consts.gridWidth; j++)
			{
                    		if(grid[i,j] != snakeLength && grid[i,j] > 0 && grid[i,j] != Consts.targetIdentifier){
                        		grid[i, j]++;
                    		}
			}
		}

            	grid[lastx, lasty] = 1;
        }

	public static void generateTarget()
	{
		Random random = new Random();
            	targetRow = random.Next(0, Consts.gridHeight-1);
            	targetCol = random.Next(0, Consts.gridWidth-1);

		grid[targetRow, targetCol] = Consts.targetIdentifier;
	}

        public static void getInputs(){
            	while(true){

                	ConsoleKeyInfo key = new ConsoleKeyInfo();
                	key = Console.ReadKey();

               		switch(key.Key){
                    		case ConsoleKey.W:
                        		direction = 0;
                        		break;
		    		case ConsoleKey.S:
					direction = 1;
					break;
		    		case ConsoleKey.A:
					direction = 2;
					break;
		    		case ConsoleKey.D:
					direction = 3;
					break;
                	}
            	}
        }

        public static void display(){
            	Console.Clear();
		for (int i = 0; i < Consts.gridHeight; i++)
		{
			for (int j = 0; j < Consts.gridWidth; j++)
                	{
                    		if (grid[i, j] == Consts.targetIdentifier)
                    		{
                        		Console.ForegroundColor = ConsoleColor.Green;
                        		Console.Write(" X ");
                    		}
                    		else
                    		{
                        		if (grid[i, j] > 0)
                        		{
                            			Console.ForegroundColor = ConsoleColor.Yellow;
                        		}
                        		else
                        		{
                            			Console.ForegroundColor = ConsoleColor.DarkGray;
                        		}			

                        		if (grid[i, j] < 10)
                        		{
                            			Console.Write(string.Format(" {0} ", grid[i, j]));
                        		}
                        		else{
                            			Console.Write(string.Format(" {0}", grid[i, j]));
                        		}
                    		}
			}
			Console.Write(Environment.NewLine + Environment.NewLine);
		}
        }

    }
}
