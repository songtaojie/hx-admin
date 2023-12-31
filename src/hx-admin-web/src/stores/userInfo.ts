import { defineStore } from 'pinia';
import { Local, Session } from '/@/utils/storage';
import Watermark from '/@/utils/watermark';
import { useThemeConfig } from '/@/stores/themeConfig';
import { useSystemConfig } from '/@/stores/systemConfig';


import { getAPI } from '/@/utils/axios-utils';
import { SysAuthApi, SysConstApi } from '/@/api-services/api';

/**
 * 用户信息
 * @methods setUserInfos 设置用户信息
 */
export const useUserInfo = defineStore('userInfo', {
	state: (): UserInfosState => ({
		userInfos: {} as any,
		constList: [] as any,
	}),
	getters: {
		// 获取系统常量列表
		async getSysConstList(): Promise<any[]> {
			var res = await getAPI(SysConstApi).apiSysConstListGet();
			this.constList = res.data.result ?? [];
			return this.constList;
		},
	},
	actions: {
		async setUserInfos() {
			// 缓存用户信息
			if (Session.get('userInfo')) {
				this.userInfos = Session.get('userInfo');
			} else {
				const userInfos: any = await this.getApiUserInfo();
				this.userInfos = userInfos;
			}
		},
		// 获取当前用户信息
		getApiUserInfo() {
			return new Promise((resolve) => {
				getAPI(SysAuthApi)
					.getUserInfo()
					.then(async (res: any) => {
						if (res.data.result == null) return;
						var d = res.data.result;
						const userInfos = {
							account: d.account,
							realName: d.realName,
							avatar: d.avatar ? d.avatar : '/favicon.ico',
							address: d.address,
							signature: d.signature,
							orgId: d.orgId,
							orgName: d.orgName,
							posName: d.posName,
							roles: [],
							authBtnList: d.buttons,
							time: new Date().getTime(),
						};
						Session.set('userInfo', userInfos);

						// 读取用户配置
						const storesThemeConfig = useThemeConfig();
						const storesSystemConfig = useSystemConfig();

						// 是否设置水印
						storesThemeConfig.themeConfig.isWatermark = storesSystemConfig.sysConfig.watermarkEnabled;
						if (storesThemeConfig.themeConfig.isWatermark) Watermark.set(storesThemeConfig.themeConfig.watermarkText);
						else Watermark.del();

						Local.remove('themeConfig');
						Local.set('themeConfig', storesThemeConfig.themeConfig);
						
						resolve(userInfos);
					});
			});
		},
	},
});
