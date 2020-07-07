using System.Threading.Tasks;

namespace Authin.Core.Api.Request
{
    interface IExecutable<T>
    {
        Task<T> Execute();
    }
}
