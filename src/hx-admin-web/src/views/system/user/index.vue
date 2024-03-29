<template>
	<div class="sys-user-container">
		<el-row :gutter="8" style="width: 100%">
			<el-col :span="4" :xs="24">
				<OrgTree ref="orgTreeRef" @node-click="nodeClick" />
			</el-col>

			<el-col :span="20" :xs="24">
				<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
					<el-form :model="state.queryParams" ref="queryForm" :inline="true">
						<el-form-item label="账号" prop="account">
							<el-input placeholder="账号" clearable @keyup.enter="handleQuery" v-model="state.queryParams.account" />
						</el-form-item>
						<!-- <el-form-item label="姓名" prop="realName">
							<el-input placeholder="姓名" clearable @keyup.enter="handleQuery" v-model="state.queryParams.realName" />
						</el-form-item> -->
						<el-form-item label="手机号码" prop="phone">
							<el-input placeholder="手机号码" clearable @keyup.enter="handleQuery" v-model="state.queryParams.phone" />
						</el-form-item>
						<el-form-item>
							<el-button-group>
								<el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'sysUser:page'"> 查询 </el-button>
								<el-button icon="ele-Refresh" @click="resetQuery"> 重置 </el-button>
							</el-button-group>
						</el-form-item>
						<el-form-item>
							<el-button type="primary" icon="ele-Plus" @click="openAddUser" v-auth="'sysUser:add'"> 新增 </el-button>
						</el-form-item>
					</el-form>
				</el-card>

				<el-card class="full-table" shadow="hover" style="margin-top: 8px">
					<el-table :data="state.userData" style="width: 100%" v-loading="state.loading" border>
						<el-table-column type="index" label="序号" width="55" align="center" fixed />
						<el-table-column prop="account" label="账号" width="120" fixed show-overflow-tooltip />
						<el-table-column prop="nickName" label="昵称" width="120" show-overflow-tooltip />
						<el-table-column label="头像" width="80" align="center" show-overflow-tooltip>
							<template #default="scope">
								<el-avatar src="" size="small">{{ scope.row.nickName?.slice(0, 1) }} </el-avatar>
							</template>
						</el-table-column>
						<el-table-column prop="realName" label="姓名" width="120" show-overflow-tooltip />
						<el-table-column label="性别" width="70" align="center" show-overflow-tooltip>
							<template #default="scope">
								<el-tag type="success" v-if="scope.row.sex === 1"> 男 </el-tag>
								<el-tag type="danger" v-else> 女 </el-tag>
							</template>
						</el-table-column>
						<el-table-column prop="phone" label="手机号码" width="120" align="center" show-overflow-tooltip />
						<el-table-column label="状态" width="70" align="center" show-overflow-tooltip>
							<template #default="scope">
								<el-switch v-model="scope.row.status" :active-value="1" :inactive-value="2" size="small" @change="changeStatus(scope.row)" :disabled="!auth('sysUser:setStatus')" />
							</template>
						</el-table-column>
						<el-table-column prop="sort" label="排序" width="70" align="center" show-overflow-tooltip />
						<el-table-column prop="updateTime" label="修改时间" width="160" show-overflow-tooltip />
						<el-table-column prop="remark" label="备注" show-overflow-tooltip />
						<el-table-column label="操作" width="110" align="center" fixed="right" show-overflow-tooltip>
							<template #default="scope">
								<el-button icon="ele-Edit" size="small" text type="primary" @click="openEditUser(scope.row)" v-auth="'sysUser:update'"> 编辑 </el-button>
								<el-dropdown>
									<el-button icon="ele-MoreFilled" size="small" text type="primary" style="padding-left: 12px" />
									<template #dropdown>
										<el-dropdown-menu>
											<el-dropdown-item icon="ele-RefreshLeft" @click="resetUserPwd(scope.row)" :disabled="!auth('sysUser:resetPwd')"> 重置密码 </el-dropdown-item>
											<el-dropdown-item icon="ele-Delete" @click="delUser(scope.row)" divided :disabled="!auth('sysUser:delete')"> 删除账号 </el-dropdown-item>
										</el-dropdown-menu>
									</template>
								</el-dropdown>
							</template>
						</el-table-column>
					</el-table>
					<el-pagination
						v-model:currentPage="state.tableParams.page"
						v-model:page-size="state.tableParams.pageSize"
						:total="state.tableParams.total"
						:page-sizes="[10, 20, 50, 100]"
						small
						background
						@size-change="handleSizeChange"
						@current-change="handleCurrentChange"
						layout="total, sizes, prev, pager, next, jumper"
					/>
				</el-card>
			</el-col>
		</el-row>
		<EditUser ref="editUserRef" :title="state.editUserTitle" :orgData="state.orgTreeData" />
	</div>
</template>

<script lang="ts" setup name="sysUser">
import { onMounted, onUnmounted, reactive, ref } from 'vue';
import { ElMessageBox, ElMessage } from 'element-plus';
import { auth } from '/@/utils/authFunction';
import mittBus from '/@/utils/mitt';
import OrgTree from '/@/views/system/org/component/orgTree.vue';
import EditUser from '/@/views/system/user/component/editUser.vue';

import { getAPI } from '/@/utils/axios-utils';
import { SysUserApi, SysOrgApi } from '/@/api-services/api';
import { SysUser, SysOrg } from '/@/api-services/models';

const orgTreeRef = ref<InstanceType<typeof OrgTree>>();
const editUserRef = ref<InstanceType<typeof EditUser>>();
const state = reactive({
	loading: false,
	userData: [] as Array<SysUser>,
	orgTreeData: [] as Array<SysOrg>,
	queryParams: {
		orgId: -1,
		account: undefined,
		realName: undefined,
		phone: undefined,
	},
	tableParams: {
		page: 1,
		pageSize: 10,
		total: 0 as any,
	},
	editUserTitle: '',
});

onMounted(async () => {
	loadOrgData();
	handleQuery();

	mittBus.on('submitRefresh', () => {
		handleQuery();
	});
});

onUnmounted(() => {
	mittBus.off('submitRefresh');
});

// 查询机构数据
const loadOrgData = async () => {
	state.loading = true;
	var res = await getAPI(SysOrgApi).getSysOrgList(0);
	state.orgTreeData = res.data.data ?? [];
	state.loading = false;
};

// 查询操作
const handleQuery = async () => {
	state.loading = true;
	let params = Object.assign(state.queryParams, state.tableParams);
	var res = await getAPI(SysUserApi).getUserPage(params);
	state.userData = res.data.data?.items ?? [];
	state.tableParams.total = res.data.data?.total;
	state.loading = false;
};

// 重置操作
const resetQuery = () => {
	state.queryParams.orgId = -1;
	state.queryParams.account = undefined;
	state.queryParams.realName = undefined;
	state.queryParams.phone = undefined;
	handleQuery();
};

// 打开新增页面
const openAddUser = () => {
	state.editUserTitle = '添加账号';
	editUserRef.value?.openDialog({});
};

// 打开编辑页面
const openEditUser = (row: any) => {
	state.editUserTitle = '编辑账号';
	editUserRef.value?.openDialog(row);
};

// 删除
const delUser = (row: any) => {
	ElMessageBox.confirm(`确定删除账号：【${row.account}】?`, '提示', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		type: 'warning',
	})
		.then(async () => {
			await getAPI(SysUserApi).deleteUser({ id: row.id });
			handleQuery();
			ElMessage.success('删除成功');
		})
		.catch(() => {});
};

// 改变页面容量
const handleSizeChange = (val: number) => {
	state.tableParams.pageSize = val;
	handleQuery();
};

// 改变页码序号
const handleCurrentChange = (val: number) => {
	state.tableParams.page = val;
	handleQuery();
};

// 修改状态
const changeStatus = (row: any) => {
	getAPI(SysUserApi)
		.setUserStatus({ id: row.id, status: row.status })
		.then(() => {
			ElMessage.success('账号状态设置成功');
		})
		.catch(() => {
			row.status = row.status == 1 ? 2 : 1;
		});
};

// 重置密码
const resetUserPwd = async (row: any) => {
	ElMessageBox.confirm(`确定重置密码：【${row.account}】?`, '提示', {
		confirmButtonText: '确定',
		cancelButtonText: '取消',
		type: 'warning',
	})
		.then(async () => {
			var res = await getAPI(SysUserApi).resetUserPwd({ id: row.id });
			ElMessage.success(`密码重置成功：${res.data.data}`);
		})
		.catch(() => {});
};

// 树组件点击
const nodeClick = async (node: any) => {
	state.queryParams.orgId = node.id;
	state.queryParams.account = undefined;
	state.queryParams.realName = undefined;
	state.queryParams.phone = undefined;
	handleQuery();
};
</script>
