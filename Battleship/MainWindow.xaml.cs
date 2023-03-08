using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game;
        private bool isVertical { set; get; }
        private bool _cheatMode = true;
        
        public MainWindow()
        {
            InitializeComponent();
            game = new Game();
            //updateGrid();
        }

        //private void updateGrid()
        //{
            //get the board from the game and update the grid
        //     Game.TileStatus[,] board = game.board;
        //     //get the grid from the xaml
        //     Grid grid = (Grid)FindName("grid");
        //     for (int row = 0; row < 8; row++)
        //     {
        //         for (int col = 0; col < 8; col++)
        //         {
        //             if (board[row, col] == Game.TileStatus.Miss)
        //             {
        //                 //black
        //                 //get the image elements from the grid
        //                 Image img = (Image)grid.Children.Cast<UIElement>()
        //                     .Last(x => Grid.GetRow(x) == row && Grid.GetColumn(x) == col);
        //                 img.Source = new BitmapImage(new Uri("assets/BlackPiece_lg.png", UriKind.Relative));


        //             }
        //             else if (board[row, col] == Game.TileStatus.Hit)
        //             {
        //                 //white
        //                 Image img = (Image)grid.Children.Cast<UIElement>()
        //                     .Last(x => Grid.GetRow(x) == row && Grid.GetColumn(x) == col);
        //                 img.Source = new BitmapImage(new Uri("assets/WhitePiece_lg.png", UriKind.Relative));
        //             }
        //             else
        //             {
        //                 //empty
        //                 Image img = (Image)grid.Children.Cast<UIElement>()
        //                     .Last(x => Grid.GetRow(x) == row && Grid.GetColumn(x) == col);
        //                 img.Source = new BitmapImage(new Uri("assets/EmptyPiece_lg.png", UriKind.Relative));
        //
        //             }
        //         }
        //     }
        // }
        
         private void Button_OnClick(object sender, RoutedEventArgs routedEventArgs)
            {
            Button btn = (Button)sender;
            Grid grid = (Grid)btn.Parent;
            
            int y = Grid.GetRow(btn);
            int x =Grid.GetColumn(btn);
            if(game.phase == Game.Phase.PlaceShips && grid.Name == "FireAtPlayer")
            {
                game.player.PlaceShips(x, y , isVertical);

                if (game.phase == Game.Phase.PlayGame)
                {
                    foreach (Button button in FireAtPlayer.Children.Cast<Button>())
                    {
                        button.IsEnabled = false;
                    }
                    return;
                }
            }

            if (game.phase == Game.Phase.PlayGame)
            {
                if (game.isPlayerTurn)
                {
                    game.Fire(x, y);
                    if (game.CheckForWin(game.player))
                    {
                        disableGrids();
                        MessageBox.Show("You Win!");
                    }
                    else if (game.CheckForWin(game.computer))
                    {
                        disableGrids();
                        MessageBox.Show("You Don't Win ): !");
                    }
                }
            }

           

            //update both boards
            UpdateBoard();
        }

        private void disableGrids()
        {
            foreach (Button button in FireAtPlayer.Children.Cast<Button>())
            {
                button.IsEnabled = false;
            }
            foreach (Button button in FireAtCPUBoard.Children.Cast<Button>())
            {
                button.IsEnabled = false;
            }
        }


        private void UpdateBoard()
        {
            Player.TileStatus[,] board = game.player.Board;
            Player.TileStatus[,] cpuboard = game.computer.Board;
            
            Grid grid = (Grid)FindName("FireAtPlayer");
            Grid cpugrid = (Grid)FindName("FireAtCPUBoard");

            Grid[] grids = { grid, cpugrid };
            
            for(int i =0; i < 2; i++)
            {
                Grid g = grids[i];
                Player.TileStatus[,] currentBoard;
                if(i == 0)
                {
                    currentBoard = board;
                }else
                {
                    currentBoard = cpuboard;
                }

                
                foreach (Button btn in g.Children)
                {
                    int x = Grid.GetColumn(btn);
                    int y = Grid.GetRow(btn);
                    if (g.Name != "FireAtCPUBoard" || _cheatMode)
                    {
                        if (currentBoard[x, y].status == Player.TileStatus.Status.Ship)
                            btn.Background = new SolidColorBrush(Colors.Gray);
                    }
                    if (currentBoard[x, y].status == Player.TileStatus.Status.Hit)
                        btn.Background = new SolidColorBrush(Colors.Red);
                    if (currentBoard[x, y].status == Player.TileStatus.Status.Miss)
                        btn.Background = new SolidColorBrush(Colors.Blue);
                    if (currentBoard[x, y].status == Player.TileStatus.Status.Sunk)
                        btn.Background = new SolidColorBrush(Colors.Black);
                    if(currentBoard[x, y].status == Player.TileStatus.Status.Empty)
                        btn.Background = new SolidColorBrush(Colors.Transparent);
                }
            }
        }


        private void Button_OnMouseEnter(object sender, MouseEventArgs e)
        {
            UpdateBoard();
            // We will add code here to check if the move is valid and then change the color of the button
            // to indicate that it is a valid move.
            // for now it will just change the color of the button to gray
            Button btn = (Button)sender;
            Grid grid = (Grid)btn.Parent;
            //    grid.Name = "grid";
            if (btn.IsEnabled)
            {
                
                if (game.phase == Game.Phase.PlaceShips && grid.Name == "FireAtPlayer")
                {
                    int length = game.player.ShipsToPlace;
                    if (length == 1) length = 3;
                    int y = Grid.GetRow(btn);
                    int x = Grid.GetColumn(btn);
                    if (game.CheckPlacement(x, y, length, isVertical))
                    {
                        for (int i = 0; i < length; i++)
                        {
                            //if you can place the ship, change the color of the buttons where the ship would go to green
                            if(!isVertical) FireAtPlayer.Children.Cast<Button>().First(btn => Grid.GetRow(btn) == y && Grid.GetColumn(btn) == x + i).Background = new SolidColorBrush(Colors.Green);
                            else FireAtPlayer.Children.Cast<Button>().First(btn => Grid.GetRow(btn) == y + i && Grid.GetColumn(btn) == x).Background = new SolidColorBrush(Colors.Green);
                        }
                    }
                }
                else if (game.phase == Game.Phase.PlayGame){
                    if (game.isPlayerTurn)
                        btn.Background = new SolidColorBrush(Colors.Gray);
                }
            }
        }

        private void Button_OnMouseLeave(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            
            if (btn.IsEnabled)
            {
                if (game.phase == Game.Phase.PlaceShips)
                {
                    int length = game.player.ShipsToPlace;
                    if (length == 1) length = 3;
                    int y = Grid.GetRow(btn);
                    int x = Grid.GetColumn(btn);
                    if (game.CheckPlacement(x, y, length, isVertical))
                    {
                        for (int i = 0; i < length; i++)
                        {
                            //if you can place the ship, change the color of the buttons where the ship would go to green
                            if(!isVertical) FireAtPlayer.Children.Cast<Button>().First(btn => Grid.GetRow(btn) == y && Grid.GetColumn(btn) == x + i).Background = new SolidColorBrush(Colors.Transparent);
                            else FireAtPlayer.Children.Cast<Button>().First(btn => Grid.GetRow(btn) == y + i && Grid.GetColumn(btn) == x).Background = new SolidColorBrush(Colors.Transparent);
                        }
                    }
                }
                else if (game.phase == Game.Phase.PlayGame){
                    if (game.isPlayerTurn)
                        btn.Background = new SolidColorBrush(Colors.Transparent);
                }
            }
            UpdateBoard();
        }

        private void BtnNewGame_OnClick(object sender, RoutedEventArgs e)
        {
            game = new Game();
            UpdateBoard();
            foreach (Button button in FireAtPlayer.Children.Cast<Button>())
            {
                button.IsEnabled = true;
            }

        }

        private void BtnNewGame_OnMouseEnter(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.IsEnabled)
            {
                btn.Background = new SolidColorBrush(System.Windows.Media.Colors.Gray);
            }
        }

        private void BtnNewGame_OnMouseLeave(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            btn.Background = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
        }

        private void Window_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R)
            {
                isVertical = !isVertical;
                //refresh the board
                UpdateBoard();
            }
        }
    }
}