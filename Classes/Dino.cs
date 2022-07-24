using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace FinalProjectV1.Classes
{
    class Dino: Character
    {
        /// <summary>
        /// פעולה בונה אשר יוצרת דמות דינוזאור אשר יורש מהמחלקה character.
        /// </summary>
        /// <param name="placeX">מיקום הדמות בציר איקס</param>
        /// <param name="placeY">מיקום הדמות בציר Y</param>
        /// <param name="arena">מגרש המשחק- הcanvas</param>
        /// <param name="Width">רוחב תמונת הדמות</param>
        /// <param name="Height">גובה תמונת הדמות</param>
        public Dino(double placeX, double placeY, Canvas arena, double Width, double Height) : base(placeX, placeY, arena, Width, Height)
        {
            this.state = StateType.idle; //הגדרת מצב התחלתי של עמידה
            MatchGif();
            this.SpeedX = 0;//איפוס המהירות בציר x 
            this.SpeedY = 0;//איפוס מהירות הדמות בציר Y
            base.Accelaration = 0;//איפוס תאוצת הדמות בציר Y
        }

        /// <summary>
        /// פעולה שמתאימה את מצב הדמות אל הגיף המתאים כך שכאשר הדמות תזוז לכיוון מסויים הגיף ישתנה בהתאם
        /// </summary>
        public override void MatchGif()
        {
            switch (this.state)
            {
                case StateType.runRight:
                    this.image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Characters/Dino/RunDinoRight.gif"));
                    break;
                case StateType.runLeft:
                    this.image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Characters/Dino/DinoRunLeft.gif"));
                    break;
                case StateType.idle:
                    this.image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Characters/Dino/IdleDinoRight.gif"));
                    break;
                case StateType.JumpRight:
                    this.image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Characters/Dino/DinoJumpRight.gif"));
                    break;
                case StateType.StandLeft:
                    this.image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Characters/Dino/IdleDinoLeft.gif"));
                    break;
                case StateType.JumpLeft:
                    this.image.Source = new BitmapImage(new Uri("ms-appx:///Assets/Characters/Dino/DinoJumpLeft.gif"));
                    break;
            }
        }
    }
}
