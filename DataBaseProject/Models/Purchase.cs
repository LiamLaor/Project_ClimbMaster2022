using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseProject.Models
{
    public class Purchase
    {
        public int UserId { get; set; }//   משתנה של המספר זהות של המשתמש באתר אשר ניתן לקבל ולעדכן אותה
        public int ProductSetialNumber { get; set; }//מספר סידורי של מוצר בחנות שניתן לקבל ולעדכן אותו
       /// <summary>
       /// פעולה שמחזיקה משתנה סטרינג של מספר זהות המשתמש באתר ומספר הסידורי של המוצר אותו הוא קנה
       /// </summary>
       /// <returns></returns>
        public override string ToString()
        {
            return "User Id: " + UserId + "Serial of Product: " + ProductSetialNumber;
        }
    }
}
