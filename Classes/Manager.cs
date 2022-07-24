using DataBaseProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace FinalProjectV1.Classes
{
    class Manager
    {
        private int AchievedCoins;//מטבעות שהשחקן משיג במשחק נוכחי
        private List<Stair> stairs;//רשימה מסוג מדרגה שתכיל את המדרגות במשחק
        private Coin coin;//עצם מטבע מסוג מטבע
        private bool isOnStair;//משתנה שבודק האם הדמות על המדרגה
        private Canvas arena;//עצם מסוג קאנבאס שמייצג את מגרש המשחק
        private Random random;//עצם רנדום מסוג רנדום
        private Character character;//עצם דמות מסוג דמות
        private DispatcherTimer AddStair;// טיימר שמוסיף מדרגות
        private DispatcherTimer CollisionTimer;// טיימר שבודק התנגשות של הדמות עם המדרגות
        private DispatcherTimer CollisionWithCoinTimer;// טיימר שבודק התנגדשות דמות עם המטבעות
        private DispatcherTimer EndGameTimer;//עצם מסוג טיימר שבודק סיום משחק
        private StairType Type;//משתנה סוג מדרגה
        public User user = null;//משתמש במשחק
        public event EventHandler ShowScore;//איבנט שמראה את הניקוד של השחקן
        public event EventHandler ShowCoin;//איבנט שמראה את הכסף שהשחקן צובר במהלך המשחק
        public event EventHandler EndGame;  //איבנט שמסיים את המשחק
        public bool IsEndGame;//משתנה שבודק האם נגמר המשחק
        private int Score;//הנקודות שצובר השחקן במשחק כשהוא קופץ על המדרגות
        private double SpeedYCounter;//משתנה של מהירות המדרגה בציר וואי
        private double CanvasHeight;//משתנה של גובה הקאנבאס ההתחלתי כשמתחיל המשחק
        //private bool IsSounds;//משתנה בוליאני שבודק אם הסאונד מופעל או לא
        private double StairInterval;//משתנה מסוג דאבל שאחראי למהירות שמתרחש הטיימר שמייצר מדרגות חדשות
         
        /// <summary>
        /// פעולה בונה עצם מסוג מנגר אשר אחראי על הפעולות במשחק
        /// </summary>
        /// <param name="arena">מגרש המשחק</param>
        /// <param name="user">המשתמש המחובר במשחק</param>
        public Manager(Canvas arena,DataBaseProject.Models.User user)
        {
            this.StairInterval = 2.5;
            this.SpeedYCounter = 2;//הגדרת מהירות המדרגה כ2 בציר האנכי
            this.AchievedCoins = 0;
            this.Score = 0;
            IsEndGame = false;
            this.user = user;
            this.arena = arena;
            switch(this.user.CurrentCharacter)
            {
                case 1:this.character = new Boy(110, 500, arena, 100, 100);break;
                case 2:this.character = new Santa(110, 500, arena, 100, 100);break;
                case 3:this.character = new Robot(110, 500, arena, 100, 100);break;
                case 4:this.character = new Dino(110, 500, arena, 100, 100);break;   
            }
            
            this.isOnStair = false;
            this.stairs = new List<Stair>();
            this.random = new Random();
            
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
            
            this.AddStair = new DispatcherTimer();
            AddStair.Tick += AddStair_Tick;
            AddStair.Start();
            AddStair.Interval = TimeSpan.FromSeconds(StairInterval);
            
            this.stairs.Add(new RegularStair(random.Next(20, (int)(arena.ActualWidth - 370)), -50, arena, 350, 50,SpeedYCounter));

            this.CollisionTimer = new DispatcherTimer();
            CollisionTimer.Tick += CollisionTimer_Tick;
            CollisionTimer.Start();
            CollisionTimer.Interval = TimeSpan.FromMilliseconds(0.000001);

            this.CollisionWithCoinTimer = new DispatcherTimer();
            CollisionWithCoinTimer.Tick += CollisionWithCoin_Tick;
            CollisionWithCoinTimer.Interval = TimeSpan.FromMilliseconds(0.0001);

            this.EndGameTimer = new DispatcherTimer();
            this.EndGameTimer.Tick += EndGameTimer_Tick;
            this.EndGameTimer.Start();
            this.EndGameTimer.Interval = TimeSpan.FromMilliseconds(0.0001);
            this.CanvasHeight = this.arena.ActualHeight;

        }

        /// <summary>
        /// טיימר שבודק אם המשחק הסתיים 
        /// בכך שבודק האם הדמות הגיעה למיקום מסויים
        /// בציר בוואי ואז היא מפעילה סאונד סיום משחק ואת האיבנט סיום משחק
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void EndGameTimer_Tick(object sender, object e)
        {
            if ((this.character.placeY > (this.CanvasHeight + 130)) && !IsEndGame)
            {
                PlaySound("yougottabekiddingmesound.mp3");
                IsEndGame = true;
                EndGame(IsEndGame, null);

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
        /// טיימר שמפעיל את הפעולה שמעדכנת את המדרגות
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddStair_Tick(object sender, object e)
        {
            UpdateStair();
        }

        /// <summary>
        /// טיימר שבודק התנגשות של הדמות עם המדרגות כאשר
        /// הוא בודק אם הדמות מתנגשת עם המדרגה בלמעלה של המדרגה
        /// כלומר הדמות יכולה לעבור דרך המדרגה אם היא קופצת למעלה
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CollisionTimer_Tick(object sender, object e)
        {
            double speedx = 0;
            double speedy = 0;
            isOnStair = false;
            for (int i = 0; i < stairs.Count; i++)
            {
                double CharacterLeft, CharacterRight, CharacterTop, CharacterBottom;
                Image Character1 = character.GetImage();
                double StairLeft, StairRight, StairTop, StairBottom;
                Image Stair1 = this.stairs[i].GetImage();
                CharacterLeft = Canvas.GetLeft(Character1) + 10;
                CharacterRight = Character1.Width + CharacterLeft - 20;
                CharacterTop = Canvas.GetTop(Character1);
                CharacterBottom = Character1.Height + CharacterTop;
                StairLeft = Canvas.GetLeft(Stair1);
                StairRight = Stair1.Width + StairLeft;
                StairTop = Canvas.GetTop(Stair1);
                StairBottom = Stair1.Height + StairTop;
                bool side_ab = CharacterLeft < StairRight && CharacterRight > StairLeft;
                bool top_ab = CharacterTop < StairBottom && CharacterBottom > StairTop;
                bool is_speed_positive = character.SpeedY > 0;
                this.stairs[i].SpeedY = SpeedYCounter;
                if (side_ab)
                {
                    if (top_ab && is_speed_positive)
                    {
                        if (CharacterBottom <= StairTop + 20)
                        {
                            if (!stairs[i].IsHitStair)
                            {
                                stairs[i].IsHitStair = true;
                                this.Score++;
                                ShowScore(this.Score, null);
                            }
                            this.isOnStair = true;
                            speedx = this.stairs[i].SpeedX;
                            speedy = this.stairs[i].SpeedY;
                            if (character.checkfirst)
                            {
                                this.coin = new Coin(random.Next(0, 600), 0, arena, 90, 90);
                                character.checkfirst = false;
                                CollisionWithCoinTimer.Start();
                            }
                        }
                    }
                }             
            }
            if (isOnStair)
            {
                character.Accelaration = 0;
                character.SpeedY = speedy;
                character.baseSpeed = speedx;
                if(this.character.state == Character.StateType.idle)
                   this.character.GoIdle();
                if(this.character.state == Character.StateType.StandLeft)
                   this.character.GoIdleLeft();
            }
            if(!isOnStair)
               this.character.Accelaration = 0.8;
            if (!isOnStair && character.baseSpeed != 0)//בדיקה אם הדמות לא על מדרגה ובמצב שהמהירות של העצם עליו היא הייתה לא 0 אז להשוות אותו ל0 כי היא לא על עצם מדרגה יותר
                character.baseSpeed = 0;
        }
        /// <summary>
        ///    falseאחרת תחזיר trueשל הדמות מתנגש בשל המטבע, אם כן הפעולה תחזיר rect פעולה שבודקת האם ה 
        /// </summary>
        /// <param name="character1">הדמות במשחק</param>
        /// <param name="coin1">המטבע שאיתו הדמות מתנגשת</param>
        /// <returns></returns>
        private bool IntersectWith(Character character1, Coin coin1)
        {
            Rect character = new Rect(character1.PlaceX, character1.placeY, 100, 100);
            Rect coin = new Rect(coin1.PlaceX, coin1.placeY, 90, 90);
            Rect help = RectHelper.Intersect(character, coin);
            if (help.Width > 0 || help.Height > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///הפעולה תשנה את תמונת המטבע בהתאם לסוג המטבע ותוסיף כסף לסכום אותו צבר השחקן באותו משחק, true במידה ו .false או true
        ///תחזיר IntersectWith טיימר שבודק האם הפעולה   
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CollisionWithCoin_Tick(object sender, object e)
        {
            if (IntersectWith(character, coin))
            {
                switch (coin.type)
                {
                    case Coin.CoinType.gold:
                        this.AchievedCoins+=100;
                        break;
                    case Coin.CoinType.silver:
                       this.AchievedCoins+=50;
                        break;
                    case Coin.CoinType.bronze:
                        AchievedCoins+=10;
                        break;
                }
                Canvas.SetTop(coin.GetImage(), 900);
                this.coin.placeY = 900;
                ShowCoin(this.AchievedCoins, null);  
                

            }
           
               

        }


        /// <summary>
        /// פעולה שמעדכנת את המדרגות. 
        /// תחילה לגבי המדרגות הנעות הפעולה מגרילה מספר ובודקת אם המספר 
        /// שווה ל1 אז המדרגה התנוע ימינה ולא היא תנוע שמאלה בהתחלה .
        /// לאחר מכן, הפעולה בודקת את השלב הראשון במשחק שבו
        /// כל המדרגות הם מדרגות שנעות כלפי מטה אלא אם כן השחקן
        ///   .קפץ על למעלה מ20 מדרגות ואז המשחק עובר לרק מדרגות נעות
        ///   בנוסף הפעולה גורמת לכך שכל 20 מדרגות שהשחקן קופץ עליהן המשחק יחיל להיות 
        ///   מהיר יותר כלומר המדרגות יתחילו לנוע מהר יותר אך כאשר מספר המדרגות 
        ///   יעמוד על 80 המדרגות יישארו בקצב קבוע ולא יגדילו עוד את מהירותם
        ///   
        /// </summary>
        public void UpdateStair()
        {
            int HelpNum = random.Next(1,3);//עוזר בלבחור את הכיוון ההתחלתי של המדרגה
            
            if (HelpNum==1)
                HelpNum = 1;
            else if(HelpNum==2)
                HelpNum = -1;

            if (this.Score < 20)
                this.Type = StairType.RegularStair;

            else if (stairs.Count() >= 20)
                this.Type = StairType.MovingStair;

            if (this.Score % 20 == 0 &&this.Score!=0 && this.Score<80)
            {
                SpeedYCounter+=1;
                if (StairInterval > 0.6)
                    AddStair.Interval = TimeSpan.FromSeconds((StairInterval - 0.6));
            }
            if(this.Type == StairType.RegularStair)
            {
                try
                {
                    this.stairs.Add(new RegularStair(random.Next(20, (int)(arena.ActualWidth - 370)), -50, arena, 350, 50,SpeedYCounter));
                }
                catch
                {
                    
                }
            }
            else if (this.Type==StairType.MovingStair)
            {      
                try
                {
                    this.stairs.Add(new MovingStair(random.Next(20, (int)(arena.ActualWidth - 370)), -50, arena, 350, 50, SpeedYCounter,(SpeedYCounter*HelpNum)));
                }
                catch
                {

                }
            }                    
        }
        /// <summary>
        /// פעולה שמופעלת כאשר 
        /// עוזבים את כפתורי החצים במקלדת. במצב כזה הפעולה בודקת אם עזבנו את חץ ימינה היא תשנה את 
        /// מצב הדמות לעמידה ימינה ואם עזבנו את הכפתור חץ שמאלה אז מצב הדמות יהיה עמידה שמאלה
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            
                if (args.VirtualKey == Windows.System.VirtualKey.Right)
                    this.character.GoIdle();
                if (args.VirtualKey == Windows.System.VirtualKey.Left)
                    this.character.GoIdleLeft();
            
        }
        /// <summary>
        /// פעולה שמועלת כאשר המשתמש לחת על אחד החצים במקלדת. 
        /// הפעולה תבדוק אם הדמות של השחקן היא אחת מהדמויות המיוחדות אז הפעולה תשלח 
        /// משתנה בוליאני שיגיד האם הדמות
        /// היא מיוחדת או לא ובהתאם תוגדל המהירות בציר איקס או בציר וואי. הדמויות סנטה ודרקון
        /// שמספרם הוא 2,4 רצות מהר יותר והדמויות רובוט ודרקון קופצות גבוה יותר ומספרן הוא 3,4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {

            if (args.VirtualKey == Windows.System.VirtualKey.Right)//אם המשתמש לחץ על החץ ימינה במקלדת
            {
                if (this.user.CurrentCharacter == 2 ||this.user.CurrentCharacter == 4)//בדיקה האם הדמות היא סנטה או דרקון. אם הדמות דרקון היא יכולה גף לרוץ מהר וגם לקפוץ מהר
                    this.character.GoRight(true);//הפעלת הפעולה ריצה ימינה שיודעים שהדמות סנטה או דרקון ובכך הדמות תרוץ מהר יותר
                else
                    this.character.GoRight(false);// במידה והדמות של השחקן היא לא סנטה או דרקון
            }
            if (args.VirtualKey == Windows.System.VirtualKey.Left)//אם השחקן לחץ על החץ שמאלה במקלדת
            {
                if(this.user.CurrentCharacter==2 || this.user.CurrentCharacter==4)//בדיקה האם הדמות היא סנטה או דרקון
                    this.character.GoLeft(true);//הפעלת הפעולה ריצה ימינה שיודעים שהדמות סנטה או דרקון ובכך הדמות תרוץ מהר יותר
                else
                    this.character.GoLeft(false);//במידה והדמות של השחקן היא לא סנטה או דרקון


            }
            if (args.VirtualKey == Windows.System.VirtualKey.Up)
            {
                if (this.user.CurrentCharacter == 4 || this.user.CurrentCharacter == 3)//אם הדמויות הם רובוט או דרקון אז הדמות תקפוץ גבוה יותר
                    this.character.Jump(isOnStair, true);
                else
                    this.character.Jump(isOnStair, false);

            }

            
            
        }

        /// <summary>
        /// סוגי המדרגות שיש לי במשחק
        /// </summary>
        public enum StairType
        {
            MovingStair,RegularStair
        }
    }

}
