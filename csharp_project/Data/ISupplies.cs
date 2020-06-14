
namespace csharp_project.Data
{
    interface ISupplies
    {
        int Id { get; set; }
        string Name { get; set; }
        string GetInformation();
    }
}
