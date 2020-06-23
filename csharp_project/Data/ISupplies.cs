
namespace csharp_project.Data
{
    interface ISupplies
    {
        #region Public Properties

        int Id { get; set; }

        string Name { get; set; }

        #endregion Public Properties

        #region Public Methods

        string GetInformation();

        #endregion Public Methods
    }
}
