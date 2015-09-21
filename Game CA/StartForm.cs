using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;


namespace Game_CA
{
    //this is my mdi controller basically that handles all of the navigation through the software
    public partial class StartForm : Form
    {
        List<Game> games = new List<Game>();
        public StartForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //when the user clicks new create a checkerboard
            Checkers checkers = new Checkers();
            checkers.MdiParent = this;
            checkers.Show();
            LayoutMdi(MdiLayout.Cascade);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void horizontallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void verticallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help_Details helpDetails = new Help_Details();
            helpDetails.MdiParent = this;
            helpDetails.Show();

            Help help = new Help();
            help.MdiParent = this;
            help.Show();

            LayoutMdi(MdiLayout.TileVertical);
        }

        private void windowsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void iconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //here we save instances of all the draughts boards on screen, it gives the user a dialog box
            //in order to specify where they want to store the file
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Please indicate a file name";
            string fileName = "";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                fileName = sfd.FileName;
            }
            if (fileName != "")
            {
                foreach (Checkers chkr in this.MdiChildren)
                {
                    games.Add(chkr.theGame);
                }

                XmlSerializer xs = new XmlSerializer(typeof(List<Game>));

                using (Stream str = new FileStream(string.Format("{0}.xml", fileName), FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    xs.Serialize(str, games);
                }

                games.Clear();
                //CALL THE SERVICE
                //This feature turned out to be impossible on the custom windows 8 my VM has as it doesnt use users in the conventional way because it autologs in with
                //adaptive encryption everytime so I dont have credentials to install the service unfortunatly and I dont have a windows computer. According to Google anyway

                //get an instance of the GameCAService
                //ServiceController serviceController = new ServiceController("GameCAService");

                //send a custom command to it, any int could be in the brackets for this particular example
                //serviceController.ExecuteCommand(1);
            }


        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //here we load the file that was perviously saved back into the program using a dialog box for the user
            //to indicate what file to load and then to load it
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Please indicate a file to load";
            string fileName = "";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileName = ofd.FileName;
            }
            if (fileName != "")
            {

                using (var stream = File.OpenRead(fileName))
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(List<Game>));
                    TextReader textReader = new StreamReader(stream);
                    games = (List<Game>)deserializer.Deserialize(textReader);
                    textReader.Close();
                }

                foreach (Game g in games)
                {
                    Checkers checkers = new Checkers(g);
                    checkers.MdiParent = this;
                    checkers.Show();
                    LayoutMdi(MdiLayout.Cascade);
                }
               
            }
        }

        private void StartForm_Load(object sender, EventArgs e)
        {
            Welcome w = new Welcome();
            w.MdiParent = this;
            w.Show();

        }

    }
}
