using System.Threading.Tasks;

namespace ConsoleWorker
{
    public interface IWorker
    {
        Task Start();
    }
}
