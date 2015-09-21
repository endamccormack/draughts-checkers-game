using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_CA
{
    public partial class Help_Details : Form
    {
        //fairly simple, this is just a reiever for the information passed via drag and drop
        public Help_Details()
        {
            InitializeComponent();
        }

        private void Help_Details_DragDrop(object sender, DragEventArgs e)
        {
            HelpItem hi = null;
            if (e.Data.GetDataPresent(typeof(HelpItem)))
            {
                hi = (HelpItem)e.Data.GetData(typeof(HelpItem));
                lblTitle.Text = hi.Title;
                lblDescription.Text = hi.Contents;
            }

            
        }

        private void Help_Details_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
    }
}
