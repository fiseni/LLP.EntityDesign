using System.Threading.Tasks;

namespace LLP.EntityDesign.API.Contracts
{
    public interface IDocumentNoGenerator
    {
        Task<string> GetNewOrderNo();
    }
}
