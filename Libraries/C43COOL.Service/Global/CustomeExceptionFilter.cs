using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Global
{
    public class CustomeExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
                string msg = context.Exception.Message;
                if (context.Exception is InternationalException)
                {
                    var exception = (InternationalException)context.Exception;
                    var result = new ObjectResult(new ErrorResultModel
                    {
                        Code = exception.Code ?? context.Exception.GetType().Name,
                        Message = exception.Message,
                    });
                    result.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Result = result;
                }
                else
                {
                    var exception = context.Exception;
                    var result = new ObjectResult(new ErrorResultModel
                    {
                        Code = context.Exception.GetType().Name,
                        Message = exception.Message,
                    });
                    result.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Result = result;
                }
            }
            context.ExceptionHandled = true; //异常已处理了

            return Task.CompletedTask;
        }
    }
    public class ErrorResultModel
    {
        public string Message { get; set; }
        public string Code { get; set; }
        public object Data { get; set; }
    }
}
