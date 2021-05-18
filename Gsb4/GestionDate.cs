using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsb4
{
    public class GestionDate
    {
        /// <summary>
        /// Recupération de la date du jour 
        /// </summary>
        /// <returns>la date du jour sous format "dd/mm/yyyy"</returns>
        public String dateJour()
        {
            DateTime ajd = DateTime.Now;
            String asString = ajd.ToString("dd/MM/yyyy");
            return asString; 
        }
        /// <summary>
        /// Récuperation du mois précedent
        /// </summary>
        /// <returns>mois précédent sous format "yyyyMM" en rapport à la BDD</returns>
        public String moisPrecedent()
        {
            DateTime ajd = DateTime.Now;
            ajd = ajd.AddMonths(-1);
            String asString = ajd.ToString("yyyyMM");

            return asString;
        }
    }
}
