using MySql.Data.MySqlClient;

namespace Datos
{
    public class ConnectionManager
    {

        internal MySqlConnection _connection;
        public ConnectionManager(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
        }
        public void Open()
        {
            _connection.Open();
        }
        public void Close()
        {
            _connection.Close();
        }


    }
}
