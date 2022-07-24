using DataBaseProject;
using DataBaseProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class StorePage : Page
    {
        private User user = null;//עצם מסוג משתמש
        private List<Product> products;//רשימת מוצרי החנות מסוג מוצר

        /// <summary>
        /// הפעולה בונה שאחראית על ייצוג הדף ומציגה את הרכיבים על המסך
        /// </summary>
        public StorePage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// פעולה שמחזירה את המשתמש למסך הראשי תוך שמירתו מחובר למשחק
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
        /// פעולת הקנייה של הדמות סנטה. הפעולה מוסיפה רכישה למשתמש כלומר מעדכנת את הדאטא 
        /// בייס בטבלה של הרכישות שהמשתמש המחובר רכש את הדמות הזו. 
        /// בנוסף הפעולה מבטלת את האפשרות ללחוץ על כפתור זה שוב,
        /// משנה את התוכן שכתוב על הכפתור ומרעננת את הדף כלומר 
        /// מעבירה את השחקן לדף זה שוב כדי שיתעדכן הכסף של השחקן כדי שלא יוכל לרכוש מוצרים שאין לו כסף בעבורם
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></par
        private void BuySanta_Click(object sender, RoutedEventArgs e)
        {
            this.user = DataBaseMethods.AddPurchase(user, products[1].Price, 2);
            BuySanta.IsEnabled = false;
            BuySanta.Content = "Owned";
            Frame.Navigate(typeof(StorePage), this.user);

        }

        /// <summary>
        /// פעולת קניית הילד לא מופעלת לעולם כי הילד הוא הדמות הבסיסית במשחק שלכולם יש אותו
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuyChild_Click(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// פעולת הקנייה של הדמות רובוט. הפעולה מוסיפה רכישה למשתמש כלומר מעדכנת את הדאטא 
        /// בייס בטבלה של הרכישות שהמשתמש המחובר רכש את הדמות הזו. 
        /// בנוסף הפעולה מבטלת את האפשרות ללחוץ על כפתור זה שוב,
        /// משנה את התוכן שכתוב על הכפתור ומרעננת את הדף כלומר 
        /// מעבירה את השחקן לדף זה שוב כדי שיתעדכן הכסף של השחקן כדי שלא יוכל לרכוש מוצרים שאין לו כסף בעבורם
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></par>
        private void BuyRobot_Click(object sender, RoutedEventArgs e)
        {
            this.user = DataBaseMethods.AddPurchase(user, products[2].Price, 3);
            BuyRobot.IsEnabled = false;
            BuyRobot.Content = "Owned";
            Frame.Navigate(typeof(StorePage), this.user);

        }


        /// <summary>
        /// פעולת הקנייה של הדמות דינזאור. הפעולה מוסיפה רכישה למשתמש כלומר מעדכנת את הדאטא 
        /// בייס בטבלה של הרכישות שהמשתמש המחובר רכש את הדמות הזו. 
        /// בנוסף הפעולה מבטלת את האפשרות ללחוץ על כפתור זה שוב,
        /// משנה את התוכן שכתוב על הכפתור ומרעננת את הדף כלומר 
        /// מעבירה את השחקן לדף זה שוב כדי שיתעדכן הכסף של השחקן כדי שלא יוכל לרכוש מוצרים שאין לו כסף בעבורם
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuyDino_Click(object sender, RoutedEventArgs e)
        {
            this.user = DataBaseMethods.AddPurchase(user, products[3].Price, 4);
            BuyDino.IsEnabled = false;
            BuyDino.Content = "Owned";
            Frame.Navigate(typeof(StorePage), this.user);

        }

        /// <summary>
        ///פעולה שמופעלת בטעינת דף החנות, הפעולה יוצרת רשימת מוצרים מסוג מוצר ומשימה לתוכה את 
        ///רשימת כל הקניות של כל המשתמשים במשחק. הפעולה קודם כל מניחה 
        ///כי למשתמש אין אף מוצר, לאחר מכן הפעולה בודקת על כל דמות 
        ///ורקע בחנות האם למשתמש יש אותם בעזרת פעולה חיצונית שמופעלת,  
        ///במידה וכן משתנה כיתוב הכפתור בהתאם והאפשרות ללחוץ עליו.
        ///אם למשתמש אין את הדמות או הרקע הפעולה תבדוק באמצעות 
        ///פעולה חיצונית האם למשתמש יש מספיק כסף לקנות את המוצר, 
        ///ואם כן תאפשר לחיצה על כפתור הקנייה בהתאם למוצר ולמחירו.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<Purchase> purchases;
            if (this.user != null)
            {
                this.products = DataBaseMethods.GetProducts();
                purchases = DataBaseMethods.GetPurchases(this.user.Id);
                CoinText.Text = "Coins: "+user.Coins.ToString();
                BuySanta.IsEnabled = false;
                if ((IsExist(2, purchases, this.user.Id)))
                {
                    BuySanta.Content = "Owned";
                    BuySanta.IsEnabled = false;
                }
                else if (IsEnoughToBuy(2, this.user.Coins, products)) 
                    BuySanta.IsEnabled = true;

                BuyRobot.IsEnabled = false;
                if ((IsExist(3, purchases, this.user.Id)))
                {
                    BuyRobot.Content = "Owned";
                    BuyRobot.IsEnabled = false;
                }
                else if(IsEnoughToBuy(3, this.user.Coins, products))
                    BuyRobot.IsEnabled = true;

                BuyDino.IsEnabled = false;
                if ((IsExist(4, purchases, this.user.Id)))
                {
                    BuyDino.Content = "Owned";
                    BuyDino.IsEnabled = false;
                }
                else if(IsEnoughToBuy(4, this.user.Coins, products))
                    BuyDino.IsEnabled = true;

                BuyBlueBackground.IsEnabled = false;
                if (IsExist(5, purchases, this.user.Id))
                {
                    BuyBlueBackground.Content = "Owned";
                    BuyBlueBackground.IsEnabled = false;
                }
                else if (IsEnoughToBuy(5, this.user.Coins, products))
                    BuyBlueBackground.IsEnabled = true;

                BuyPurpleBackground.IsEnabled = false;
                if (IsExist(6, purchases, this.user.Id))
                {
                    BuyPurpleBackground.Content = "Owned";
                    BuyPurpleBackground.IsEnabled = false;
                }
                else if (IsEnoughToBuy(6, this.user.Coins, products))
                    BuyPurpleBackground.IsEnabled = true;

                BuyRedBackground.IsEnabled = false;
                if (IsExist(7, purchases, this.user.Id))
                {
                    BuyRedBackground.Content = "Owned";
                    BuyRedBackground.IsEnabled = false;
                }
                else if (IsEnoughToBuy(7, this.user.Coins, products))
                    BuyRedBackground.IsEnabled = true;
            }
        }
        /// <summary>
        /// פעולה שבודקת אם יש לך מספיק כסף לקנות מוצר מהחנות
        /// </summary>
        /// <param name="num"></param>
        /// <param name="coins"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        private bool IsEnoughToBuy(int num, int coins, List<Product> products)
        {
            foreach(Product product in products)
            {
                if (coins >= product.Price && num == product.ProductSerialNumber)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// פעולה שבודקת אם יש לך את הדמויות כבר כדי לא לאפשר לך לקנות אותם שוב
        /// </summary>
        /// <param name="num"></param>
        /// <param name="purchases"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsExist(int num, List<Purchase> purchases, int id)
        {
            foreach (Purchase purchase in purchases)
            {
                if (purchase.UserId == id && purchase.ProductSetialNumber == num)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// פעולת קנייה של רקע כחול שמופעלת בלחיצה על כפתור הרכישה. הפעולה מוסיפה לטבלת 
        /// הרכישות בדאטא בייס את הרכישה של הרקע הכחול לזהות השחקן שמחובר למשחק, 
        /// page loadedמבטלת את האפשרות ללחוץ על כפתור הרכישה שוב ועושה רענון לדף כדי שה 
        /// יפעל שוב על מנת שהכסף יתעדכן כדי שהשחקן לא יוכל לרכוש מוצרים כשאין לו את הכסף
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuyBlueBackground_Click(object sender, RoutedEventArgs e)
        {
            this.user = DataBaseMethods.AddPurchase(user, products[4].Price, 5);
            BuyBlueBackground.IsEnabled = false;
            BuyBlueBackground.Content = "Owned";
            Frame.Navigate(typeof(StorePage), this.user);
        }

        /// <summary>
        /// פעולת קנייה של רקע סגול שמופעלת בלחיצה על כפתור הרכישה. הפעולה מוסיפה לטבלת 
        /// הרכישות בדאטא בייס את הרכישה של הרקע הסגול לזהות השחקן שמחובר למשחק, 
        /// page loadedמבטלת את האפשרות ללחוץ על כפתור הרכישה שוב ועושה רענון לדף כדי שה 
        /// יפעל שוב על מנת שהכסף יתעדכן כדי שהשחקן לא יוכל לרכוש מוצרים כשאין לו את הכסף
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuyPurpleBackground_Click(object sender, RoutedEventArgs e)
        {
            this.user = DataBaseMethods.AddPurchase(user, products[5].Price, 6);
            BuyPurpleBackground.IsEnabled = false;
            BuyPurpleBackground.Content = "Owned";
            Frame.Navigate(typeof(StorePage), this.user);
        }

        /// <summary>
        /// פעולת קנייה של רקע אדום שמופעלת בלחיצה על כפתור הרכישה. הפעולה מוסיפה לטבלת 
        /// הרכישות בדאטא בייס את הרכישה של הרקע האדום לזהות השחקן שמחובר למשחק, 
        /// page loadedמבטלת את האפשרות ללחוץ על כפתור הרכישה שוב ועושה רענון לדף כדי שה 
        /// יפעל שוב על מנת שהכסף יתעדכן כדי שהשחקן לא יוכל לרכוש מוצרים כשאין לו את הכסף
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuyRedBackground_Click(object sender, RoutedEventArgs e)
        {
            this.user = DataBaseMethods.AddPurchase(user, products[6].Price, 7);
            BuyRedBackground.IsEnabled = false;
            BuyRedBackground.Content = "Owned";
            Frame.Navigate(typeof(StorePage), this.user);
        }
    }
}
