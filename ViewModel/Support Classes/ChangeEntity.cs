using Microsoft.Data.SqlClient;
using Model.Entitys;
using System.Data.Sql;
using System.Text;

namespace ViewModel
{
    public enum DbAction
    {
        Insert,         // הכנסת רגילה 
        InsertChild,    // הכנסת ילד בלבד - שלב ב' של הכנסה בהורשה
        InsertFather,   // הכנסת ילד בלבד - שלב א' של הכנסה בהורשה
        Update,         // עדכון רגילה / עדכון ילד בלבד
        UpdateFather,   // עדכון אב בלבד
        Delete,         // מחיקה רגילה / מחיקת ילד בלבד
        DeleteFather,   // מחיקת אב בלבד
    }

    public class ChangeEntity
    {
        // שימוש ב-Properties קצרים ונקיים (Auto-implemented Properties)
        public BaseEntity Entity { get; set; }
        public DbAction Action { get; set; }

        public ChangeEntity(BaseEntity entity, DbAction action)
        {
            Entity = entity;
            Action = action;
        }
    }
}
