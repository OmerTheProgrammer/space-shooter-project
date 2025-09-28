using Microsoft.Data.SqlClient;
using Model.Entitys;
using System.Data.Sql;
using System.Text;

namespace ViewModel
{
    public class ChangeEntity
    {
        private BaseEntity entity;
        private CreateSql createSql;

        public ChangeEntity(CreateSql createSql, BaseEntity entity)
        {
            this.createSql = createSql;
            this.entity = entity;
        }

        public BaseEntity Entity { get => entity; set => entity = value; }
        public CreateSql CreateSql { get => createSql; set => createSql = value; }
    }
    public delegate void CreateSql(BaseEntity entity, SqlCommand command);
}
