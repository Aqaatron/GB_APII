using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using MetricsAgent.DAL.MetricsClasses;
using MetricsAgent.DAL.Interfaces;
using Dapper;

namespace MetricsAgent.DAL.Repositories
{
    public interface IDotNetMetricsRepository : IRepository<DotNetMetric>, IDatabaseSettings
    {

    }
    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        private string ConnectionString;

        public DotNetMetricsRepository()
        {
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }

        public void getConnectionString()
        {
            ConnectionString = ConnectionStringClass.ConnectionString;
        }
        public void Create(DotNetMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                //  запрос на вставку данных с плейсхолдерами для параметров
                connection.Execute("INSERT INTO dotnetmetrics(value, time) VALUES(@value, @time)",
                    // анонимный объект с параметрами запроса
                    new
                    {
                        // value подставится на место "@value" в строке запроса
                        // значение запишется из поля Value объекта item
                        value = item.Value,

                        // записываем в поле time количество секунд
                        time = item.Time
                    });
            }
        }

        public IList<DotNetMetric> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            long fromTimeUnixTime = fromTime.ToUnixTimeSeconds();

            long toTimeUnixTime = toTime.ToUnixTimeSeconds();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                // читаем при помощи Query и в шаблон подставляем тип данных
                // объект которого Dapper сам и заполнит его поля
                // в соответсвии с названиями колонок
                return connection.Query<DotNetMetric>("SELECT *FROM dotnetmetrics WHERE time >= @fromTimeUnixTime AND time <= @toTimeUnixTime").ToList();
            }
        }

        public DotNetMetric GetById(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QuerySingle<DotNetMetric>("SELECT Id, Time, Value FROM dotnetmetrics WHERE id=@id",
                    new { id = id });
            }


        }

    }
}
