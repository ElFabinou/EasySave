using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using easysave.Models;

namespace easysave.ViewModels
{
    public class SocketViewModel
    {

        public SocketViewModel() {
            //SocketModel socketModel = new SocketModel();
            SocketModel socketModel = SocketModel.GetInstance();
        }

    }
}
