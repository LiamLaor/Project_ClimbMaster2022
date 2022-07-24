using DataBaseProject;
using DataBaseProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

namespace FinalProjectV1.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyPlayerPage : Page
    {
        public User user = null;//עצם מסוג משתמש
        /// <summary>
        /// הפעולה בונה שאחראית על ייצוג הדף ומציגה את הרכיבים על המסך
        /// </summary>
        public MyPlayerPage()
        {
            this.InitializeComponent();
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
        /// . פעולה שעובדת כאשר המסך של הדף נפתח, ומטרתה לטפל בכפתורים שמאפשרים להשתמש
        /// בדמויות ורקעים במשחק. הפעולה בודקת עם תכונות המשתמש באיזה דמות נוכחית ורקע נוכחי 
        /// משתמש ו בהתאם היא משנה את תוכן הכפתורים של הדמויות והרקעים
        /// כמו כן, הפעולה מפעילה פעולה אחרת שבודקת האם קיים בטבלה למשתמש הנוכחי דמות מסויימת ורקע מסויים ובהתאם מפעילה את כפתור השימוש בה
        /// בנוסף הפעולה מציגה על המסך את הכסף הנוכחי שיש לשחקן
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<Purchase> purchases;
            if (this.user != null)
            {
                CoinText.Text = "Coins: " + user.Coins.ToString();

                purchases = DataBaseMethods.GetPurchases(this.user.Id);
                if (IsExist(2, purchases, this.user.Id))
                {
                    UseSanta.IsEnabled = true;
                    if (user.CurrentCharacter == 2)
                    {
                        UseDino.Content = "Use";
                        UseRobot.Content = "Use";
                        UseSanta.Content = "Used";
                        UseChild.Content = "Use";

                    }
                }
                else
                    UseSanta.IsEnabled = false;
                if (IsExist(3, purchases, this.user.Id))
                {
                    UseRobot.IsEnabled = true;
                    if (user.CurrentCharacter == 3)
                    {
                        UseDino.Content = "Use";
                        UseRobot.Content = "Used";
                        UseSanta.Content = "Use";
                        UseChild.Content = "Use";
                    }
                }
                else
                    UseRobot.IsEnabled = false;
                if (IsExist(4, purchases, this.user.Id))
                {
                    UseDino.IsEnabled = true;
                    if (user.CurrentCharacter == 4)
                    {
                        UseDino.Content = "Used";
                        UseRobot.Content = "Use";
                        UseSanta.Content = "Use";
                        UseChild.Content = "Use";
                    }
                }
                else
                    UseDino.IsEnabled = false;

                if (user.CurrentCharacter == 1)
                {
                    UseDino.Content = "Use";
                    UseRobot.Content = "Use";
                    UseSanta.Content = "Use";
                    UseChild.Content = "Used";
                }

                if (IsExist(5, purchases, this.user.Id))
                {
                    UseBlueBackground.IsEnabled = true;
                    if (user.CurrentBackground == 2)
                    {
                        UseBlackBackground.Content = "Use";
                        UsePurpleBackground.Content = "Use";
                        UseBlueBackground.Content = "Used";
                        UseRedBackground.Content = "Use";
                    }
                }
                else
                {
                    UseBlueBackground.IsEnabled = false;
                }

                if (IsExist(6, purchases, this.user.Id))
                {
                    UsePurpleBackground.IsEnabled = true;
                    if (user.CurrentBackground == 3)
                    {
                        UseBlackBackground.Content = "Use";
                        UsePurpleBackground.Content = "Used";
                        UseBlueBackground.Content = "Use";
                        UseRedBackground.Content = "Use";
                    }
                }
                else
                {
                    UsePurpleBackground.IsEnabled = false;
                }

                if (IsExist(7, purchases, this.user.Id))
                {
                    UseRedBackground.IsEnabled = true;
                    if (user.CurrentBackground == 4)
                    {
                        UseBlackBackground.Content = "Use";
                        UsePurpleBackground.Content = "Use";
                        UseBlueBackground.Content = "Use";
                        UseRedBackground.Content = "Used";
                    }
                }
                else
                {
                    UseRedBackground.IsEnabled = false;
                }
                if (this.user.CurrentBackground == 1)
                {
                    UseBlackBackground.Content = "Used";
                    UsePurpleBackground.Content = "Use";
                    UseBlueBackground.Content = "Use";
                    UseRedBackground.Content = "Use";
                }



            }
        }

        /// <summary>
        /// פעולה שמקבלת רשימת מוצרים שקנו המשתמשים במשחק, 
        /// מספר המוצר הנוכחי שרוצים לבדוק ותעודת זהות השחקן. 
        ///false אחרת תחזיר  true הפעולה בודקת מתוך כל הרשימה האם למשתמש יש את המוצר או לא. אם כן הפעולה תחזיר 
        /// </summary>
        /// <param name="num"></param>
        /// <param name="purchases"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsExist(int num, List<Purchase>purchases, int id)
        {
            foreach(Purchase purchase in purchases)
            {
                if (purchase.UserId == id && purchase.ProductSetialNumber == num)
                    return true;
            }
            return false;
        }

       /// <summary>
       /// פעולת חזרה למסך הראשי. הפעולה תעדכן את הדמות 
       /// והרקע של המשתמש בהתאם לבחירותיו בדף ותעביר את המשתמש למסך הראשי
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DataBaseMethods.UpdateCurrentlyProductAndMoneyAndBackground(this.user, 0);
            Frame.Navigate(typeof(MenuPage), this.user);
        }

        /// <summary>
        /// פעולה שמעדכנת את השימוש בשחקן ילד ומשנה את 
        /// תוכן הכפתורים בהתאם ומעדכנת את התכונה במשתמש שהוא משתמש בדמות ילד
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UseChild_Click(object sender, RoutedEventArgs e)
        {
            this.user.CurrentCharacter = 1;
            UseDino.Content = "Use";
            UseRobot.Content = "Use";
            UseSanta.Content = "Use";
            UseChild.Content = "Used";
        }

        /// <summary>
        /// פעולה שמעדכנת את השימוש בשחקן סנטה ומשנה את 
        /// תוכן הכפתורים בהתאם ומעדכנת את התכונה במשתמש שהוא משתמש בדמות סנטה
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UseSanta_Click(object sender, RoutedEventArgs e)
        {
            this.user.CurrentCharacter = 2;
            UseDino.Content = "Use";
            UseRobot.Content = "Use";
            UseSanta.Content = "Used";
            UseChild.Content = "Use";
        }

        /// <summary>
        /// פעולה שמעדכנת את השימוש בשחקן רובוט ומשנה את 
        /// תוכן הכפתורים בהתאם ומעדכנת את התכונה במשתמש שהוא משתמש בדמות רובוט
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UseRobot_Click(object sender, RoutedEventArgs e)
        {
            this.user.CurrentCharacter = 3;
            UseDino.Content = "Use";
            UseRobot.Content = "Used";
            UseSanta.Content = "Use";
            UseChild.Content = "Use";
        }

        /// <summary>
        /// פעולה שמעדכנת את השימוש בשחקן דנוזאור ומשנה את 
        /// תוכן הכפתורים בהתאם ומעדכנת את התכונה במשתמש שהוא משתמש בדמות דינוזאור
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UseDino_Click(object sender, RoutedEventArgs e)
        {
            this.user.CurrentCharacter = 4;
            UseDino.Content = "Used";
            UseRobot.Content = "Use";
            UseSanta.Content = "Use";
            UseChild.Content = "Use";
        }


        /// <summary>
        /// פעולה שמעדכנת את השימוש ברקע שחור ומשנה את 
        /// תוכן הכפתורים בהתאם ומעדכנת את התכונה במשתמש שהוא משתמש רקע שחור
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UseBlackBackground_Click(object sender, RoutedEventArgs e)
        {
            this.user.CurrentBackground = 1;
            UseBlackBackground.Content = "Used";
            UsePurpleBackground.Content = "Use";
            UseBlueBackground.Content = "Use";
            UseRedBackground.Content = "Use";
        }

        /// <summary>
        /// פעולה שמעדכנת את השימוש ברקע כחול ומשנה את 
        /// תוכן הכפתורים בהתאם ומעדכנת את התכונה במשתמש שהוא משתמש רקע כחול
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UseBlueBackground_Click(object sender, RoutedEventArgs e)
        {
            this.user.CurrentBackground = 2;
            UseBlackBackground.Content = "Use";
            UsePurpleBackground.Content = "Use";
            UseBlueBackground.Content = "Used";
            UseRedBackground.Content = "Use";
        }

        /// <summary>
        /// פעולה שמעדכנת את השימוש ברקע סגול ומשנה את 
        /// תוכן הכפתורים בהתאם ומעדכנת את התכונה במשתמש שהוא משתמש רקע סגול
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UsePurpleBackground_Click(object sender, RoutedEventArgs e)
        {
            this.user.CurrentBackground = 3;
            UseBlackBackground.Content = "Use";
            UsePurpleBackground.Content = "Used";
            UseBlueBackground.Content = "Use";
            UseRedBackground.Content = "Use";
        }


        /// <summary>
        /// פעולה שמעדכנת את השימוש ברקע אדום ומשנה את 
        /// תוכן הכפתורים בהתאם ומעדכנת את התכונה במשתמש שהוא משתמש רקע אדום
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UseRedBackground_Click(object sender, RoutedEventArgs e)
        {
            this.user.CurrentBackground = 4;
            UseBlackBackground.Content = "Use";
            UsePurpleBackground.Content = "Use";
            UseBlueBackground.Content = "Use";
            UseRedBackground.Content = "Used";
        }
        /// <summary>
        /// פעולה שאם לוחצים על כפתור המידע ליד הדמות ילד הוא יקפיץ הודעה שתכיל מידע על הדמות ויכולותיה
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void InfoChild_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("This is the basic character. It has no special abilities.");
            dialog.Title = "Child Abilities";
            dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
            await dialog.ShowAsync();

        }
        /// <summary>
        /// פעולה שאם לוחצים על כפתור המידע ליד הדמות סנטה הוא יקפיץ הודעה שתכיל מידע על הדמות ויכולותיה
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void InfoSanta_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("This is an advanced character. The Santa run faster than other characters.");
            dialog.Title = "Santa Abilities";
            dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
            await dialog.ShowAsync();
        }
        /// <summary>
        /// פעולה שאם לוחצים על כפתור המידע ליד הדמות רובוט הוא יקפיץ הודעה שתכיל מידע על הדמות ויכולותיה
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void InfoRobot_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("This is an advanced character. The Robot Jump higher than other characters.");
            dialog.Title = "Robot Abilities";
            dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
            await dialog.ShowAsync();
        }
        /// <summary>
        /// פעולה שאם לוחצים על כפתור המידע ליד הדמות דינוזאור הוא יקפיץ הודעה שתכיל מידע על הדמות ויכולותיה
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void InfoDino_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("This is LEGEND character. The Dino Jump higher and run faster than other characters.");
            dialog.Title = "Dino Abilities";
            dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
            await dialog.ShowAsync();
        }
    }
}
