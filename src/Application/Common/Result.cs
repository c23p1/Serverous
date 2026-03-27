namespace Application.Common;

public record Result
{
	public bool IsSuccess { get; }
	public Error? Error { get; }

	protected Result(bool isSuccess)
	{
		IsSuccess = isSuccess;
	}

	protected Result(Error error)
	{
		IsSuccess = false;
		Error = error;
	}

	public static Result Success() => new Result(true);
	public static Result Failure() => new Result(false);
	public static Result Failure(Error error) => new Result(error);
}
