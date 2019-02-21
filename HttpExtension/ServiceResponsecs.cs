using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HttpExtension
{
    public sealed class ServiceResponse<T>
    {
        /// <summary>
        /// Gets the status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Gets the content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets the error.
        /// </summary>
        public Exception Error { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="statusCode">
        /// The status code.
        /// </param>
        public ServiceResponse(T value, HttpStatusCode statusCode)
        {
            this.Value = value;
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class.
        /// </summary>
        /// <param name="statusCode">
        /// The status code.
        /// </param>
        /// <param name="error">
        /// The error.
        /// </param>
        public ServiceResponse(HttpStatusCode statusCode, Exception error = null)
        {
            this.StatusCode = statusCode;
            this.Error = error;
        }
    }
