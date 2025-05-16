using SQLitePCL;

namespace WB.Dapper.Migrations.Sqlite
{
    internal static class ProviderActivator
    {
        private static bool activated = false;

        public static void Activate()
        {
            if (activated)
                return;

            Batteries.Init();
            activated = true;
        }
    }
}
