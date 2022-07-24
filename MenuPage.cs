using DataBaseProject.Models;
using FinalProjectV1.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FinalProjectV1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MenuPage : Page
    {
        private User user = null;//עצם מסוג משתמש
        /// <summary>
        /// הפעולה בונה שאחראית על ייצוג הדף ומציגה את הרכיבים על המסך
        /// </summary>
        public MenuPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// פעולה שמעבירה את המשתמש לדף העזרה
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HelpPage), this.user);
        }
        /// <summary>
        /// פעולה שבודקת האם תוכן הכפתור הוא התנתקות 
        /// ואם כן תנתק את המשתמש מהמשחק, אחרת זה כפתור התחברות שתפקידו להעביר את הדמות לדף ההתחברות
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if(LoginButton.Content.Equals("Logout"))
                Frame.Navigate(typeof(Logout));
            else
                Frame.Navigate(typeof(LoginPage));
        }

        /// <summary>
        /// הפעולה מעבירה את המשתמש לדף ההרשמה
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage));
        }

        /// <summary>
        /// הפעולה מעבירה את המשתמש לדף החנות
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Store_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StorePage), this.user);

        }

        /// <summary>
        /// הפעולה מעבירה את המשתמש לדף המשחק
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Game_Page(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GamePage), this.user);
        }

        /// <summary>
        /// הפעולה מעבירה את המשתמש לדף השיאים
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScoreBoardButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ScoreboardPage), this.user);
        }

        /// <summary>
        /// הפעולה מעבירה את המשתמש לדף האפשרויות
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OptionPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OptionPage), this.user);
        }

        /// <summary>
        /// הפעולה מעבירה את המשתמש לדף השחקן שלי
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPlayer_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MyPlayerPage), this.user);
        }
        /// <summary>
        /// הפעולה בודקת אם המשתמש קיים והוא לא ריק. אם התנאי מתקיים
        /// הפעולה מפעילה את כל הלחצנים שנמצאים בדף המסך הראשי 
        /// פרט ללחצן ההרשמה, ולחצן דף ההתחברות הופך לדף התנתקות. אם התנאי 
        /// לא מתקיים משמע המשתמש התנתק מהמשחק כל הלחצנים נכבים פרט ללחצן העזרה, ההתחברות והרשמה,
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
                // אם המשתמש קיים והוא לא ריק
            if (e.Parameter != null && e.Parameter.ToString() != "")
            {
                this.user = (User)e.Parameter;
                //-------------------הפעלת כל הלחצנים------------------//
                this.PlayButton.IsEnabled = true;
                this.OptionButton.IsEnabled = true;
                this.StoreButton.IsEnabled = true;
                this.ScoreBoardButton.IsEnabled = true;
                this.RegisterButton.IsEnabled = false;
                this.LoginButton.Content = "Logout";
                this.MyPlayer.IsEnabled = true;

            }
            else
            {
                this.user = null;
                //------------- כיבוי כל הלחצנים------------------//
                this.PlayButton.IsEnabled = false;
                this.OptionButton.IsEnabled = false;
                this.StoreButton.IsEnabled = false;
                this.ScoreBoardButton.IsEnabled = false;
                this.RegisterButton.IsEnabled = true; ;
                this.LoginButton.Content = "Login";
                this.MyPlayer.IsEnabled = false;

            }
        }
    }
}
