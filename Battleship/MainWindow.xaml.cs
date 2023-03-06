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
        bool isVertical = false;
        
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
        
         private void Image_OnMouseMouseDown(object sender, RoutedEventArgs routedEventArgs)
            {
            Button btn = (Button)sender;
            Grid grid = (Grid)btn.Parent;
            
            
            
            if(game.phase == Game.Phase.PlaceShips && grid.Name == "FireAtPlayer")
            {
                int y = Grid.GetRow(btn);
                int x =Grid.GetColumn(btn);
                game.player.PlaceShips(x, y , isVertical);
                

            }

            //update both boards
            UpdateBoard();
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
                    if (currentBoard[x, y].status == Player.TileStatus.Status.Ship)
                        btn.Background = new SolidColorBrush(Colors.Gray);
                    else if (currentBoard[x, y].status == Player.TileStatus.Status.Hit)
                        btn.Background = new SolidColorBrush(Colors.Red);
                    else if (currentBoard[x, y].status == Player.TileStatus.Status.Miss)
                        btn.Background = new SolidColorBrush(Colors.Blue);
                    else if (currentBoard[x, y].status == Player.TileStatus.Status.Sunk)
                        btn.Background = new SolidColorBrush(Colors.Black);
                    else
                        btn.Background = new SolidColorBrush(Colors.Transparent);
                }
            }

        }


        private void Button_OnMouseEnter(object sender, MouseEventArgs e)
        {
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
        }

        private void BtnNewGame_OnClick(object sender, RoutedEventArgs e)
        {
            game = new Game();
            //updateGrid();
            //reset the buttons
            Grid grid = (Grid)FindName("grid");
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Button btn = (Button)grid.Children.Cast<UIElement>().First(x => Grid.GetRow(x) == row && Grid.GetColumn(x) == col);
                    btn.IsEnabled = true;
                }
            }
            //reset output
            Label output = (Label)FindName("lblGameData");
            output.Content = "";
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
            if (e.Key == Key.Space)
            {
                isVertical = !isVertical;
                //refresh the board
                foreach (Button btn in FireAtPlayer.Children)
                {
                    int x = Grid.GetColumn(btn);
                    int y = Grid.GetRow(btn);
                    if(game.player.Board[x,y].status == Player.TileStatus.Status.Ship)
                        btn.Background = new SolidColorBrush(Colors.Gray);
                    else if(game.player.Board[x,y].status == Player.TileStatus.Status.Hit)
                        btn.Background = new SolidColorBrush(Colors.Red);
                    else if(game.player.Board[x,y].status == Player.TileStatus.Status.Miss)
                        btn.Background = new SolidColorBrush(Colors.Blue);
                    else if(game.player.Board[x,y].status == Player.TileStatus.Status.Sunk)
                        btn.Background = new SolidColorBrush(Colors.Black);
                    else 
                        btn.Background = new SolidColorBrush(Colors.Transparent);
                }
            }
        }
    }
}