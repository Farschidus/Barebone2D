using System.Threading.Tasks;

namespace Barebone.Contracts;

public interface IEvent
{
    Task Execute(); // Ensure async execution for flexibility
}