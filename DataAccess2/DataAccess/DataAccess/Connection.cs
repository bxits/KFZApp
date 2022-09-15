using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using CommonTypes;

namespace DataAccess
{
    public static class Connection
    {
        internal static string filename = "kfz.csv";
        static Connection()
        {

         
        }

        public static List<KFZ> GetKfzList()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory + "\\";

            List<KFZ> KFZListe = new List<KFZ>();

            using (var reader = new StreamReader(dir + filename))
            {

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    KFZListe.Add(new DA_KFZ(line));
                }
            }
            return KFZListe;
        }

        public static void InsertKFZ(KFZ kfz)
        {
            DA_KFZ k = new DA_KFZ(kfz);
            k.Insert();
        }
    }
}
