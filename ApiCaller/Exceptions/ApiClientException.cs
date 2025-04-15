using System;
using System.Net;

namespace ApiCaller.Exceptions;

public class ApiClientException(string message, HttpStatusCode responseStatusCode) : Exception(message);