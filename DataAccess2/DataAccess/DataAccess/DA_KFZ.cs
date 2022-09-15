using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using CommonTypes;

namespace DataAccess
{
    public class DA_KFZ : KFZ
    {

        public DA_KFZ():base()
        {

        }

        public DA_KFZ(KFZ kfz) : base()
        {
            this.Id = kfz.Id;
            this.FahrgestNr = kfz.FahrgestNr;
            this.Kennzeichen = kfz.Kennzeichen;
            this.Leistung = kfz.Leistung;
            this.Typ = kfz.Typ;
        }

        internal DA_KFZ(string csvline)
        {
            string[]values = csvline.Split(';');

            this.Id = long.Parse(values[0]);
            this.FahrgestNr = values[1];
            this.Kennzeichen = values[2];
            this.Leistung = int.Parse(values[3]);
            this.Typ = values[4];
        }

        internal void Insert()
        {
            //TODO: 4) Insert-SQL zum Einfügen der Instanz in die Datenbank

            this.Id = 88;

            string csv = string.Format(@"{0};{1};{2};{3};{4}", this.Id,
                this.FahrgestNr, this.Kennzeichen, this.Leistung, this.Typ);

            var dir = AppDomain.CurrentDomain.BaseDirectory + "\\";

            List<KFZ> KFZListe = new List<KFZ>();

            using (var writer = new StreamWriter(dir + Connection.filename, true))
            {
                writer.Write(Environment.NewLine + csv);
            }

        }
    }
}
