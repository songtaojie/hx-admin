<template>
	<div class="weChatUser-container">
		<el-dialog v-model="state.isShowDialog" draggable width="600px">
			<template #header>
				<div style="color: #fff">
					<el-icon size="16" style="margin-right: 3px; display: inline; vertical-align: middle"> <ele-Edit /> </el-icon>
					<span> {{ props.title }} </span>
				</div>
			</template>
			<el-form :model="state.ruleForm" ref="ruleFormRef" size="default" label-width="80px">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="昵称" prop="nickName" :rules="[{ required: true, message: '昵称不能为空', trigger: 'blur' }]">
							<el-input v-model="state.ruleForm.nickName" placeholder="昵称" clearable />
						</el-form-item>
					</el-col>
				</el-row>
			</el-form>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="cancel" size="default">取 消</el-button>
					<el-button type="primary" @click="submit" size="default">确 定</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup name="sysEditWeChatUser">
import { reactive, ref } from 'vue';
import mittBus from '/@/utils/mitt';

import { getAPI } from '/@/utils/axios-utils';
import { SysWechatUserApi } from '/@/api-services/api';
import { SysWechatUser } from '/@/api-services/models';

const props = defineProps({
	title: String,
});
const ruleFormRef = ref();
const state = reactive({
	isShowDialog: false,
	ruleForm: {} as SysWechatUser,
});

// 打开弹窗
const openDialog = (row: any) => {
	state.ruleForm = JSON.parse(JSON.stringify(row));
	state.isShowDialog = true;
};

// 关闭弹窗
const closeDialog = () => {
	mittBus.emit('submitRefresh');
	state.isShowDialog = false;
};

// 取消
const cancel = () => {
	state.isShowDialog = false;
};

// 提交
const submit = () => {
	ruleFormRef.value.validate(async (valid: boolean) => {
		if (!valid) return;
		if (state.ruleForm.id != undefined && state.ruleForm.id > 0) {
			await getAPI(SysWechatUserApi).updateSysWechatUser(state.ruleForm);
		} else {
			await getAPI(SysWechatUserApi).addSysWechatUser(state.ruleForm);
		}
		closeDialog();
	});
};

// 导出对象
defineExpose({ openDialog });
</script>
