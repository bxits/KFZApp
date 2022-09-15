using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Net;
using System.IO;
using System.Xml;
using System;
using System.Xml.Serialization;

namespace CommonTypes
{
    //TODO: Diese Klasse muss das Interface System.IEquatable implementieren,
    //um in Collections die generischen Funktionen nutzen zu können
    public class KFZ
    {
        public long Id;
        public string FahrgestNr;
        public string Kennzeichen;
        public int Leistung;
        public string Typ;


        public KFZ()
        {

        }

        public KFZ(KFZ kfz)
        {
            Id = kfz.Id;
            FahrgestNr = kfz.FahrgestNr;
            Kennzeichen = kfz.Kennzeichen;
            Leistung = kfz.Leistung;
            Typ = kfz.Typ;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} ({3}) {4}", this.Kennzeichen, this.Typ, Leistung, Id, FahrgestNr);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            KFZ objAsPart = obj as KFZ;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public bool Equals(KFZ k)
        {
            if (k.Id != this.Id) return false;
            return this.Id.Equals(k.Id);
        }
    }
}
