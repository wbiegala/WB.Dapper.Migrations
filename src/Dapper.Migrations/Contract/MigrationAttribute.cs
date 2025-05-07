namespace WB.Dapper.Migrations.Contract
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MigrationAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
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
