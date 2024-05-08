﻿// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.IService;
using Hx.Admin.Models.ViewModels.Notice;
using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.OnlineUser;

namespace Hx.Admin.Web.Entry.Controllers;

/// <summary>
/// 在线用户
/// </summary>
public class SysOnlineUserController : AdminControllerBase
{
    private readonly ISysOnlineUserService _service;

    /// <summary>
    /// <see cref="SysOnlineUserController"/>
    /// </summary>
    /// <param name="service"></param>
    public SysOnlineUserController(ISysOnlineUserService service)
    {
        _service = service;
    }

    /// <summary>
    /// 获取在线用户分页列表数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedListResult<SysOnlineUser>> GetPage([FromQuery] PageOnlineUserInput input)
    {
        return await _service.GetPage(input);
    }

    /// <summary>
    /// 强制下线
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task ForceOffline(SysOnlineUser user)
    {
        await _service.ForceOffline(user);
    }
}