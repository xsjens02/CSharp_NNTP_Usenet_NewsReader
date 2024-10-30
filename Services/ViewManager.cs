using System.Windows;
using UsenetProgram.Views;

namespace UsenetProgram.Services
{
    public abstract class ViewManager
    {
        private static Dictionary<ViewType, Window> Views = new();

        static ViewManager()
        {
            Views.Add(ViewType.LOGIN, new LoginView());
            Views.Add(ViewType.NEWSFEED, new NewsFeedView());
        }

        public static void OpenWindow(ViewType view)
        {
            Window newView = Views[view];
            newView.Show();
        }
    }
}
