/* tslint:disable */
/* eslint-disable */
/**
 * 所有接口
 * 让 .NET 开发更简单、更通用、更流行。前后端分离架构(.NET6/Vue3)，开箱即用紧随前沿技术。<br/><a href='https://gitee.com/zuohuaijun/Admin.NET/'>https://gitee.com/zuohuaijun/Admin.NET</a>
 *
 * OpenAPI spec version: 1.0.0
 * Contact: 515096995@qq.com
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */
import { DbType } from './db-type';
import { StatusEnum } from './enums/status-enum';
import { TenantTypeEnum } from './tenant-type-enum';
/**
 * 
 * @export
 * @interface UpdateTenantInput
 */
export interface UpdateTenantInput {
    /**
     * 雪花Id
     * @type {number}
     * @memberof UpdateTenantInput
     */
    id?: number;
    /**
     * 创建时间
     * @type {Date}
     * @memberof UpdateTenantInput
     */
    createTime?: Date | null;
    /**
     * 更新时间
     * @type {Date}
     * @memberof UpdateTenantInput
     */
    updateTime?: Date | null;
    /**
     * 创建者Id
     * @type {number}
     * @memberof UpdateTenantInput
     */
    createUserId?: number | null;
    /**
     * 修改者Id
     * @type {number}
     * @memberof UpdateTenantInput
     */
    updateUserId?: number | null;
    /**
     * 软删除
     * @type {boolean}
     * @memberof UpdateTenantInput
     */
    isDelete?: boolean;
    /**
     * 用户Id
     * @type {number}
     * @memberof UpdateTenantInput
     */
    userId?: number;
    /**
     * 机构Id
     * @type {number}
     * @memberof UpdateTenantInput
     */
    orgId?: number;
    /**
     * 主机
     * @type {string}
     * @memberof UpdateTenantInput
     */
    host?: string | null;
    /**
     * 
     * @type {TenantTypeEnum}
     * @memberof UpdateTenantInput
     */
    tenantType?: TenantTypeEnum;
    /**
     * 
     * @type {DbType}
     * @memberof UpdateTenantInput
     */
    dbType?: DbType;
    /**
     * 数据库连接
     * @type {string}
     * @memberof UpdateTenantInput
     */
    connection?: string | null;
    /**
     * 数据库标识
     * @type {string}
     * @memberof UpdateTenantInput
     */
    configId?: string | null;
    /**
     * 排序
     * @type {number}
     * @memberof UpdateTenantInput
     */
    orderNo?: number;
    /**
     * 备注
     * @type {string}
     * @memberof UpdateTenantInput
     */
    remark?: string | null;
    /**
     * 
     * @type {StatusEnum}
     * @memberof UpdateTenantInput
     */
    status?: StatusEnum;
    /**
     * 电子邮箱
     * @type {string}
     * @memberof UpdateTenantInput
     */
    email?: string | null;
    /**
     * 电话
     * @type {string}
     * @memberof UpdateTenantInput
     */
    phone?: string | null;
    /**
     * 租户名称
     * @type {string}
     * @memberof UpdateTenantInput
     */
    name: string;
    /**
     * 管理员名称
     * @type {string}
     * @memberof UpdateTenantInput
     */
    adminName: string;
}
