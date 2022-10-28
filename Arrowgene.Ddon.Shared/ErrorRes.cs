using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared;

public struct ErrorRes<T>
{
    public static readonly ErrorRes<T> Fail = new();

    public static ErrorRes<T> Error(ErrorCode errorCode)
    {
        return new ErrorRes<T>(errorCode);
    }

    public static ErrorRes<T> Success(T value)
    {
        return new ErrorRes<T>(value);
    }

    private ErrorRes(ErrorCode errorCode = ErrorCode.ERROR_CODE_FAIL)
    {
        Value = default;
        HasValue = false;
        ErrorCode = errorCode;
    }

    private ErrorRes(T value, ErrorCode errorCode = ErrorCode.ERROR_CODE_SUCCESS)
    {
        Value = value;
        HasValue = true;
        ErrorCode = errorCode;
    }

    public bool HasError => ErrorCode != ErrorCode.ERROR_CODE_SUCCESS;
    public bool HasValue { get; }
    public ErrorCode ErrorCode { get; }
    public T Value { get; }
}
