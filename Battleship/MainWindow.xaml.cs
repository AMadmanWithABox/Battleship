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
        public MainWindow()
        {
            InitializeComponent();
            game = new Game();
            updateGrid();
        }

        private void updateGrid()
        {
            //get the board from the game and update the grid
            Game.TileStatus[,] board = game.board;
            //get the grid from the xaml
            Grid grid = (Grid)FindName("grid");
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (board[row, col] == Game.TileStatus.Miss)
                    {
                        //black
                        //get the image elements from the grid
                        Image img = (Image)grid.Children.Cast<UIElement>()
                            .Last(x => Grid.GetRow(x) == row && Grid.GetColumn(x) == col);
                        img.Source = new BitmapImage(new Uri("assets/BlackPiece_lg.png", UriKind.Relative));
                    }
                    else if (board[row, col] == Game.TileStatus.Hit)
                    {
                        //white
                        Image img = (Image)grid.Children.Cast<UIElement>()
                            .Last(x => Grid.GetRow(x) == row && Grid.GetColumn(x) == col);
                        img.Source = new BitmapImage(new Uri("assets/WhitePiece_lg.png", UriKind.Relative));
                    }
                    else
                    {
                        //empty
                        Image img = (Image)grid.Children.Cast<UIElement>()
                            .Last(x => Grid.GetRow(x) == row && Grid.GetColumn(x) == col);
                        img.Source = new BitmapImage(new Uri("assets/EmptyPiece_lg.png", UriKind.Relative));

                    }
                }
            }
        }
        
         private void Image_OnMouseMouseDown(object sender, RoutedEventArgs routedEventArgs)
            {
            Button btn = (Button)sender;
            Grid grid = (Grid)btn.Parent;
            int row = Grid.GetRow(btn);
            int col = Grid.GetColumn(btn);
            Image img = (Image)grid.Children.Cast<UIElement>().Last(x => Grid.GetRow(x) == row && Grid.GetColumn(x) == col);
            //add piece to game class
            //if we want to check if piece can be played it will be here //strech goal
            if (game.isPlayerTurn)
            {
                game.board[row, col] = Game.TileStatus.Miss;
                //check for flips here then update game board array before the grid updates
                flipPieces(row, col, "black");
            }
            else
            {
                game.board[row, col] = Colors.white;
                //check for flips here then update game board array before the grid updates
                flipPieces(row, col, "white");
            }
            game.isPlayerTurn = !game.isPlayerTurn;
            updateGrid();
            btn.IsEnabled = false;
        }


        private void Button_OnMouseEnter(object sender, MouseEventArgs e)
        {
            // We will add code here to check if the move is valid and then change the color of the button
            // to indicate that it is a valid move.
            // for now it will just change the color of the button to gray
            Button btn = (Button)sender;
            if (btn.IsEnabled)
            {
                btn.Background = new SolidColorBrush(System.Windows.Media.Colors.Gray);
            }
        }

        private void Button_OnMouseLeave(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            btn.Background = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
        }

        private void BtnNewGame_OnClick(object sender, RoutedEventArgs e)
        {
            game = new Game();
            updateGrid();
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
    }
}