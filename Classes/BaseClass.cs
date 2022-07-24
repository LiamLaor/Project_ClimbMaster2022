using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FinalProjectV1.Pages
{
    /// <summary>
    /// מחלקת הבסיס ששאר המחלוקות יורשות ממנה
    /// </summary>
    class BaseClass
    {
        public double PlaceX { get; set; } //מיקום הגוף ביחס לציר אופקי
        public double placeY { get; set; } //מהירות הגוף בציר אנכי
        protected Image image;//תמונת האובייקט
        protected Canvas arena;//מגרש המשחק
        protected DispatcherTimer moveTimer;//טיימר המשחק שפועל בכל אלפית שנייה
        public double SpeedX { get; set; }//מהירות הדמות בציר אופקי
        public double SpeedY { get; set; }//מהירות הדמות בציר אנכי
        public Point position;//מיקום הדמות
        public Size size;//גודל הדמות
        /// <summary>
        /// הפעולה בונה עצם יסוד
        /// </summary>
        /// <param name="placeX">מיקום בציק איקס</param>
        /// <param name="placeY">מיקום בציר וואי</param>
        /// <param name="arena">לוח המשחק</param>
        /// <param name="Width"רוחב></param>
        /// <param name="Height">גובה</param>
        public BaseClass(double placeX, double placeY, Canvas arena, double Width, double Height)
        {
            this.PlaceX = placeX;
            this.placeY = placeY;
            this.image = new Image();
            this.arena = arena;
            this.image.Width = Width;
            this.image.Height = Height;
            Canvas.SetLeft(this.image, this.PlaceX);
            Canvas.SetTop(this.image, this.placeY);
            arena.Children.Add(image);
            this.moveTimer = new DispatcherTimer();
            this.moveTimer.Start();
            this.moveTimer.Interval = TimeSpan.FromMilliseconds(1);
            this.moveTimer.Tick += MoveTimer_Tick;
            this.SpeedX = 0;
            this.SpeedY = 0;
        }

        /// <summary>
        /// טיימר שמעדכן את המיקון בציר איקס ובציר וואי לפי המהירויות בצירים
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void MoveTimer_Tick(object sender, object e)
        {
            this.PlaceX += SpeedX;
            this.placeY += SpeedY;
            Canvas.SetLeft(this.image, this.PlaceX);
            Canvas.SetTop(this.image, this.placeY);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>המלבן מסביב לגוף</returns>
        public virtual Rect GetRectangle()
        {
            return new Rect(this.PlaceX, this.placeY, this.image.Width, this.image.Height);
        }

        /// <summary>
        /// הפעולה מחזירה את התמונה של הגוף
        /// </summary>
        /// <returns>תמונה של הגוף</returns>
        public virtual Image GetImage()
        {
            return this.image;
        }
        
    }
}
