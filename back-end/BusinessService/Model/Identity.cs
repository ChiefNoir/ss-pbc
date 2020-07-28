namespace API.Model
{
    /// <summary> Account confirmed identity</summary>
    internal class Identity
    {
        /// <summary> Account login </summary>
        public string Login { get; set; }

        /// <summary> Account identification  token </summary>
        public string Token { get; set; }

        /// <summary> Tokens lifespan </summary>
        public int TokenLifeTimeMinutes { get; set; }
    }
}