using DataBaseProject.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DataBaseProject
{
    /// <summary>
    /// המחלקה תאגד פעולות שתטפל במסד נתונים
    /// </summary>
    
    public static class DataBaseMethods //לא צריך ליצור אוביקט של המחלקה כדי להשתמש בםעולות בה ולכן היא סטטית
    {
        //משתנה סטטי הוא משתנה שקבוע לעצם
        private static string dbPath = ApplicationData.Current.LocalFolder.Path;//משתנה שם מיקום הקובץ
        private static string connectionString = "Filename=" + dbPath + "\\DBGame.db";//משתנה שם קישוריות הקובץ למסד נתונים

        /// <summary>
        /// פעולת קבלת משתמש לפי שם המשתמש והסיסמה שלו
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword">סיסמת המשתמש</param>
        /// <returns>מחזירה משץמש או  null אם המשתמש לא נמצא </returns>
        public static User GetUser(string userName, string userPassword)
        {
            string query = $"SELECT * FROM [Users] WHERE UserName='{userName}' AND Password='{userPassword}'";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open(); //פתיחת חיבור
                SqliteCommand command = new SqliteCommand(query, connection);//הפקודה
                SqliteDataReader reader = command.ExecuteReader(); //ביצוע קריאה
                if (reader.HasRows)
                {
                    reader.Read();
                    User user = new User
                    {
                        Id = reader.GetInt32(0),
                        UserName = reader.GetString(1),
                        Password = reader.GetString(2),
                        CurrentCharacter = reader.GetInt32(3),
                        Coins = reader.GetInt32(4),
                        MaxScore = reader.GetInt32(5),
                        Mail = reader.GetString(6),
                        CurrentBackground = reader.GetInt32(7),
                    };
                    return user;
                }
            }
            return null;
        }
        /// <summary>
        /// קבלת משתמש רק לפי שם המשתמש שלו וללא הסיסמה לצורך ההרשמה לראות שלא קיים עוד שם כזה
        /// </summary>
        /// <param name="userName">שם משתמש</param>
        /// <returns>משתמש</returns>
        public static User GetUserByName(string userName)
        {
            string query = $"SELECT * FROM [Users] WHERE UserName='{userName}'";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open(); //פתיחת חיבור
                SqliteCommand command = new SqliteCommand(query, connection);//הפקודה
                SqliteDataReader reader = command.ExecuteReader(); //ביצוע קריאה
                if (reader.HasRows)
                {
                    reader.Read();
                    User user = new User
                    {
                        Id = reader.GetInt32(0),
                        UserName = reader.GetString(1),
                        Password = reader.GetString(2),
                        CurrentCharacter = reader.GetInt32(3),
                        Coins = reader.GetInt32(4),
                        MaxScore = reader.GetInt32(5),
                        Mail = reader.GetString(6),
                        CurrentBackground = reader.GetInt32(7),
                    };
                    return user;
                }
            }
            return null;
        }

       /// <summary>
       /// ]פעולה שמוסיפה משתמש לדאטא בייס של טבלת המשתמשים באתר ומחזירה את המשתמש במידה והיא כן הוסיפה אותו ואם לא היא תחזיר ריק.
       /// </summary>
       /// <param name="UserName">שם המשתמש של הדמות</param>
       /// <param name="password">הסיסמא של הדמות</param>
       /// <param name="mail">המייל של הדמות</param>
       /// <returns></returns>
        public static User AddUser(string UserName, string password, string mail)
        {
            User user = GetUserByName(UserName);//נסיון שליפת משתמש
            if (user != null)//זאת אומרת שהמשתמש כבר קיים
                return null;//הגעת למקום הלא נכון, אתה משתמש קיים
            //אם אנו ממשיכים זאת אומרת שהמשתמש לא קיים ועלינו להוסיפו למאגר
            string query = $"INSERT INTO [Users] (UserName,Password,CurrentCharacter,Coins,MaxScore,Mail,CurrentBackground) VALUES ('{UserName}','{password}',{1},{100},{0},'{mail}',{1})";
            Execute(query);
            user = GetUser(UserName, password);
            return user;
        }
        /// <summary>
        /// הפעולה מקבלת שאילתה ומבצעת אותה
        /// </summary>
        /// <param name="query">ההוראה לאופן פעולה במאגר נתונים</param>
        private static void Execute(string query)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// הפעולה מחזירה את המוצרים שהמשתמש בעל המספר זהות הספציפי קנה
        /// </summary>
        /// <param name="userId">זהות המשתמש</param>
        /// <returns>הפעולה מחזירה רשימה מסוג קנייה של כל המוצרים אותם המשתמש קנה והם מסודרים לפי המספר הסידורי של המוצרים</returns>
        public static List<Purchase> GetPurchases(int userId)
        {
            List<Purchase> products = new List<Purchase>();
            string query = $"SELECT * FROM Purchase WHERE UserId = {userId} ORDER BY ProductSerialNumber";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(query, connection);
                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Purchase purchase = new Purchase
                        {
                            UserId = reader.GetInt32(0),
                            ProductSetialNumber = reader.GetInt32(1),

                        };
                        products.Add(purchase);
                    }
                }
                return products;
            }
        }
        /// <summary>
        ///  הפעולה מעדכנת את הטבלה של המשתמשים כאשר היא מעדכנת משתמש ספציפי בדמות הנוכחית שלו, בכסף שלו במשחק ואת הרקע הנוכחי שלו
        /// </summary>
        /// <param name="user">המשתמש שאותו מעדכנים בטבלה</param>
        /// <param name="coins">המטבעות שאותם צבר במשחק ומוסיפים אותם למספר הכולל של מטבעות</param>
        /// <returns>הפעולה מחזירה את המשתמש המעודכן</returns>
        public static User UpdateCurrentlyProductAndMoneyAndBackground(User user, int coins)
        {
            string query = $"UPDATE Users SET CurrentCharacter = {user.CurrentCharacter}, Coins = {(user.Coins + coins)}, CurrentBackground = {user.CurrentBackground} WHERE Id = {user.Id}";
            Execute(query);
            return GetUser(user.UserName, user.Password);
        }

        /// <summary>
        /// הפעולהנ מעדכנת את תוצאת השיא נקודות של המשתמש שצבר במשחק אחד
        /// </summary>
        /// <param name="user">  המשתמש שעבר את השיא ויש לעדכן את השיא החדש שלו</param>
        /// <param name="score">מספר הנקודות שצבר במשחק</param>
        /// <returns>משתמש מעודכן עם השיא החדש</returns>
        public static User UpdateCurrentlyHighScore(User user, int score)
        {
            
            string query = $"UPDATE Users SET MaxScore = {score} WHERE Id = {user.Id}";
            Execute(query);
            return GetUser(user.UserName, user.Password);

        }

        /// <summary>
        /// המשתמש רכש מוצר חדש מהחנות והפעולה מוסיפה את המוצר לטבלה עם זהותו של המשתמש
        /// </summary>
        /// <param name="user">המשתמש שרכש את המוצר</param>
        /// <param name="price">מחיר המוצר</param>
        /// <param name="ProductSerialNumber">המספר הסידורי של המוצר</param>
        /// <returns>הפעולה מחזירה את המשתמש</returns>
        public static User AddPurchase(User user, int price, int ProductSerialNumber)
        {
            if (IsExistInPurchases(user.Id, ProductSerialNumber))
                return GetUser(user.UserName, user.Password);
            string query = $"INSERT INTO [Purchase] (UserId,ProductSerialNumber) VALUES ({user.Id},{ProductSerialNumber})";
            Execute(query);
            query = $"UPDATE Users SET Coins = {user.Coins- price} WHERE Id = {user.Id}";
            Execute(query);
            return GetUser(user.UserName, user.Password);
        }

        /// <summary>
        /// הפעולה מקבהלת מספר סידורי של מוצר ובודקת האם למשתמש יש את המוצר הזה או שלא
        /// </summary>
        /// <param name="UserId">מספר זהות המשתמש </param>
        /// <param name="ProductSerialNumber">מספר סידורי של המוצר</param>
        /// <returns>הפעולה מחזירה האם המוצר קיים או לא</returns>
        private static bool IsExistInPurchases(int UserId, int ProductSerialNumber)
        {
            string query = $"SELECT * FROM [Purchase] WHERE UserId={UserId} AND ProductSerialNumber={ProductSerialNumber}";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(query, connection);
                SqliteDataReader reader = command.ExecuteReader();
                return reader.HasRows;//האם קיים

            }
        }

        /// <summary>
        /// הפעולה מחזירה רשימה מסוג מוצרים שכוללת את כל המוצרים בחנות אשר מסודרת לפי המספר הסידורי של המוצרים 
        /// </summary>
        /// <returns>רשימה מסוג מוצר</returns>
        public static List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            string query = $"SELECT * FROM Store ORDER BY ProductId";
            using(SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(query, connection);
                SqliteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            ProductSerialNumber = reader.GetInt32(0),
                            Price = reader.GetInt32(1),
                        };
                        products.Add(product);
                    }
                }
                
            }
            return products;
        }

        /// <summary>
        /// פעולה שמחזירה רשימה מסוג משתמש אשר מכילה את כל המשתמשים המשחק מסודרים לפי התוצאה הגבוהה  ביותר שהשיגו
        /// </summary>
        /// <returns>רשימה מסוג משתמש</returns>
        public static List<User> GetUsersSortMaxScore()
        {
            string query = $"SELECT * FROM Users ORDER BY MaxScore";
            List<User> users = new List<User>();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(query, connection);
                SqliteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        User user = new User
                        {
                            Id = reader.GetInt32(0),
                            UserName = reader.GetString(1),
                            Password = reader.GetString(2),
                            CurrentCharacter = reader.GetInt32(3),
                            Coins = reader.GetInt32(4),
                            MaxScore = reader.GetInt32(5),
                            Mail = reader.GetString(6),
                            CurrentBackground = reader.GetInt32(7),
                        };
                        users.Add(user);
                    }   
                }
            }
            return users;
        }

        /// <summary>
        /// הפעולה מחזירה את המשתמש לפי שם המשתמש והמייל שלו שהמשתמש הזה רוצה לדעת מה הסיסמה שלו והוא שכח
        /// </summary>
        /// <param name="userName">שם המשתמש</param>
        /// <param name="userMail">מייל המשתמש</param>
        /// <returns>המשתמש המבוקש</returns>
        public static User GetUserForgotPassword(string userName, string userMail)
        {
            string query = $"SELECT * FROM [Users] WHERE UserName='{userName}' AND Mail ='{userMail}'";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {

                connection.Open(); //פתיחת חיבור
                SqliteCommand command = new SqliteCommand(query, connection);//הפקודה
                SqliteDataReader reader = command.ExecuteReader(); //ביצוע קריאה
                if (reader.HasRows)
                {
                    reader.Read();
                    User user = new User
                    {
                        Id = reader.GetInt32(0),
                        UserName = reader.GetString(1),
                        Password = reader.GetString(2),
                        CurrentCharacter = reader.GetInt32(3),
                        Coins = reader.GetInt32(4),
                        MaxScore = reader.GetInt32(5),
                        Mail = reader.GetString(6),
                        CurrentBackground = reader.GetInt32(7),
                    };
                    return user;
                }
            }
            return null;
        }

        /// <summary>
        /// פעולה שמעדכנת סיסמה חדשה למשתמש ומחזירה את המשתמש המעודכן
        /// </summary>
        /// <param name="user">המשתמש שרוצים לעדכן את סיסמתו</param>
        /// <param name="NewPass">הסיסמה החדשה</param>
        /// <returnsמשתמש מעודכן></returns>
        public static User ChangePassword(User user, string NewPass)
        {
            string query = $"UPDATE Users SET Password = {NewPass} WHERE Id = {user.Id}";
            Execute(query);
            return GetUser(user.UserName, NewPass);
        }

    }

    
}
