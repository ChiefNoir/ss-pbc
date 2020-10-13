namespace Abstractions.Model.System
{
    public class Identity
    {
        public Account Account { get; set; }

        /// <summary> Account identification  token </summary>
        public string Token { get; set; }

        /// <summary> Tokens lifespan </summary>
        public int TokenLifeTimeMinutes { get; set; }
    }
}
