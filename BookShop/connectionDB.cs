//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Text;
//using System.Threading.Tasks;
//using Dapper;

//namespace BookShop
//{
//    public class ProductRepository
//    {
//        public interface IDatabaseConnectionFactory
//        {
//            IDbConnection GetConnection();
//        }
//        private readonly IDatabaseConnectionFactory connectionFactory;

//        public ProductRepository(IDatabaseConnectionFactory connectionFactory)
//        {
//            this.connectionFactory = connectionFactory;
//        }

//        public Task<IEnumerable<UInt16>> GetAll()
//        {
//            return this.connectionFactory.GetConnection().QueryAsync<User>("select * from User");
//        }
//    }
//    public class InMemoryDatabase
//    {
//        private readonly OrmLiteConnectionFactory dbFactory =
//            new OrmLiteConnectionFactory(":memory:", SqliteOrmLiteDialectProvider.Instance);

//        public IDbConnection OpenConnection() => this.dbFactory.OpenDbConnection();

//        public void Insert<T>(IEnumerable<T> items)
//        {
//            using (var db = this.OpenConnection())
//            {
//                db.CreateTableIfNotExists<T>();
//                foreach (var item in items)
//                {
//                    db.Insert(item);
//                }
//            }
//        }
//    }
//}
