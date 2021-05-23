using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using MetricsAgent.MetricsClasses;

namespace MetricsAgent.Repositories
{
    public interface IHddMetricsRepository : IRepository<HddMetric>
    {

    }
    public class HddMetricsRepository : IHddMetricsRepository
    {
        private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";

        public void Create(HddMetric item)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            // создаем команду
            using var cmd = new SQLiteCommand(connection);
            // прописываем в команду SQL запрос на вставку данных
            cmd.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(@value, @time)";

            // добавляем параметры в запрос из нашего объекта
            cmd.Parameters.AddWithValue("@value", item.Value);

            // в таблице будем хранить время в секундах, потому преобразуем перед записью в секунды
            // через свойство
            cmd.Parameters.AddWithValue("@time", item.Time);
            // подготовка команды к выполнению
            cmd.Prepare();

            // выполнение команды
            cmd.ExecuteNonQuery();
        }

        public IList<HddMetric> GetAll()
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);

            // прописываем в команду SQL запрос на получение всех данных из таблицы
            cmd.CommandText = "SELECT * FROM cpumetrics";

            var returnList = new List<HddMetric>();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // пока есть что читать -- читаем
                while (reader.Read())
                {
                    // добавляем объект в список возврата
                    returnList.Add(new HddMetric
                    {
                        Id = reader.GetInt32(0),

                        Value = reader.GetInt32(1),

                        // налету преобразуем прочитанные секунды в метку времени
                        Time = reader.GetInt64(2)
                    });
                }

                return returnList;
            }



        }
        public HddMetric GetById(int id)
        {
            using var connection = new SQLiteConnection(ConnectionString);

            connection.Open();

            using var cmd = new SQLiteCommand(connection);

            cmd.CommandText = "SELECT * FROM cpumetrics WHERE id=@id";

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                // если удалось что то прочитать
                if (reader.Read())
                {
                    // возвращаем прочитанное
                    return new HddMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = reader.GetInt64(2)
                    };
                }
                else
                {
                    // не нашлось запись по идентификатору, не делаем ничего
                    return null;
                }
            }

        }
    }
}
