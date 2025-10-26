using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public class RequestData :BaseEntity
    {
        private ProfileEditRequest request;
        private string field = "";
        private string oldValue = "";
        private string newValue = "";

        public ProfileEditRequest Request { get => request; set => request = value; }
        public string Field { get => field; set => field = value; }
        public string OldValue { get => oldValue; set => oldValue = value; }
        public string NewValue { get => newValue; set => newValue = value; }

        public override string ToString()
        {
            return $"{base.ToString()} " +
                $"Connected Request : {this.Request}," +
                $"Field: {this.Field}, " +
                $"Old Value: {this.OldValue}, " +
                $"New Value: {this.NewValue},";
        }
    }
}
