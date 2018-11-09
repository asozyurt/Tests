namespace HeadlessBrowser.Common.Message
{
    public class ResponseMessage : BaseMessage
    {
        public bool IsError { get; private set; }
        public object Result { get; }

        public ResponseMessage()
        {
        }
        public ResponseMessage(object result)
        {
            Result = result;
        }

        public static ResponseMessage Ok() { return new ResponseMessage(null); }
        public static ResponseMessage Ok(object result) { return new ResponseMessage(result); }
        public static ResponseMessage NOk() { return new ResponseMessage(null) { IsError = true }; }
    }
}
