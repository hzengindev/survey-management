using Core.Utilities.Serialize;
using System.Runtime.Serialization;

namespace Actions.Models.Base
{
    [DataContract]
    public class BaseDataResult<T> : BaseResult
    {
        [DataMember]
        public T DataResult { get; set; }
        [DataMember]
        public string DataResultSerialize { get => DataResult == null ? string.Empty : Serializer.Serialize<T>(DataResult); }
        
        public BaseDataResult(bool success): base(success)
        {
            this.DataResult = default(T);
        }

        public BaseDataResult(bool success, string message) : base(success, message)
        {
            this.DataResult = default(T);
        }

        public BaseDataResult(bool success, string message, int code) : base(success, message, code)
        {
            this.DataResult = default(T);
        }

        public BaseDataResult(bool success, T data) : base(success)
        {
            this.DataResult = data;
        }

        public BaseDataResult(bool success, string message, T data): base(success, message)
        {
            this.DataResult = data;
        }

        public BaseDataResult(bool success, string message, int code, T data) : base(success, message, code)
        {
            this.DataResult = data;
        }
    }
}