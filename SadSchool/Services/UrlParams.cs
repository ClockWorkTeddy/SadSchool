namespace SadSchool.Services
{
    /// <summary>
    /// Url parameters for the navigation service.
    /// </summary>
    public struct UrlParams
    {
        /// <summary>
        /// Gets or sets the controller name.
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Gets or sets the action name.
        /// </summary>
        public string Action { get; set; }
    }
}
