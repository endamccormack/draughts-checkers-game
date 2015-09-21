using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Game_CA
{
    public partial class Help : Form
    {
        //interestingly I decided to store everything in json to make it more manageable for myself 
        // this code reads the json file and populates a list box and allows you to drag an item in
        //the list to drop somewhere else
        List<HelpItem> helpItems = new List<HelpItem>();
        public Help()
        {
            InitializeComponent();
            String json = System.IO.File.ReadAllText("help.json");

            helpItems = JsonConvert.DeserializeObject<List<HelpItem>>(json);

            listBox1.DataSource = helpItems;
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (listBox1.Items.Count == 0)
                return;
            //get the origin
            ListBox parent = (ListBox)sender;
      
            //get the data for the origin
            //object data = GetDataFromListBox(dragSource1, e.GetPosition(parent));

            HelpItem hi = null;
            try
            {
                hi = (HelpItem)parent.Items[listBox1.IndexFromPoint(e.X, e.Y)];
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //DragDropEffects dde1 = DoDragDrop(s,
            //    DragDropEffects.All);

            if (hi != null)
            {
                System.Windows.Forms.DragDropEffects dde1 = DoDragDrop(hi,System.Windows.Forms.DragDropEffects.Copy);
                //pass that onto another method to move it
                //DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
            }
        }


        
    }
}
