using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Xml.Serialization;

namespace Game_CA
{
    /*
     *This is the most complex piece of code, it encapsulates the meaning of the gridSquare and nearly everything that affects
     *a grid square is handled in this class. There is a whole lot of logic associated with this as most of the interactions in draughts 
     *is at a draught or gridSquare level.
     */
    [Serializable]
    public class GridSquare 
    {
        [XmlIgnore()]
        GridPanel panel;
        [XmlIgnore()]
        public GridPanel GridPanel { get { return panel; } set { panel = value; } }
        public enum Status { Empty, Black, Red, KingRed, KingBlack};
        public enum Colors { White, Grey }

        //what occupies the square
        public Status Occupied { get; set; }
        public Colors TileColor { get; set; }

        //X and Y position which is handy when redrawing from load
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        private int defaultTileSize = 40;
        public int Size { get { return panel.Size.Height; } set { this.panel.Size = new Size(value, value); defaultTileSize = value;  } }

        public Point Location { get { return panel.Location; } set { panel.Location = value; } }

        //ignoring these as they are unserialisable for XML even if they werent there is no point in storing drawn assets, one of the many benefits that
        // drawing the board yeilded;
        [XmlIgnore()]
        Bitmap bmp;
        //[NonSerialized()]
        [XmlIgnore()]
        public Bitmap BMP { get { return bmp == null ? new Bitmap(defaultTileSize, defaultTileSize) : bmp; } set { bmp = value; } }

        //several different useful constructors for GridSquare
        public GridSquare()
        {
            panel = new GridPanel(this);
        }
      
        public GridSquare(Point Location, Colors TileColor, Status Occupied)
        {
            panel = new GridPanel(this);
            this.Location = Location;
            this.TileColor = TileColor;
            this.Occupied = Occupied;

            this.Size = defaultTileSize;

        }

        public GridSquare(Point Location, Colors TileColor, Status Occupied, int n, int m)
        {
            panel = new GridPanel(this);
            this.Location = Location;
            this.TileColor = TileColor;
            this.Occupied = Occupied;

            this.PositionX = n;
            this.PositionY = m;

            this.Size = defaultTileSize;

           // Draw();
        }
        
        /*this is my draw method, you might think it odd that I chose to do this entirely in code but rending these graphics
         * was the most efficient ways to achieve the tasks and I am quite happy with the aestetic that we finished on.
         * I could have done with more time for more styling but in terms of code rendering it looks pretty nice
         */
        public void Draw()
        {
            Bitmap disbmp = new Bitmap(defaultTileSize, defaultTileSize);
          
            using (Graphics g = Graphics.FromImage(disbmp))
            {
                if (this.TileColor == Colors.Grey)
                    g.Clear(System.Drawing.Color.Gray);
                else if (this.TileColor == Colors.White)
                    g.Clear(System.Drawing.Color.White);

                if (this.Occupied == GridSquare.Status.Red)
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.FillEllipse(new HatchBrush(HatchStyle.OutlinedDiamond, System.Drawing.Color.DarkRed, System.Drawing.Color.Red), new Rectangle(Convert.ToInt32(defaultTileSize * .05), Convert.ToInt32(defaultTileSize * .05), Convert.ToInt32(defaultTileSize * .90), Convert.ToInt32(defaultTileSize * .90)));
                    g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                }
                else if (this.Occupied == GridSquare.Status.Black)
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.FillEllipse(new HatchBrush(HatchStyle.OutlinedDiamond, System.Drawing.Color.Gray, System.Drawing.Color.Black), new Rectangle(Convert.ToInt32(defaultTileSize * .05), Convert.ToInt32(defaultTileSize * .05), Convert.ToInt32(defaultTileSize * .90), Convert.ToInt32(defaultTileSize * .90)));
                    g.InterpolationMode = InterpolationMode.HighQualityBilinear; 
                }
                else if (this.Occupied == GridSquare.Status.KingRed)
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.FillEllipse(new HatchBrush(HatchStyle.SolidDiamond, System.Drawing.Color.DarkRed, System.Drawing.Color.Red), new Rectangle(Convert.ToInt32(defaultTileSize * .05), Convert.ToInt32(defaultTileSize * .05), Convert.ToInt32(defaultTileSize * .90), Convert.ToInt32(defaultTileSize * .90)));
                    //g.FillEllipse(System.Drawing.Brushes.White, new Rectangle(defaultTileSize / 4, defaultTileSize / 4, defaultTileSize / 2, defaultTileSize / 2));
                    g.InterpolationMode = InterpolationMode.HighQualityBilinear;

                    PointF[] Star1 = Star.Calculate5StarPoints(new PointF(defaultTileSize / 2, defaultTileSize / 2), defaultTileSize / 3, defaultTileSize / 6);
                    SolidBrush FillBrush = new SolidBrush(System.Drawing.Color.Yellow);
                    g.FillPolygon(FillBrush, Star1);
                }
                else if (this.Occupied == GridSquare.Status.KingBlack)
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.FillEllipse(new HatchBrush(HatchStyle.SolidDiamond, System.Drawing.Color.Gray, System.Drawing.Color.Black), new Rectangle(Convert.ToInt32(defaultTileSize * .05), Convert.ToInt32(defaultTileSize * .05), Convert.ToInt32(defaultTileSize * .90), Convert.ToInt32(defaultTileSize * .90)));
                    //g.FillEllipse(System.Drawing.Brushes.White, new Rectangle(defaultTileSize / 4, defaultTileSize / 4, defaultTileSize / 2, defaultTileSize / 2));
                    g.InterpolationMode = InterpolationMode.HighQualityBilinear;

                    PointF[] Star1 = Star.Calculate5StarPoints(new PointF(defaultTileSize / 2, defaultTileSize / 2), defaultTileSize / 3, defaultTileSize / 6);
                    SolidBrush FillBrush = new SolidBrush(System.Drawing.Color.Yellow);
                    g.FillPolygon(FillBrush, Star1);
                }
                
            }
            
            BMP = disbmp;
            this.panel.Invalidate();
        }

        //the try move method is a major piece of code where all of the logic associated with moving is kept
        public void tryMove(GridPanel possibleNewPosition)
        {
            // you cant move to an occupied square
            if (this.Occupied != Status.Empty)
            {
                //make sure its your turn
                if ((new List<Status>() { Status.KingRed, Status.Red }.Contains(this.Occupied) && Checkers.form.theGame.ColorTurn != Game.Turn.RedsTurn))
                {
                     MessageBox.Show("You cannot move a red piece when its the blacks turn");
                }
                else if((new List<Status>() { Status.KingBlack, Status.Black }.Contains(this.Occupied) && Checkers.form.theGame.ColorTurn != Game.Turn.BlackTurn)){
                    MessageBox.Show("You cannot move a black piece when its the reds turn");
                }
                else{

                    //calculate all the posibilities of what you will be allowed to move to
                    List<Point> movablePoints = new List<Point>();

                    if (this.Occupied == Status.Red)
                    {
                        movablePoints.Add(new Point(this.Location.X - defaultTileSize, this.Location.Y - defaultTileSize));
                        movablePoints.Add(new Point(this.Location.X + defaultTileSize, this.Location.Y - defaultTileSize));
                    }
                    else if (this.Occupied == Status.Black)
                    {
                        movablePoints.Add(new Point(this.Location.X - defaultTileSize, this.Location.Y + defaultTileSize));
                        movablePoints.Add(new Point(this.Location.X + defaultTileSize, this.Location.Y + defaultTileSize));

                    }
                    else if (this.Occupied == Status.KingRed)
                    {
                        movablePoints.Add(new Point(this.Location.X - defaultTileSize, this.Location.Y - defaultTileSize));
                        movablePoints.Add(new Point(this.Location.X + defaultTileSize, this.Location.Y - defaultTileSize));
                        movablePoints.Add(new Point(this.Location.X - defaultTileSize, this.Location.Y + defaultTileSize));
                        movablePoints.Add(new Point(this.Location.X + defaultTileSize, this.Location.Y + defaultTileSize));

                    }
                    else if (this.Occupied == Status.KingBlack)
                    {
                        movablePoints.Add(new Point(this.Location.X - defaultTileSize, this.Location.Y - defaultTileSize));
                        movablePoints.Add(new Point(this.Location.X + defaultTileSize, this.Location.Y - defaultTileSize));
                        movablePoints.Add(new Point(this.Location.X - defaultTileSize, this.Location.Y + defaultTileSize));
                        movablePoints.Add(new Point(this.Location.X + defaultTileSize, this.Location.Y + defaultTileSize));
                    }

                    if (movablePoints.Contains(possibleNewPosition.Location))
                    {
                        //Point newPoint = 
                        if (this.Occupied == Status.Black && possibleNewPosition.Location.Y == defaultTileSize * (Checkers.gridSize - 1))
                        {
                            this.Occupied = Status.KingBlack;
                        }
                        else if (this.Occupied == Status.Red && possibleNewPosition.Location.Y == 0)
                        {
                            this.Occupied = Status.KingRed;
                        }
                        moveItem(possibleNewPosition.GridSquareOwner);
                    }
                    else
                    {
                        MessageBox.Show("sorry that is not a valid move");
                    }
                }
            }
            else
            {
                MessageBox.Show("You can only move a piece, not an empty square");
            }
            
        }

        public void moveItem(GridSquare newGS){
            //ensure that you are not jumping your own piece
            if ((new List<Status>() { Status.KingRed, Status.Red }.Contains(this.Occupied) && new List<Status>() { Status.KingRed, Status.Red }.Contains(newGS.Occupied)) ||
                (new List<Status>() { Status.KingBlack, Status.Black }.Contains(this.Occupied) && new List<Status>() { Status.KingBlack, Status.Black }.Contains(newGS.Occupied)))
            {
                MessageBox.Show("You cannot Jump your own items");
            }
            else
            {
                //change whos turn it will be
                Checkers.form.theGame.ColorTurn = this.Occupied == Status.Red || this.Occupied == Status.KingRed ? Game.Turn.BlackTurn : Game.Turn.RedsTurn;

                //make sure a red isnt landing on a red or a black isnt landing on a black
                if((new List<Status>() { Status.KingRed, Status.Red }.Contains(newGS.Occupied))){
                    if(newGS.Occupied == Status.KingRed)
                        Checkers.form.theGame.BlackPoints += 2;
                    else
                        Checkers.form.theGame.BlackPoints++;

                    doJump(newGS);
                }
                else if ((new List<Status>() { Status.KingBlack, Status.Black }.Contains(newGS.Occupied)))
                {
                    if (newGS.Occupied == Status.KingBlack)
                        Checkers.form.theGame.RedPoints += 2;
                    else
                        Checkers.form.theGame.RedPoints++;

                    doJump(newGS);
                }
                else
                {
                    newGS.Occupied = this.Occupied;
                    this.Occupied = Status.Empty;
                }

                newGS.Draw();
                this.Draw();

              
            }
        }
        
        public bool doJump(GridSquare newGS)
        {
            //place to object following the jump and change both the jumped and source square
            Point difference = new Point(this.Location.X - newGS.Location.X, this.Location.Y - newGS.Location.Y);
            Point newLocation = new Point(newGS.Location.X - difference.X, newGS.Location.Y - difference.Y);
            List<Control> allGS = Checkers.form.GetAll(Checkers.form, typeof(GridPanel)).ToList<Control>();
            GridSquare discoveredGS = ((GridPanel)allGS.Where(x => 
                (((GridPanel)x).GridSquareOwner).Location == newLocation).First()
                ).GridSquareOwner;
            if (discoveredGS.Occupied == Status.Empty)
            {
                //Form1.form.Controls.Remove(discoveredGS);
                discoveredGS.Occupied = getNewStatus(this.Occupied);
                this.Occupied = Status.Empty;
                newGS.Occupied = Status.Empty;

                //Form1.form.Controls.Add(discoveredGS);
                //check if landed on home, if so make king
                if (discoveredGS.Occupied == Status.Black && discoveredGS.Location.Y == defaultTileSize * (Checkers.gridSize - 1))
                {
                    discoveredGS.Occupied = Status.KingBlack;
                }
                else if (discoveredGS.Occupied == Status.Red && discoveredGS.Location.Y == 0)
                {
                    discoveredGS.Occupied = Status.KingRed;
                }
                discoveredGS.Draw();
                return true;
            }
            else
            {
                MessageBox.Show("You cannot jump a piece and land on another");
                return false;
            }
            
        }

        public Status getNewStatus(Status s)
        {
            //work around for issue with pass by reference
            if (s == Status.Red)
                return Status.Red;
            else if (s == Status.KingRed)
                return Status.KingRed;
            else if (s == Status.Black)
                return Status.Black;
            else if (s == Status.KingBlack)
                return Status.KingBlack;
            else
                return Status.Empty;
        }
        
    }
}
