using DataBaseProject;
using DataBaseProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class RegisterPage : Page
    {
        private User user;//עצם מסוג משתמש
         /// <summary>
        /// הפעולה בונה שאחראית על ייצוג הדף ומציגה את הרכיבים על המסך
        /// </summary>
        public RegisterPage()
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
        /// פעולה שעובדת כאשר המשתמש לחץ על כפתור 
        /// ההרשמה בדף זה. הפעולה ראשית בודקת שהשדות לא ריקים, לאחר מכן הפעולה 
        /// בודקת שהסיסמאות שהוזנו זהות כי בהרשמה המשתמש צריך לחזור על הסיסמה 
        /// פעמיים. בנוסף לאחר בדיקה זו הפעולה בודקת שלא קיים משתמש כזה בטבלאת 
        /// משתמשים. אם לא קיים הפעולה מוסיפה את המשתמש לטבלת משתמשים והמשתמש 
        /// מתחבר למשחק באופן אוטומטי ומועבר לדף הראשי. במידה והמשתמש קיים 
        /// תקפוץ הודעה מתאימה לכך, וגם במידה והסיסמאות זהות תקפוץ הודעה 
        /// מתאימה לכך וגם במצב שחסר אחד מהפרטים בהרשמה תקפוץ הודעה מתאימה.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (SignUpName.Text != "" && SignUpPasswordName.Password != "" && SignUpPasswordNameSecond.Password != "" && SignUpEmailName.Text != "")
            {
                if (SignUpPasswordName.Password.Equals(SignUpPasswordNameSecond.Password) == true)
                {
                    if(SignUpEmailName.Text.IndexOf('@')<0)

                    this.user = DataBaseMethods.AddUser(SignUpName.Text, SignUpPasswordName.Password, SignUpEmailName.Text);
                    if (this.user != null)
                        Frame.Navigate(typeof(MenuPage), this.user);
                    else
                    {
                        var dialog = new MessageDialog("The user already exsit. You have to identify");
                        dialog.Title = "System notice";
                        dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                        await dialog.ShowAsync();
                    }
                }
                else
                {
                    var dialog = new MessageDialog("Passwords are not the same!");
                    dialog.Title = "System notice";
                    dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                    await dialog.ShowAsync();
                }
            }
            else
            {
                var dialog = new MessageDialog("One of data is empty!");
                dialog.Title = "System notice";
                dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                await dialog.ShowAsync();
            }
        }
    }
}
