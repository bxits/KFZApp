using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;

using CommonTypes;
using DataAccess;

namespace BusinessLogic.Models
{
    public class KFZCollectionModel
    {
        //Event nach außen publizieren.
        public event KFZDataArrivedEventHandler KFZDataArrived;
        public event KFZChangedEventHandler KFZChanged;
        public event KFZDeletedEventHandler KFZDeleted;
        public event KFZNewEventHandler KFZNew;

        public List<KFZ> KFZListe = new List<KFZ>();
        private BackgroundWorker _bwKFZ;


        public KFZCollectionModel()
        {
            //Tür auf! Für das Event registrieren.
            Connection.KfzListeReady += Connection_KfzListeReady;
            AutoRefreshDisabled = true;
        }

        public bool AutoRefreshDisabled { get; private set; }

        public void StartAutoRefresh()
        {

            AutoRefreshDisabled = false;
        }


        private void Connection_KfzListeReady(List<KFZ> kfzs)
        {
            KFZListe = kfzs;

            KFZDataArrived(KFZListe);
        }

        public void GetAllKfz()
        {
            //Alle KFZ aus der Datenquelle abholen und in der Liste speichern:

            //jetzt über Events...
            //this.KFZListe = Connection.GetKfzList();
            Connection.GetKfzList();
        }

        public void Insert(KFZ kfz)
        {
            //Überprüfen, ob das neue Kfz korrekte Werte besitzt.
            if (kfz.Id == -1 &&
                kfz.FahrgestNr != string.Empty &&
                kfz.Kennzeichen != string.Empty &&
                kfz.Leistung > 0
                && kfz.Typ != string.Empty)
            {
                Connection.InsertKFZ(kfz);
            }
        }

        public void Update(KFZ kfz)
        {
            Connection.UpdateKFZ(kfz);
        }

        public void Delete(KFZ kfz)
        {
            //1. KFZ abmelden bei Zulassungsstelle
            //2. Fahrzeughalter benachrichtigen, dass KFZ abgemeldet wurde.
            //3. auf Bestätigung des Halters warten
            //4. usw.
            Connection.DeleteKFZ(kfz);
        }

        //private void CheckForNewKFZ()
        public void RefreshKFZData()
        {
            //Künstlicher Delay
            System.Threading.Thread.Sleep(15000);


            List<KFZ> tmp = Connection.GetKfzList();
            List<KFZ> kfznichtmehrdrin = new List<KFZ>();

            foreach (KFZ k in tmp)
            {
                if (!this.KFZListe.Contains(k))
                {
                    KFZListe.Add(k);
                    if (KFZNew != null)
                        KFZNew(k);
                }
            }

            foreach (KFZ k in this.KFZListe)
            {
                if (!tmp.Contains(k))
                {
                    if (KFZDeleted != null)
                        KFZDeleted(k);
                }
            }

            foreach (KFZ k in this.KFZListe)
            {
                if (tmp.Contains(k))
                {
                    bool sendChangedEvent = false;

                    int i = tmp.IndexOf(k);

                    if (k.Typ != tmp[i].Typ)
                    {
                        k.Typ = tmp[i].Typ;
                        sendChangedEvent = true;
                    }

                    if (k.Leistung != tmp[i].Leistung)
                    {
                        k.Leistung = tmp[i].Leistung;
                        sendChangedEvent = true;
                    }

                    if (k.Kennzeichen != tmp[i].Kennzeichen)
                    {
                        k.Kennzeichen = tmp[i].Kennzeichen;
                        sendChangedEvent = true;
                    }

                    if (k.FahrgestNr != tmp[i].FahrgestNr)
                    {
                        k.FahrgestNr = tmp[i].FahrgestNr;
                        sendChangedEvent = true;
                    }

                    //nur wenn eine Änderung gemacht wurde, wird ein ChangeEvent gefeuert.
                    if (sendChangedEvent == true)
                    {
                        if (KFZChanged != null)
                            KFZChanged(k);
                    }

                }
            }
            

        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Delegation of this private event to possible consumers outside of this object.
        }
    }
}
