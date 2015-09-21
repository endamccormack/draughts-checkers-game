using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Game_CA
{
    public partial class Checkers : Form
    {
        /*
         * The game is completely dynamic, you can change grid sizes etc on the fly, I have tried to cater for all of this as its good practice
         * but adds complexity, draughts is draughts thought so I hard coded in the standard board side that we were used to playing with, I called
         * it checkers because I keep mispelling draughts. As you will see I went for a horribly complicated serialisation technique, it started with the
         * idea that I wanted the user to be able to edit the xml files in order to set up games the way they want to, putting pieces whereever they wanted 
         * for training purposes, using XML serialisation was made extremely complex by the way I originally coded it, I use inheritance a lot in coding 
         * which does not work well for serialisation so I switched my architecture technique to encapsulate instead of inherit components which took a long long time
         * the entire architecture of the way I built the game had to be rebuilt which basically made it like I created 2 games. I am very stubborn, using binary would
         * have made my serialisation a lot easier.
         * 
         * The choice of game too was perhaps a bad one for me as a person, there is a lot of logic in draughts which I had to code, this made for a complex game logic
         * as you will see in GridSquares. GridPanel on the other hand is how I encapsulated the GridSquares idea without using inheritance in order to be able to serialise
         * the GridSquares for storage.
         */
        public static Checkers form;

        public Panel[,] draftsBoardPanels;
        public static int gridSize = 8;
        public Game theGame;
        public Game Game { get { return theGame; } set { theGame = value; } }

        //the constructor used for load
        public Checkers(Game loadGame)
        {
            form = this;
            theGame = loadGame;
            theGame.form = this;
            theGame.startWithGame();
            InitializeComponent();
        }

        //the default constructor
        public Checkers()
        {
            form = this;
            theGame = new Game(form);
            theGame.start();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        //get all the controls on screen for a certain type
        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        //serialise game that was just used for testing
        public void serializeGame(){
            XmlSerializer xs = new XmlSerializer(typeof(Game));

            using (Stream str = new FileStream("Game.xml", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                xs.Serialize(str, theGame);
            }
        }
    }   
}
