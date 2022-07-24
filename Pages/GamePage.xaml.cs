using DataBaseProject.Models;
using FinalProjectV1.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FinalProjectV1.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
 

    public sealed partial class GamePage : Page//מחלקה שלא ניתן לעבור ממנה בירושה ושעשויה להיות מוגדרת בעוד קבצים
    {
        Manager manager; // עצם מסוג מנגר
        public User user = null;//עצם מסוג משתמש
        private int AcheivedCoins = 0;//משתנה מסוג אינט שסוכם את הנקודות שהשחקן צבר המהלך המשחק ומתאפס כשהוא יוצא ממסך המשחק
        private int AcheivedScore = 0;//משתנה מסוג אינט שסוכם את כמות הנקודות שהשחקן צבר במשחק בכך שהוא קופץ על מדרגות- כל מדרגה שווה נקודה
        private bool EndGame;//משתנה בוליאני שבודק האם המשחק נגמר או לא
        private string Background;//משתנה מסוג סטרינג ששווה לרקע שיהיה במשחק

        /// <summary>
        /// הפעולה בונה שאחראית על ייצוג הדף ומציגה את הרכיבים על המסך
        /// </summary>
        public GamePage()
        {
            this.InitializeComponent();
            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();

        }
        /// <summary>
        /// . "פעולה שמתרחשת כאשר נפתח הדף משחק והיא מאפסת את המדתנים שסוכמים את הכסף והנקודות של המשתמש.
        /// בנוסף היא אחראית על קביעת הרקע של המשחק בהצאם למה שהמשתמש בוחר בדף ה"שחקן שלי 
        ///  בנוסף הפעולה מפעילה את האיבנטים שבמנגר שאחראיים על להראות את הניקוד והכסף שהמשתמש משיג במשחק 
        /// ולכתוב אותם על המסך באותו רגע
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.user != null)
            {
                this.AcheivedCoins = 0;
                this.AcheivedScore = 0;
                this.manager = new Manager(arena, this.user);
                manager.ShowCoin += Manager_ShowCoin;
                manager.ShowScore += Manager_ShowScore;
                manager.EndGame += Manager_EndGame;
                CoinsText.Text = "balance: " + 0;
                ScoreText.Text = "score: " + 0;
                HighScoreText.Text = "High Score: " + this.user.MaxScore.ToString();
                
                switch (this.user.CurrentBackground)
                {
                    case 1: this.Background = "ms-appx:///Assets/Backgrounds/BlackBackground.gif"; break;
                    case 2: this.Background = "ms-appx:///Assets/Backgrounds/BlueBackground.gif"; break;
                    case 3: this.Background = "ms-appx:///Assets/Backgrounds/PurpleRainBackground.gif"; break;
                    case 4: this.Background = "ms-appx:///Assets/Backgrounds/redBackground.gif"; break;
                }
                
                Uri imageUri = new Uri(Background, UriKind.Absolute);
                BitmapImage imageBitmap = new BitmapImage(imageUri);
                BackgroundGame.ImageSource = imageBitmap;
                
            }
        }

        /// <summary>
        /// פעולה שאחראית על להראות את הכסף שהמשתמש צובר במהלך 
        /// המשחק על ידי קבלת סכום הכסף שהשחקן השיג מהמנגר 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Manager_ShowCoin(object sender, EventArgs e)
        {
            this.AcheivedCoins = (int)sender;
            CoinsText.Text = "balance: " + (AcheivedCoins).ToString();
        }

        /// <summary>
        /// פעולה שאחראית על הצגת הנקודות שהמשתמש 
        /// צובר תוך כדי משחק על ידי קבלתם מהמנגר והשמתם כטקסט שמוצג על המסך
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Manager_ShowScore(object sender, EventArgs e)
        {
            this.AcheivedScore = (int)sender;
            ScoreText.Text = "score: " + (AcheivedScore).ToString();
        }

        /// <summary>
        /// פעולה שבודקת האם המשחק אכן נגמר, במידה וכן הוא מעדכנת את הכסף של השחקן 
        /// בדאטא בייס ובמידה והוא צבר מספר נקודות מקסימלי אז היא תעדכן אותם גם כן
        ///   והפעולה תקפיץ מודעה שתשאל האם השחקן מעוניין להמשיך למשחק חדש או
        ///   לצאת לתפריט הראשי. במידה ויבחר יהתחעל משחק חדש יופעל סאונד של תחילת משחק
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Manager_EndGame(object sender, EventArgs e)
        {
            this.EndGame = (bool)sender;
            if (this.EndGame)
            {
                this.EndGame = false;
                if(this.AcheivedCoins!=0)
                    this.user = DataBaseProject.DataBaseMethods.UpdateCurrentlyProductAndMoneyAndBackground(user, AcheivedCoins);

                if (this.AcheivedScore > this.user.MaxScore)
                    this.user = DataBaseProject.DataBaseMethods.UpdateCurrentlyHighScore(user, AcheivedScore);

                StackPanel panel = new StackPanel
                {
                    Width = 200
                };
                panel.Orientation = Orientation.Vertical;
                ContentDialog firstPopUp = new ContentDialog()
                {
                    Title = "GAME OVER ",
                    Content = panel,
                    Background = new SolidColorBrush(Colors.LightGray),
                    Width = 400,
                    PrimaryButtonText = "START AGAIN",
                    SecondaryButtonText = "RETURN TO HOME"
                };
                var answer = await firstPopUp.ShowAsync();
                if (answer == ContentDialogResult.Primary)//בהודעה המשתמש לחץ על להתחיל משחק מחדש
                {
                    Frame.Navigate(typeof(GamePage), this.user);

                    PlaySound("GameBeginSound.mp3");//הפעלת המוזיקה של התחלת משחק
                }
                else
                {
                    Frame.Navigate(typeof(MenuPage), this.user);
                }
            }
        }

        /// <summary>
        /// פעולה שמתחרשת כאשר השחקן לחץ על כפתור החזרה הביתה. הפעולה תקפיץ מודעה שתשאל האם השחקן 
        /// בטוח שהוא רוצה לצאת מהמשחק. במודעה כתוב כיאם השדחקן יוצא הכסף והשיא שלו לא נשמרים.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void home_Click(object sender, RoutedEventArgs e)
        {
            this.EndGame = false;
            StackPanel panel = new StackPanel
            {
                Width = 200
            };
            panel.Orientation = Orientation.Vertical;
            ContentDialog firstPopUp = new ContentDialog()
            {
                Title = "Are you sure you want to Exit the game? you will lose the points you acheived ",
                Content = panel,
                Background = new SolidColorBrush(Colors.LightGray),
                Width = 400,
                PrimaryButtonText = "Ok",
                SecondaryButtonText = "Cancel"
            };
            var answer = await firstPopUp.ShowAsync();
            if (answer == ContentDialogResult.Primary)//אם המשתמש לחץ על ok בהודעה
            {
                this.EndGame = false;

                Frame.Navigate(typeof(MenuPage), this.user);// חזרה לתפריט הראשי
            }   
        }
        /// <summary>
        /// פעולה שיוצרת אלמנט מסוג mediaelement ניגשת לתיקיה ומפעילה את המוזיקה 
        /// ששם הקובץ סאונד 
        /// שלה זהה למשתנה שהפעולה מקבלת 
        /// </summary>
        /// <param name="FilePath">קובץ הסאונד שאנחנו רוצים להפעיל</param>
        private async void PlaySound(string FilePath)
        {
            MediaElement PlayMusic = new MediaElement();//יצירת קובץ סאונד חדש
            StorageFolder Folder = Windows.ApplicationModel.Package.Current.InstalledLocation;//התיקיה שבה הסאונד נמצא
            Folder = await Folder.GetFolderAsync("sound");//מציאת התיקיה
            StorageFile sf = await Folder.GetFileAsync(FilePath);//הקובץ של הסאונד בתיקיה
            PlayMusic.SetSource(await sf.OpenAsync(FileAccessMode.Read), sf.ContentType);//קריאת הקובץ
            PlayMusic.Play();//הפעלת הסאונד

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

        

       
    }
}
