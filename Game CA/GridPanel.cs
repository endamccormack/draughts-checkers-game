using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_CA
{
    [Serializable]
    public class GridPanel : Panel
    {
        //this is the work around I used in order to be able to serialise GridSquare which used to inherit Panel the same 
        // way that this does
        public GridSquare GridSquareOwner { get; set; }
        public GridPanel(GridSquare GridSquareOwner)
        {
            this.GridSquareOwner = GridSquareOwner;
        }
        public GridPanel()
        {

        }
    }
}
