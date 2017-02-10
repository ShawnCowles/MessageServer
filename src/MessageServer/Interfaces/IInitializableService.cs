namespace MessageServer.Interfaces
{
    /// <summary>
    /// Any service that can be initialized.
    /// </summary>
    public interface IInitializableService
    {
        /// <summary>
        /// Initialize the service.
        /// </summary>
        void Initialize();
    }
}
