    using System.Threading.Tasks;

namespace Authin.Api.Sdk.Request
{
    interface IExecutable<T>
    {
        Task<T> Execute();

        T ExecuteSync();
    }
}
