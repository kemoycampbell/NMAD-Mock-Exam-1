using System;

namespace Flexify.Exceptions
{
    public class UserException: Exception,IUserErrorException
    {
        private readonly  int _status;
        private readonly  string _message;
        
        
        public UserException(string errorMessage, int statusCode = 400)
            : base(errorMessage)
        {
            _status = statusCode;
            _message = errorMessage;
        }

        
        public int GetStatusCode()
        {
            return _status;
        }

        public string GetMessage()
        {
            return _message;
        }
    }
}