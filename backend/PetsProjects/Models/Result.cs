namespace PetsProjects.Models
{
    public class Result<TValue>
    {
        public bool IsSuccess { get; }

        public TValue? Value { get; }

        public string? Error { get; }

        private Result(TValue value)
        {
            IsSuccess = true;
            Value = value;
            Error = null;
        }
        private Result(string error)
        {
            IsSuccess = false;
            Value = default;
            Error = error;
        }

        public static Result<TValue> Success(TValue value) => new(value);
        public static Result<TValue> Failure(string error) => new(error);

        public static implicit operator Result<TValue>(TValue value)
        {
            return new (value);
        }
      
    }
}
