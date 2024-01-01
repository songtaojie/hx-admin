﻿using Hx.Admin.Models;
using Hx.Admin.Models.ViewModels.Menu;
using Hx.Common.DependencyInjection;

namespace Hx.Admin.IService;

/// <summary>
/// 系统用户服务
/// </summary>
public interface ISysUserService : IBaseService<SysUserRole>, IScopedDependency
{
    /// <summary>
    /// 获取用户分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedListResult<SysUser>> GetPage(PageUserInput input);

    /// <summary>
    /// 增加用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task AddUser(AddUserInput input);

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UpdateUser(UpdateUserInput input);

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task DeleteUser(DeleteUserInput input);

    /// <summary>
    /// 查看用户基本信息
    /// </summary>
    /// <returns></returns>
    Task<SysUser> GetBaseInfo();

    /// <summary>
    /// 更新用户基本信息
    /// </summary>
    /// <returns></returns>
    Task<int> UpdateBaseInfo(SysUser user);

    /// <summary>
    /// 设置用户状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<int> SetStatus(UserInput input);

    /// <summary>
    /// 授权用户角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task GrantRole(UserRoleInput input);

    /// <summary>
    /// 修改用户密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<int> ChangePwd(ChangePwdInput input);

    /// <summary>
    /// 重置用户密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<int> ResetPwd(ResetPwdUserInput input);

    /// <summary>
    /// 获取用户拥有角色集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IEnumerable<long>> GetOwnRoleList(long userId);

    /// <summary>
    /// 获取用户扩展机构集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IEnumerable<SysUserExtOrg>> GetOwnExtOrgList(long userId);
}