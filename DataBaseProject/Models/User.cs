using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseProject.Models
{
    /// <summary>
    /// מחלקה של משתמש
    /// </summary>
    public class User
    {
        public int Id { get; set; }//המזהה הייחודי של המשתמש
        public string UserName { get; set; }//שם המשתמש
        public string Password { get; set; }//הסיסמה
        public int CurrentCharacter { get; set; }//מספר הדמות, איתה משחק השחקן
        public int Coins { get; set; }//הכסף המצטבר
        public int MaxScore { get; set; }//הישג
        public string Mail { get; set; }//כתובת אלקטרונית
        public int CurrentBackground { get; set; }//הרקע הנוכחי של הדמות

    }
}
