﻿namespace KadoshShared.ValueObjects
{
    public class Error : ValueObject
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public Error(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
