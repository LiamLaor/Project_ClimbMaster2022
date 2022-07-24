using DataBaseProject;
using DataBaseProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
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
    public sealed partial class LoginPage : Page
    {
        private User user;//עצם מסוג משתמש
        /// <summary>
        /// הפעולה בונה שאחראית על ייצוג הדף ומציגה את הרכיבים על המסך
        /// </summary>
        public LoginPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// פעולת חזרה למסך הראשי כאשר המשתמש אינו מחובר
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MenuPage));

        }


        /// <summary>
        /// פעולה שבודקת האם אכן קיים משתמש כפי שהוזן , אם כן היא תעביר את המשתמש למסך הראשי 
        /// כאשר הוא מחובר, אחרתתוצג הודעה קופצת שתרשום כי המשתמש אינו קיים
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            this.user = DataBaseMethods.GetUser(SignInName.Text, SignInPasswordName.Password);
            if (this.user != null)
                Frame.Navigate(typeof(MenuPage), this.user);
            else
            {//הצגת הודעה קופצת
                var dialog = new MessageDialog("user does not exist. you must first register");
                dialog.Title = "system notice";
                dialog.Commands.Add(new UICommand { Label = "ok", Id = 0 });
                await dialog.ShowAsync();
            }
        }


        /// <summary>
        /// פעולה של שכחתי סיסמא. מטרת הפעולה היא לבדוק האם המשתמש
        /// . אכן קיים באמצעות בדיקת השדות של שם המשתמש והמייל שלו.
        /// אם אכן קיים משתמש כזה הפעולה תציג למשתמש על המסך את סיסמתו מודגשת באדום.
        /// אם לא קיים משתמש כזה הפעולה תציג הודעה שהכנסת מידע שגוי
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string userName;
            string userMail;
            var nameBox = new TextBox
            {
                Width = 150,
                Height = 30,
                FontSize = 15
            };
            var mailBox = new TextBox
            {
                Width = 150,
                Height = 30,
                FontSize = 15
            };
            var passBlock = new TextBlock
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
            panel.Children.Add(nameBox);
            panel.Children.Add(mailBox);
            ContentDialog firstPopUp = new ContentDialog()
            {
                Title = "Enter your userName and your mail: ",
                Content = panel,
                Background = new SolidColorBrush(Colors.LightGray),
                Width = 400,
                PrimaryButtonText = "Ok",
                SecondaryButtonText = "Cancel"
            };
            ContentDialog secondPopUp = new ContentDialog()
            {
                Title = "Your password is: ",
                Content = passBlock,
                Background = new SolidColorBrush(Colors.LightGray),
                Width = 200,
                PrimaryButtonText = "Ok"
            };
            var answer = await firstPopUp.ShowAsync();
            if (answer == ContentDialogResult.Primary)
            {
                TextBox nameText = (TextBox)((StackPanel)firstPopUp.Content).Children[0];
                TextBox mailText = (TextBox)((StackPanel)firstPopUp.Content).Children[1];
                userName = nameText.Text;
                userMail = mailText.Text;
                User user = DataBaseMethods.GetUserForgotPassword(userName, userMail);
                if (user != null)
                    ((TextBlock)secondPopUp.Content).Text = user.Password;
                else
                    ((TextBlock)secondPopUp.Content).Text = "The data you entered is incorrect";
                await secondPopUp.ShowAsync();
            }
        }
    }
}
    

