using Arrowgene.Ddon.Shared.Model;

[System.Serializable]
public class ResponseErrorException : System.Exception
{
    public ErrorCode ErrorCode { get; }
    public bool Critical { get; }

    public ResponseErrorException() { ErrorCode = ErrorCode.ERROR_CODE_FAIL; }
    public ResponseErrorException(ErrorCode errorCode, bool critical = false) { ErrorCode = errorCode; Critical = critical; }
    public ResponseErrorException(ErrorCode errorCode, string message, bool critical = false) : base(message) { ErrorCode = errorCode; Critical = critical; }
    public ResponseErrorException(ErrorCode errorCode, string message, System.Exception inner, bool critical = false) : base(message, inner) { ErrorCode = errorCode; Critical = critical; }
    protected ResponseErrorException(
        ErrorCode errorCode,
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context, 
        bool critical = false) : base(info, context)
    {
        ErrorCode = errorCode;
        Critical = critical;
    }
}
