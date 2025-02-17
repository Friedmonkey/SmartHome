using SmartHome.UI.Layout;

namespace SmartHome.UI
{
    public class NavigationController
    {
        public static NavMenu navigationMenu;

        public static string HomeName;

        public static void RefreshMenu()
        {
            //Ververs de pagina bijvoorbeeld wanneer de ingestelde home word gewijzigd
            navigationMenu.Refresh();
        }
    }
}
