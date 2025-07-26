using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RinhaDeBackend.Domain
{
    public class Result<T, E>
    {
        public bool IsSuccess { get; set; }
        public E? ErrorValue { get; set; }
        public T? Value { get; set; }

        public static Result<T, E> Success(T value)
        {
            return new Result<T, E> { IsSuccess = true, Value = value };
        }

        public static Result<T, E> Error(E error)
        {
            return new Result<T, E> { IsSuccess = false, ErrorValue = error };
        }

        public static implicit operator Result<T, E>(T value)
        {
            return Success(value);
        }

        public static implicit operator Result<T, E>(E error)
        {
            return Error(error);
        }
    }
}
