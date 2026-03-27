namespace Application.Common;

public sealed record Error
{
	public ErrorType ErrorType { get; }
	public string Title { get; } = default!;
	public string Message { get; } = default!;

	private Error(ErrorType errorType, string title, string message)
	{
		Title = title;
		ErrorType = errorType;
		Message = message;
	}

	public static Error Validation(string message) => new Error(ErrorType.Validation, "Ошибка валидации", message);
	public static Error InvalidOperation(string message) => new Error(ErrorType.InvalidOperation, "Недопустимая операция", message);
	public static Error Unauthorized(string message) => new Error(ErrorType.Unauthorized, "Ошибка авторизации", message);
	public static Error EntityNotFound(string entityName, string id) => new Error(ErrorType.NotFound, "Сущность не найдена", $"Сущность ({entityName}) с Id ({id}) не найдена");
	public static Error ResourceNotFound(Uri resourceUri) => new Error(ErrorType.NotFound, "Ресурс не найден", $"Ресурс с идентификатором ({resourceUri}) не найден");
	public static Error Unexpected(string message) => new Error(ErrorType.Unexpected, "Произошла непредвиденная ошибка", message);
}
