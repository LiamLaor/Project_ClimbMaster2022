using DataBaseProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FinalProjectV1.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScoreboardPage : Page
    {
        public User user = null;//עצם מסוג משתמש
        private List<User> Users;// רשימה של משתמשים מסוג משתמש
        /// <summary>
        /// הפעולה בונה שאחראית על ייצוג הדף ומציגה את הרכיבים על המסך
        /// </summary>
        public ScoreboardPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// פעולה שמתחרשת כאשר השחקן לחץ על כפתור החזרה הביתה.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MenuPage), this.user);
        }

        /// <summary>
        ///בuser  e בפעולה שבודקת האם אכן קיים משתמש במשחק ואם כן היא משימה את הפרמטר
        /// </summary>
        /// <param name="e">הפרמטר שאותו בודקים אם הוא ריק או לא, במידה ולא ריק הוא המשתמש</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (e.Parameter != null && e.Parameter.ToString() != "")
            {
                this.user = (User)e.Parameter;
            }
        }
        /// <summary>
        /// פעולה סטטית שמטרתה לקבל לרשימה את רשימת השחקנים במשחק כאשר הם מסודרים
        /// לפי הניקוד המקסימלי שלהם ולהציג בתיבות הטקסט המתאימות את פרטי השחקנים שצברו הכי הרבה נקודות
        /// </summary>
        private void TopUsers()
        {
            Users = DataBaseProject.DataBaseMethods.GetUsersSortMaxScore();//השמת רשימת המשתמשים ברשימה חדשה
            if (Users.Count >= 3)//בדיקה אם יש ברשימה מעל 3 שחקנים אז שייקח את 3 השחקנים האחרונים ברשימה
            {
                NamePlace1.Text = Users[(Users.Count-1)].UserName.ToString();//השמת השם של מקום אחרון ברשימה במקום הראשון בטבלת השיאים  מכיוון שהרשימה מסודרת שבסוף הרשימה נמצא השחקן עם ההכי הרבה נקודת
                NamePlace2.Text = Users[(Users.Count - 2)].UserName.ToString();//השמת השם של השחקן עם המספר השני הכי גבוה של נקודות
                NamePlace3.Text = Users[(Users.Count - 3)].UserName.ToString();//השמת השם של השחקן עם המספר נקודות השלישי הכי גבוה
                ScoreHighPlace1.Text = Users[(Users.Count - 1)].MaxScore.ToString();//השמת מספר הנקודות של השחקן במקום האחרון ברשימה המסודרת כלומר בעל מספר הנקודות הגבוה ביותר
                ScoreHighPlace2.Text = Users[(Users.Count - 2)].MaxScore.ToString();//השמת מספר הנקודות של השחקן של מקום שני
                ScoreHighPlace3.Text = Users[(Users.Count - 3)].MaxScore.ToString();//השמת מספר הנקודות של מקום שלישי
            }
            else if(Users.Count == 2)//בדיקה של האם יש רק 2 שחקנים קיימים במשחק אז הוא ישים את שניהם ובמקום השלישי לא יהי  אף שחקן
            {
                NamePlace1.Text = Users[(Users.Count - 1)].UserName.ToString();//השמת השם של מקום אחרון ברשימה במקום הראשון בטבלת השיאים  מכיוון שהרשימה מסודרת שבסוף הרשימה נמצא השחקן עם ההכי הרבה נקודת
                NamePlace2.Text = Users[(Users.Count - 2)].UserName.ToString();//השמת השם של השחקן עם המספר השני הכי גבוה של נקודות
                ScoreHighPlace1.Text = Users[(Users.Count - 1)].MaxScore.ToString();//השמת מספר הנקודות של השחקן במקום האחרון ברשימה המסודרת כלומר בעל מספר הנקודות הגבוה ביותר
                ScoreHighPlace2.Text = Users[(Users.Count - 2)].MaxScore.ToString();//השמת מספר הנקודות של השחקן של מקום שני
            }
            else if(Users.Count == 1)//בדיקה אם קיים רק שחקן אחד במשחק שנרשם והוא ברשימה ובמצב כזה רק הוא יופיע בטבלה במקום הראשון
            {
                NamePlace1.Text = Users[(Users.Count - 1)].UserName.ToString();//השמת השם של מקום אחרון ברשימה במקום הראשון בטבלת השיאים  מכיוון שהרשימה מסודרת שבסוף הרשימה נמצא השחקן עם ההכי הרבה נקודת
                ScoreHighPlace1.Text = Users[(Users.Count - 1)].MaxScore.ToString();//השמת מספר הנקודות של השחקן במקום האחרון ברשימה המסודרת כלומר בעל מספר הנקודות הגבוה ביותר
            }
        }
        /// <summary>
        /// פעולה שמופעלת בעת טעינת הדף
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.user != null)
            {
                TopUsers();//הפעלת הפעולה שמדפיסה במקומות המתאימים את טבלת השיאים
            }
        }
    } 
        


    
}
