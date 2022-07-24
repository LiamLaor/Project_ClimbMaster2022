using DataBaseProject;
using DataBaseProject.Models;
using FinalProjectV1.Classes;
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

namespace FinalProjectV1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OptionPage : Page
    {
        public User user = null;//עצם מסוג משתמש
        /// <summary>
        /// הפעולה בונה שאחראית על ייצוג הדף ומציגה את הרכיבים על המסך
        /// </summary>
        public OptionPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// פעולה שמופעלת בעת טעינת הדף
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {


        }

        /// <summary>
        /// פעולה שמחזירה את המשתמש למסך הבית
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MenuPage),this.user);
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
        /// שינוי סיסמא- בלחיצה על כפתור השינוי 
        /// סיסמא מופעלת הפעולה הזאת שמקפיצה מודעה אשר מבקשת מהמשתמש את הסיסמה הקיימת
        /// וסיסמה חדשה. במידה והמשתמש הזין סיסמא שגויה
        /// שאמורה להיות הסיסמה שלו תקפוץ הודעה מתאימה. אם הכל עבד תקין 
        /// והתנאים התקיימו הפעולה תפנה לדאטא בייס ותשנה את הסיסמה לסיסמה החדשה ותשלח הודעה מתאימה לכך
        /// אם המשתמש הזין את אותה הסיסמה פעמיים תוקפץ הודעה מתאימה
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string userPassword;
            string userNewPassword;
            var Password = new TextBox
            {
                Width = 150,
                Height = 30,
                FontSize = 15,
                PlaceholderText = "Enter your old password"

            };
            var NewPassword = new TextBox
            {
                Width = 150,
                Height = 30,
                FontSize = 15,
                PlaceholderText = "Enter your new password"

            };
            var PassBlock = new TextBlock
            {
                Width = 300,
                Height = 30,
                FontSize = 20,
                Foreground = new SolidColorBrush(Colors.Red)
            };
            StackPanel panel = new StackPanel
            {
                Width = 200
            };
            panel.Orientation = Orientation.Vertical;
            panel.Children.Add(Password);
            panel.Children.Add(NewPassword);
            ContentDialog firstPopUp = new ContentDialog()
            {
                Title = "Enter the old password and the new password ",
                Content = panel,
                Background = new SolidColorBrush(Colors.LightGray),
                Width = 400,
                PrimaryButtonText = "Ok",
                SecondaryButtonText = "Cancel"
            };

            ContentDialog secondPopUp = new ContentDialog()
            {
                Title = "Your new password is: ",
                Content = PassBlock,
                Background = new SolidColorBrush(Colors.LightGray),
                Width = 200,
                PrimaryButtonText = "Ok"
            };
           
            var answer = await firstPopUp.ShowAsync();
            if (answer == ContentDialogResult.Primary)
            {
                TextBox CurrentPassword = (TextBox)((StackPanel)firstPopUp.Content).Children[0];
                TextBox newPassword = (TextBox)((StackPanel)firstPopUp.Content).Children[1];
                userPassword = CurrentPassword.Text;
                if(userPassword == user.Password)
                {
                    if (newPassword.Text.Equals(CurrentPassword.Text))
                    {
                        ((TextBlock)secondPopUp.Content).Text = "The passwords are the same";
                        (secondPopUp.Title) = "Alert";
                    }
                    else
                    {
                        userNewPassword = newPassword.Text;
                        this.user = DataBaseMethods.ChangePassword(this.user, userNewPassword);
                        ((TextBlock)secondPopUp.Content).Text = this.user.Password;
                    }            
                }
                else
                {
                    ((TextBlock)secondPopUp.Content).Text = "you entered wrong password!";
                    (secondPopUp.Title) = "Alert";
                }
               
                await secondPopUp.ShowAsync();
            }


        }

        
    }
}
