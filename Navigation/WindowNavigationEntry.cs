using Prism.Regions;
using System.Windows;

namespace PrismWindowNavExample.Navigation
{
    public class WindowNavigationEntry
    {
        public Window Window { get; private set; }
        public NavigationContext Context { get; private set; }

        public WindowNavigationEntry(Window window, NavigationContext context)
        {
            Window = window;
            Context = context;
        }
    }
}
