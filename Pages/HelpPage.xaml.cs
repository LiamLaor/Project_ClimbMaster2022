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
    public sealed partial class HelpPage : Page
    {
        public User user = null;//עצם מסוג משתמש
        /// <summary>
        /// הפעולה בונה שאחראית על ייצוג הדף ומציגה את הרכיבים על המסך
        /// </summary>
        public HelpPage()
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
        /// פעולה של חזרה למסך הבית
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MenuPage),this.user);

        }
    }
}
