using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace Gsb4
{
    public partial class Form1 : Form
    {
        private string provider = "localhost";
        private string dataBase = "gsb-v1";
        private string Uid = "root";
        private string mdp = "";

        private ConnexionSql maConnexion;

        MySqlCommand oCom;

       
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        // Initialisation du Timer

        /// <summary>
        /// Initialisation du timer toute les 10s
        /// </summary>
        private void InitializeTimer()
        {
            timer1.Interval = 10000; // intervale du timer toutes les 10 secondes
            timer1.Tick += new EventHandler(timer1_Tick);
        }

        /// <summary>
        /// Le timer vas lancer des requêtes SQL pour modifier les tables 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            try
            {
                timer1.Start();
                
                GestionDate date = new GestionDate(); // on instancie un objet type GestionDate
                String ajd = date.dateJour(); // on récupere de la date du jour sous format "dd/mm/yyyy"
                int a = int.Parse(ajd.Substring(0, 2));  // on récupere juste le jour le "dd" et on le convertie en int
               
                maConnexion = ConnexionSql.getInstance(provider, dataBase, Uid, mdp); // connexion à la BDD (cf ConnexionSql.cs)

                
                if (a <= 10) // vérifications que c'est bien compris entre 1 et 10 inclus
                {

                    maConnexion.openConnection(); // ouverture de la connexion 

                    DataTable dt = new DataTable();
                    //on modifie l'état des fiches de frais à l'état 'CR' en 'CL' que pour les fiches du mois précedent.
                    MySqlCommand oCom1 = maConnexion.reqExec("Update testfichefrais set idEtat = 'CL' where idEtat ='CR' and mois =" + date.moisPrecedent());
                    oCom1.ExecuteNonQuery();  //Excecution de la requête

                    oCom = maConnexion.reqExec("Select * from testfichefrais where mois =" + date.moisPrecedent()); 
                    dt.Load(oCom.ExecuteReader());

                    dataGridView1.DataSource = dt;
                    maConnexion.closeConnection();
                }

                // on refait la même chose mais cette fois la tache commence à partir du 20
                // pour les fiches en état 'RB' qui passe à 'VA'
                

                if (a >= 20)
                {
                    maConnexion.openConnection();

                    DataTable dt = new DataTable();
                    MySqlCommand oCom1 = maConnexion.reqExec("Update testfichefrais set idEtat = 'RB' where idEtat ='VA' and mois =" + date.moisPrecedent());
                    oCom1.ExecuteNonQuery();

                    oCom = maConnexion.reqExec("Select * from testfichefrais where mois =" + date.moisPrecedent());
                    dt.Load(oCom.ExecuteReader());

                    dataGridView1.DataSource = dt;
                    maConnexion.closeConnection();
                    
                }
                timer1.Stop();
            }
            catch (Exception emp)
            {
                MessageBox.Show(emp.Message);
            }

        }
    }
}
