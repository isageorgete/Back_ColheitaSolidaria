using System;

namespace YourProjectNamespace.Exceptions
{
	public class ApiException : Exception
	{
		public int StatusCode { get; }
		public string Title { get; }

		public ApiException(string message, int statusCode = 400, string title = null) : base(message)
		{
			StatusCode = statusCode;
			Title = title;
		}
	}
}
