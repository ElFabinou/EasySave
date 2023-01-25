using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easysave
{
    public class Save
    {
        private readonly DateTime _created;
        private readonly string _name;
        private readonly string _description;

        public Save(DateTime _created, string _name, string _description)
        {
            this._created = _created;
            this._name = _name;
            this._description = _description;

        }

        public string displaySaveInformation()
        {
            return ("La sauvegarde "+this._name+" été créée à cette date : "+this._created+"\nDescription : "+this._description);
        }

    }
}
