namespace WB.Dapper.Migrations.Contract
{
    /// <summary>
    /// Metadata of migration. Every migration (implementation of <see cref="IMigration"/>) should be decorated by this attribute, otherwise it will be ignored.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MigrationAttribute : Attribute
    {
        /// <summary>
        /// Migration number. This number should be unique for every migration. It's determines the order of migration execution.
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// Short name of migration.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Optional description of migration. This description will be used in the migration log.
        /// </summary>
        public string? Describtion { get; }


        public MigrationAttribute(int number, string name, string? describtion = null)
        {
            Number = number;
            Name = name;
            Describtion = describtion;
        }
    }
}
