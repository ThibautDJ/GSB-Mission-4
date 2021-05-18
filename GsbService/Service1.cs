using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using MySql.Data.MySqlClient;
using Gsb4;

namespace GsbService
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer = new Timer();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 86400000;
            timer.Enabled = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            GestionDate date = new GestionDate(); // on instancie un objet type GestionDate
            String ajd = date.dateJour(); // on récupere de la date du jour sous format "dd/mm/yyyy"
            int a = int.Parse(ajd.Substring(0, 2));  // on récupere juste le jour le "dd" et on le convertie en int

            if (a <= 10) // vérifications que c'est bien compris entre 1 et 10 inclus
            {
                etatCloturer(date);
            }
            else if (a >= 20)
            {
                etatValider(date);
            }
        }
        protected override void OnStop()
        {
        }

        public void etatCloturer(GestionDate date)
        {

            try
            {


                ConnexionSql maConnexion = ConnexionSql.getInstance("localHost", "gsb-v1", "root", ""); // connexion à la BDD (cf ConnexionSql.cs)

                maConnexion.openConnection(); // ouverture de la connexion 

                //on modifie l'état des fiches de frais à l'état 'CR' en 'CL' que pour les fiches du mois précedent.
                MySqlCommand oCom1 = maConnexion.reqExec("Update testfichefrais set idEtat = 'CL' where idEtat ='CR' and mois =" + date.moisPrecedent());


                maConnexion.closeConnection();


            }
            catch (Exception emp)
            {
                throw (emp);
            }
        }

        public void etatValider(GestionDate date)
        {

            try
            {
                ConnexionSql maConnexion = ConnexionSql.getInstance("localHost", "gsb-v1", "root", ""); // connexion à la BDD (cf ConnexionSql.cs)

                maConnexion.openConnection(); // ouverture de la connexion 

                //on modifie l'état des fiches de frais à l'état 'RB' en 'VA' que pour les fiches du mois précedent.

                MySqlCommand oCom = maConnexion.reqExec("Update testfichefrais set idEtat = 'VA' where idEtat ='RB' and mois =" + date.moisPrecedent());
                oCom.ExecuteNonQuery();

                maConnexion.closeConnection(); // fermeture de la connexion
            }
            catch (Exception emp)
            {
                throw (emp);
            }
        }
    }
}

