using ServerDB.Repositories;

namespace DBTester
{
    internal class DBTester
    {
        static void Main(string[] args, IRoomRepository repository)
        {
            IRoomRepository _rooms = repository;
        }
    }
}
