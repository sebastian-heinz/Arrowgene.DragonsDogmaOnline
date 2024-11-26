using Arrowgene.Ddon.Shared.Model;

[System.Serializable]
public class ResponseErrorException : System.Exception
{
    public ErrorCode ErrorCode { get; }

    public ResponseErrorException() { ErrorCode = ErrorCode.ERROR_CODE_FAIL; }
    public ResponseErrorException(ErrorCode errorCode) { ErrorCode = errorCode;}
    public ResponseErrorException(ErrorCode errorCode, string message) : base(message) { ErrorCode = errorCode; }
    public ResponseErrorException(ErrorCode errorCode, string message, System.Exception inner) : base(message, inner) { ErrorCode = errorCode; }
    protected ResponseErrorException(
        ErrorCode errorCode,
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
        ErrorCode = errorCode;
    }
}