namespace monsterCompendium
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Registrar rutas para las páginas de detalles de monstruos
            Routing.RegisterRoute("monsterdetails", typeof(Views.MonsterDetailPage));
        }
    }
}
