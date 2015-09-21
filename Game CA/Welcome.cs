using Newtonsoft.Json;
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
    public partial class Welcome : Form
    {
        // this is my welcome page again populated by json which essentially explains why I chose to
        //undertake this particular game.
        Content marcellineContent;
        public Welcome()
        {
            InitializeComponent();
        }

        private void Welcome_Load(object sender, EventArgs e)
        {
            String json = System.IO.File.ReadAllText("description.json");

            marcellineContent = JsonConvert.DeserializeObject<Content>(json);

            lblDescription.Text = marcellineContent.Contents;
        }
    }
}
