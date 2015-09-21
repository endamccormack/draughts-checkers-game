using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_CA
{
    //the basic outline of the HelpItem JSon object
    class HelpItem
    {
        public String Title { get; set; }
        public String Contents { get; set; }
        public HelpItem()
        {

        }

        public HelpItem(String Title, String Contents)
        {
            this.Title = Title;
            this.Contents = Contents;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
