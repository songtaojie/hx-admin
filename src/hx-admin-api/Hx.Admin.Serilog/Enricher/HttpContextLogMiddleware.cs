﻿// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Admin.Serilog.Enricher;
public class HttpContextLogMiddleware
{
    private readonly RequestDelegate _next;

    public HttpContextLogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var serviceProvider = context.RequestServices;
        // 将我们自定义的Enricher添加到LogContext中。
        // LogContext功能很强大，可以动态添加属性，具体使用介绍，参见官方wiki文档
        using (LogContext.Push(new HttpContextEnricher(serviceProvider)))
        {
            await _next(context);
        }
    }
}

//// 使用扩展方法形式注入中间件
//public static IApplicationBuilder UseHttpContextLog(
//    this IApplicationBuilder builder)
//{
//    return builder.UseMiddleware<HttpContextLogMiddleware>();
//}


