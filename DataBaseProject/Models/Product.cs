using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseProject.Models
{
    /// <summary>
    /// פעולה בונה של המחלקה מוצר
    /// </summary>
    public class Product
    {
        public int ProductSerialNumber { get; set; }//מספר סידורי של מוצר בחנות שניתן לקבל ולעדכן אותו
        public int Price { get; set; }//מחיר המוצר בחנות שניתן לקבל ולעדכן אותו
        /// <summary>
        /// פעולה שמחזירה משתנה סטרינגג שמראה את המספר סידורי של המוצר בחנות ואת מחירו
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "product id: " + ProductSerialNumber + "price " + Price;
        }



    }
}
