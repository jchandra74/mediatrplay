namespace MyTodos;

public readonly struct Result<TValue,TError>
{
    public bool IsError { get; }

    public bool IsSuccess => !IsError;

    private readonly TValue? _value;
    private readonly TError? _error;

    public Result(TValue value)
    {
        IsError = false;
        _value = value;
        _error = default;        
    }

    public Result(TError error)
    {
        IsError = true;
        _value = default;
        _error = error;
    }

    public static implicit operator Result<TValue, TError>(TValue value) => new Result<TValue, TError>(value);
    public static implicit operator Result<TValue, TError>(TError error) => new Result<TValue, TError>(error);

    public TResult Match<TResult>(
        Func<TValue,TResult> f0,
        Func<TError,TResult> f1) 
    {
        if (!IsError && f0 != null)
        {
            return f0(_value!);
        }

        if (IsError && f1 != null)
        {
            return f1(_error!);
        }

        throw new InvalidOperationException();
    }

    public void Switch(
        Action<TValue> a0,
        Action<TError> a1)
    {
        if (!IsError && a0 != null)
        {
            a0(_value!);
            return;
        }

        if (IsError && a1 != null)
        {
            a1(_error!);
            return;
        }

        throw new InvalidOperationException();
    }
}