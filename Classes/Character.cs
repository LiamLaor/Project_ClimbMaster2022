using FinalProjectV1.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace FinalProjectV1.Classes
{
    class Character : BaseClass
    {
        public double Accelaration { get; set; }//תאוצה
        public StateType state { get; set; }// מצב הדמות
        public double baseSpeed { get; set; }// מהירות של העצם עליו הדמות נמצאת בציר איקס כלומר אם הדמות על מדרגה זזה אז זוהי המהירות שלה
        public bool IsMusicOn { get; set; }//המוזיקה במשחק
        public bool checkfirst { get; set; }//המשטח הבסיסי שכל עוד הדמות לא קפצה על מדרגה היא לא יכולה להיפסל וליפול מתחת לקאנבאס

        /// <summary>
        /// פעולה בונה עצם מסוג דמות אשר יורשת מהמחלקה baseclass
        /// </summary>
        /// <param name="placeX"מיקום הדמות בציר איקס></param>
        /// <param name="placeY">מיקום הדמות בציר וואי</param>
        /// <param name="arena">משטח המשחק</param>
        /// <param name="Width">רוחב הדמות</param>
        /// <param name="Height">גובה הדמות</param>
        public Character(double placeX, double placeY, Canvas arena, double Width, double Height) :base(placeX, placeY, arena, Width, Height)
        {
            this.checkfirst = true;
            this.baseSpeed = 0;
            this.Accelaration = 0;
            base.SpeedX = 0;
            base.SpeedY = 0;
            this.IsMusicOn = true;
        }

        /// <summary>
        /// טיימר אשר מעדכן את מיקום הדמות ובודק שהיא לא יוצאת מתחומי המשחק
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void MoveTimer_Tick(object sender, object e)
        {
            if (this.PlaceX > 0 && this.PlaceX < (this.arena.ActualWidth - 100))
            {
                base.MoveTimer_Tick(sender, e);
                this.SpeedY += Accelaration;
                if (this.placeY >= arena.ActualHeight - 100 && checkfirst)//בודק את הרצפה למטה בהתחלה שהדמות לא תיפול
                {
                    SpeedY = 0;
                    Accelaration = 0;
                    state = StateType.idle;
                }
            }
            else if (this.PlaceX <= 0)//בדיקה שהדמות לא יוצאת מתחומי המשחק
                this.PlaceX = 1;
            else if (this.PlaceX >= (this.arena.ActualWidth - 100))
                this.PlaceX = (this.arena.ActualWidth - 101);
            if (this.placeY <= 0)
            {
                this.placeY = 1;
                this.SpeedY = 1.5;
            }
           



        }
        /// <summary>
        /// פעולה שמתאימה את הגיפים של הדמות והיא וירטואל כלומר ניתנת לדריסה
        /// </summary>
        public virtual void MatchGif()
        {

        }

        /// <summary>
        /// פעולה שמשמנה את מצב הדמות לעמידה, מתאימה את הגיפים ואת מהירותו 
        /// </summary>
        public virtual void GoIdle()
        {
            this.state = StateType.idle;
            MatchGif();
            base.SpeedX = baseSpeed;
        }

        /// <summary>
        /// פעולה שמשנה את מצב הדמות לעמידה, מתאימה את הגיפים שמאלה ואת מהירותו
        /// </summary>
        public virtual void GoIdleLeft()
        {
            this.state = StateType.StandLeft;
            MatchGif();
            base.SpeedX = baseSpeed;
        }

        /// <summary>
        /// פעולה שמשנה את מצב הדמות לריצה שמאלה, מתאימה את הגיפים ואת מהירותו של הדמות. 
        /// הפעולה בודקת אם הדמות סנטה או דרקון ומשנה את המהירות בהתראם ליכולותיו המיוחדות
        /// </summary>
        /// <param name="CheckSantaAndDragon"></param>
        public virtual void GoLeft(bool CheckSantaAndDragon)
        {

            if (this.state != StateType.runLeft && this.state != StateType.JumpRight && this.state != StateType.JumpLeft)
            {
                this.state = StateType.runLeft;
                MatchGif();
                if (CheckSantaAndDragon)
                    base.SpeedX = -19 + baseSpeed;
                else
                    base.SpeedX = -11 + baseSpeed;
            }
        }

        /// <summary>
        /// פעולה שמופעלת כאשר הדמות נעה ימינה.
        /// , היא מפעילה פעולה שמעדכנת גיפים ובודקת אם הדמות היא סנטה או דרקון
        /// </summary>
        /// <param name="CheckSantaAndDragon"></param>
        public virtual void GoRight(bool CheckSantaAndDragon)
        {
            if (this.state != StateType.runRight&&this.state!=StateType.JumpRight&& this.state != StateType.JumpLeft)
            {
                this.state = StateType.runRight;
                MatchGif();
                if(CheckSantaAndDragon)//בדיקה אם הדמות של המשתמש היא סנטה
                    base.SpeedX = 19 + baseSpeed;
                else
                    base.SpeedX = 11 + baseSpeed;
            }
        }

        /// <summary>
        /// הפעולה מחזירה מלבן סביב הדמות
        /// </summary>
        /// <returns></returns>
        public override Rect GetRectangle()
        {
            return new Rect(this.PlaceX, this.image.Height-10, this.image.Width, this.image.Height);
        }

        /// <summary>
        /// הפעולה בודקת האם מצבה הנוכחי של הדמות הוא קפיצה ואז היא לא תקפוץ. 
        /// אם לא אז הדמות תבדוק אם היא על המדרגה ואם הכל תקין בתנאים הפעולה תעדכן את המהירות של 
        /// הדמות בציר איקס ווואי ותבדוק האם יש דמות מיוחדת שיש לה יכולות מיוחדות של קפיצה
        /// </summary>
        /// <param name="isOnStair"></param>
        /// <param name="CheckCharacter"></param>
        public virtual void Jump(bool isOnStair, bool CheckCharacter)
        {
            if (Accelaration == 0)
            {
                if (isOnStair || (this.state != StateType.JumpLeft && this.state != StateType.JumpRight && this.state != StateType.JumpUp))
                {
                    if (this.state == StateType.runLeft)
                    {
                        this.state = StateType.JumpLeft; //שינוי מצב לקפיצה
                        if (CheckCharacter)//בדיקה האם דמות השחקן היא רובוט או דרקון ואז המהירות בציר וואי תהיה גדולה יותר
                            base.SpeedY = -25.5;
                        else//אם דמות השחקן היא לא רובוט או דרקון
                            base.SpeedY = -17.5;

                        this.SpeedX = -9;
                        MatchGif(); //התאמנו גיף מתאים
                        this.Accelaration = 0.8;
                    }
                    else if ((this.state == StateType.StandLeft || this.state == StateType.idle))
                    {
                        if (CheckCharacter)//בדיקה האם דמות השחקן היא רובוט או דרקון ואז ההמהירות בציר וואי תהיה גדולה יותר
                            base.SpeedY = -20.5;
                        else//אם דמות השחקן היא לא רובוט או דרקון
                            base.SpeedY = -17.5;
                        base.SpeedX = 0;
                        this.state = StateType.JumpUp;
                        this.Accelaration = 0.8;

                    }
                    else if (this.state == StateType.runRight)
                    {
                        this.state = StateType.JumpRight;
                        base.SpeedX = 9;
                        MatchGif();
                        this.Accelaration = 0.8;
                        if (CheckCharacter)//בדיקה האם דמות השחקן היא רובוט או דרקון ואז ההמהירות בציר וואי תהיה גדולה יותר
                            base.SpeedY = -20.5;
                        else//אם דמות השחקן היא לא רובוט או דרקון
                            base.SpeedY = -17.5;
                    }
                }
            }
        }
        /// <summary>
        /// סוגי מצבי הדמות
        /// </summary>
        public enum StateType
        {
            idle, runRight, runLeft, StandLeft, JumpLeft, JumpRight, JumpUp
        }
    }
}
