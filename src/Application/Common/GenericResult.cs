namespace Application.Common;

public record GenericResult<T> : Result
{
	public T Value => IsSuccess
		? _value!
		: throw new InvalidOperationException("Значение не может быть получено при наличии ошибки");

	private readonly T? _value;

	protected GenericResult(T value) : base(true)
	{
		_value = value;
	}

	protected GenericResult(Error error) : base(error) { }

	public static GenericResult<T> Success(T value) => new GenericResult<T>(value);
	public new static GenericResult<T> Failure(Error error) => new GenericResult<T>(error);
}
