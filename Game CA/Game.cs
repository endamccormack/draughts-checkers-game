using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Game_CA
{
    [Serializable]
    public class Game
    {
        public enum Turn { RedsTurn, BlackTurn };
        public Turn ColorTurn { get; set; }
        public List<GridSquare> GridSquares;
        int redPoints = 0;
        int blackPoints = 0;
        public int RedPoints { get { return redPoints; } set { redPoints = value; } }
        public int BlackPoints { get { return blackPoints; } set { blackPoints = value; } }

        public static int gridSize = 8;

        [XmlIgnore]
        public Panel[,] draftsBoardPanels;
        [XmlIgnore]
        public Form form;
        [XmlIgnore]
        const int tileSize = 80;

        public Game()
        {

        }

        public Game(Form form)
        {
            GridSquares = new List<GridSquare>();
            this.form = form;
            //start();
        }

        //this creates the blank board with the draughts in their initial positions
        public void startWithGame()
        {
            draftsBoardPanels = new Panel[gridSize, gridSize];
            foreach (GridSquare gs in GridSquares)
            {
                gs.GridPanel.AllowDrop = true;

                gs.GridPanel.Paint += panel1_Paint;
                gs.GridPanel.MouseDown += squareSource_MouseDown;
                gs.GridPanel.DragOver += squareTarget_DragOver;
                gs.GridPanel.DragDrop += squareTarget_DragDrop;

                // add to Form's Controls so that they show up
                form.Controls.Add(gs.GridPanel);

                // add to our 2d array of panels for future use
                draftsBoardPanels[gs.PositionX, gs.PositionY] = gs.GridPanel;
                gs.Draw();
            }
            form.Location = new Point(0, 0);
            form.Size = new Size((tileSize * gridSize) + 50, (tileSize * gridSize) + 50);
        }

        public void start(){
            
            gridSize = 8;
            var clr1 = GridSquare.Colors.Grey;
            var clr2 = GridSquare.Colors.White;

            // initialize the "chess board"
            draftsBoardPanels = new Panel[gridSize, gridSize];

            // double for loop to handle all rows and columns
            for (var n = 0; n < gridSize; n++)
            {
                for (var m = 0; m < gridSize; m++)
                {
                    //set the background color of a piece
                    GridSquare.Status pieceColor = GridSquare.Status.Empty;
                    GridSquare.Colors backColor;
                    if (n % 2 == 0)
                    {
                        if (m % 2 != 0)
                        {
                            backColor = clr1;
                            if (m <= 1)
                            {
                                pieceColor = GridSquare.Status.Black;
                            }
                            if (m > ((Math.Sqrt(draftsBoardPanels.Length)) - 3))
                            {
                                pieceColor = GridSquare.Status.Red;
                            }
                        }
                        else
                        {
                            backColor = clr2;
                        }
                    }
                    else
                    {
                        if (m % 2 != 0)
                        {
                            backColor = clr2;
                        }
                        else
                        {
                            backColor = clr1;
                            if (m <= 1)
                                pieceColor = GridSquare.Status.Black;
                            if (m > ((Math.Sqrt(draftsBoardPanels.Length)) - 3))
                                pieceColor = GridSquare.Status.Red;
                        }
                    }

                    // create new Panel control which will be one draft board tile

                    var newPanel = new GridSquare(new Point(tileSize * n, tileSize * m), backColor, pieceColor, n, m);
                    newPanel.Size = tileSize;
                    newPanel.GridPanel.AllowDrop = true;

                    newPanel.GridPanel.Paint += panel1_Paint;
                    newPanel.GridPanel.MouseDown += squareSource_MouseDown;
                    newPanel.GridPanel.DragOver += squareTarget_DragOver;
                    newPanel.GridPanel.DragDrop += squareTarget_DragDrop;

                    // add to Form's Controls so that they show up
                    form.Controls.Add(newPanel.GridPanel);

                    // add to our 2d array of panels for future use
                    draftsBoardPanels[n, m] = newPanel.GridPanel;
                    newPanel.Draw();
                    GridSquares.Add(newPanel);
                }
            }
            form.Location = new Point(0, 0);
            form.Size = new Size((tileSize * gridSize) + 50, (tileSize * gridSize) + 50);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            GridSquare panel1 = (GridSquare)sender;
            panel1.Draw();

            MessageBox.Show("Clicked");
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bmp = ((GridPanel)sender).GridSquareOwner.BMP;
            e.Graphics.DrawImage(bmp, new Point(0, 0));
        }

        //All the squares include drag and drop functionality
        private void squareSource_MouseDown(object sender, MouseEventArgs e)
        {
            GridPanel sqr = (GridPanel)sender;
            sqr.DoDragDrop(sqr, DragDropEffects.Move);
        }

        private void squareTarget_DragDrop(object sender, DragEventArgs e)
        {
            GridPanel sqr1 = (GridPanel)e.Data.GetData("Game_CA.GridPanel");
            GridPanel sqr = (GridPanel)sender;

            sqr1.GridSquareOwner.tryMove(sqr);
            checkForWin();
        }
        private void squareTarget_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        public void checkForWin()
        {
            List<GridSquare> blacks = GridSquares.Where(x => new List<GridSquare.Status>() { GridSquare.Status.Black, GridSquare.Status.KingBlack }.Contains(x.Occupied)).ToList<GridSquare>();
            List<GridSquare> reds = GridSquares.Where(x => new List<GridSquare.Status>() { GridSquare.Status.Red, GridSquare.Status.KingRed }.Contains(x.Occupied)).ToList<GridSquare>();

            if(reds.Count < 1)
            {
                MessageBox.Show("Blacks win");
                form.Close();
            }
            if (blacks.Count < 1)
            {
                MessageBox.Show("Reds win");
                form.Close();
            }
        }
    }
}
