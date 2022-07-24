using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace FinalProjectV1.Classes
{
    class MovingStair: Stair
    {
        /// <summary>
        /// פעולה בונה עצם מסוג מדרגה נעה שיורש ממדרגה
        /// </summary>
        /// <param name="placeX">מיקום המדרגה הנעה בציר האיקס</param>
        /// <param name="placeY">מיקום המדרגה הנעה בציר וואי</param>
        /// <param name="arena">מגרש המשחק</param>
        /// <param name="Width">רוחב המדרגה הנעה</param>
        /// <param name="Height">גובה עצם המדרגה הנעה</param>
        /// <param name="Speedy">מהירות המדרגה הנעה בציר וואי</param>
        /// <param name="speedx">מהירות המדרגה הנעה בציר איקס></param>
        public MovingStair(double placeX, double placeY, Canvas arena, double Width, double Height, double Speedy, double speedx) : base(placeX, placeY, arena, Width, Height, Speedy)
        {
            this.SpeedX = speedx;
            base.image.Source = new BitmapImage(new Uri("ms-appx:///Assets/BigiceStair.png"));
        }

       /// <summary>
       /// טיימר שמעדכן בנוסף לטיימר הבסיסי שמעדכן את מיקום המדרגה הוא מעדכן שהמדרגה 
       ///  תתנגש בקירות ותחזור במהירות נגדית כלומר אם המדרגה מתנגשת בקיר ימין היא תוחזר שמאלה ולהפך
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        protected override void MoveTimer_Tick(object sender, object e)
        {
            base.MoveTimer_Tick(sender, e);
            if (this.PlaceX >= (this.arena.ActualWidth-350 ))
            {
                this.SpeedX *=-1;
            }
            else if (this.PlaceX <= 0)
            {
                this.SpeedX *=-1 ;
            }

        }
    }
}
