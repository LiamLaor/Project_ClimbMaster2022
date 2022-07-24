using FinalProjectV1.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace FinalProjectV1.Classes
{
    class Coin : BaseClass
    {
        public CoinType type { get; set; }//משתנה מסוג סוג מטבע
        private Random random;//משתנה רנדום מסוג רנדום
         
        /// <summary>
        /// פעולה בונה עצם חדש מסוג מטבע
        /// </summary>
        /// <param name="placeX">מיקום מטבע בציר איקס</param>
        /// <param name="placeY">מיקום מטבע בציר וואי</param>
        /// <param name="arena">מגרש המשחק</param>
        /// <param name="Width">רוחב המטבע</param>
        /// <param name="Height">גובה צורת המטבעע</param>
        public Coin(double placeX, double placeY, Canvas arena, double Width, double Height) : base(placeX, placeY, arena, Width, Height)
        {
            this.random = new Random();
            this.type = (CoinType)this.random.Next(3);
            base.SpeedY = 5;
            switch (this.type)
            {
                case CoinType.gold:
                    base.image.Source = new BitmapImage(new Uri("ms-appx:///Assets/GoldCoin.png"));
                    break;
                case CoinType.silver:
                    base.image.Source = new BitmapImage(new Uri("ms-appx:///Assets/SilverCoin.png"));
                    break;
                case CoinType.bronze:
                    base.image.Source = new BitmapImage(new Uri("ms-appx:///Assets/BronzeCoin.png"));
                    break;
            }
            base.moveTimer.Tick += MoveTimer_Tick;
            base.moveTimer.Start();
            base.moveTimer.Interval = TimeSpan.FromMilliseconds(1);
            position = new Point(PlaceX, placeY);
            size = new Size(Width, Height);

        }

        /// <summary>
        /// פעולה שמעדכנת את סוג המטבע ומתאימה לו תמונה בהתאם
        /// ומעדכנת לו את מיקום בציר איקס ברנדומליות ובציר וואי מעל המסך.
        /// </summary>
        public void UpdateCoin()
        {
            if (placeY > 1400)
            {
                this.type = (CoinType)this.random.Next(3);
                switch (this.type)
                {
                    case CoinType.gold:
                        base.image.Source = new BitmapImage(new Uri("ms-appx:///Assets/GoldCoin.png"));
                        break;
                    case CoinType.silver:
                        base.image.Source = new BitmapImage(new Uri("ms-appx:///Assets/SilverCoin.png"));
                        break;
                    case CoinType.bronze:
                        base.image.Source = new BitmapImage(new Uri("ms-appx:///Assets/BronzeCoin.png"));
                        break;
                }
                Canvas.SetLeft(base.image, random.Next(0, 828));
                Canvas.SetTop(base.image, -50);
                this.placeY = Canvas.GetTop(base.image);
                base.PlaceX = Canvas.GetLeft(base.image);
            }

        }
        /// <summary>
        /// טיימר שמפעיל את פעולת העדכון מטבע
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveTimer_Tick(object sender, object e)
        {
            UpdateCoin();

        }

        /// <summary>
        /// סוגי המטבעות
        /// </summary>
        public enum CoinType
        {
            gold,silver,bronze
        }
        

    }
}
