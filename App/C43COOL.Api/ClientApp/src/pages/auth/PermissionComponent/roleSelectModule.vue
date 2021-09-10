<template>
  <div>
    <a-modal
      :title="dialogTitle"
      v-model="show"
      :centered="true"
      :maskClosable="false"
      :width="1000"
      @cancel="handleCancel"
      @ok="handleConfirm"
    >
      <div>
        <a-form-model
          layout="horizontal"
          ref="queryForm"
          :model="queryFormModel"
        >
          <div>
            <a-row>
              <a-col :md="8" :sm="24">
                <a-form-model-item
                  ref="Name"
                  :label="$t('RoleName')"
                  :labelCol="{ span: 6 }"
                  :wrapperCol="{ span: 12, offset: 0 }"
                  prop="Name"
                >
                  <a-input v-model="queryFormModel.Name" />
                </a-form-model-item>
              </a-col>
              <a-col :md="8" :sm="24">
                <a-button type="primary" @click="QueryTable">{{
                  $t("query")
                }}</a-button>
                <a-button style="margin-left: 8px" @click="ResetQueryForm">{{
                  $t("reset")
                }}</a-button>
              </a-col>
            </a-row>
          </div>
        </a-form-model>
      </div>
      <a-table
        :row-key="(record) => record.Id"
        :data-source="data"
        :loading="loading"
        :pagination="pagination"
        :row-selection="rowSelection"
        @change="handleTableChange"
      >
        <a-table-column key="Name" data-index="Name" :title="$t('RoleName')" />
        <a-table-column
          key="Description"
          data-index="Description"
          :title="$t('Description')"
        />
        <a-table-column key="Sort" data-index="Sort" :title="$t('Sort')" />
      </a-table>
    </a-modal>
  </div>
</template>
<script>
import RoleApi from "@/services/role.js";
import { UserApi } from "@/services/user.js";
export default {
  i18n: require("../../i18n"),
  data() {
    return {
      dialogTitle: "绑定角色",
      data: [],
      show: false,
      loading: false,
      selectedRowKeys: [],
      UserId: 0,
      pagination: {
        defaultCurrent: 1,
        defaultPageSize: 20,
        showSizeChanger: true,
        pageSizeOptions: ["50", "100", "500"],
      },
      labelCol: { span: 4 },
      wrapperCol: { span: 14 },
      queryFormModel: {
        Name: "",
        Description: "",
        Sort: 0,
      },
    };
  },
  mounted() {
    this.fetch();
  },
  watch: {},
  computed: {
    rowSelection: function () {
      return {
        selectedRowKeys: this.selectedRowKeys,
        onChange: this.onSelectChange,
      };
    },
  },
  methods: {
    async ShowC(Id) {
      this.UserId = Id;
      var temp = (await UserApi.GetBindRoleList({ UserId: this.UserId })).data;
      console.log(temp);
      this.selectedRowKeys = temp;
      this.show = true;
    },
    // 回车方法
    handleQuery(e) {
      var eCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
      if (eCode === 13) {
        // 调用对应的方法
        this.QueryTable();
      }
    },
    // 回车方法
    handleKeyDown(e) {
      var eCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
      if (eCode === 13) {
        // 调用对应的方法
        this.handleConfirm();
      }
    },
    // 多选的方法
    onSelectChange(selectedRowKeys) {
      selectedRowKeys.length > 0
        ? (this.deleteDisable = false)
        : (this.deleteDisable = true);
      this.selectedRowKeys = selectedRowKeys;
    },
    handleTableChange(pagination, filters) {
      console.log(pagination);
      const pager = { ...this.pagination };
      pager.current = pagination.current;
      pager.defaultPageSize = pagination.pageSize;
      this.pagination = pager;
      this.fetch({
        pageSize: pagination.pageSize,
        page: pagination.current,
        ...this.RoleForm,
        ...filters,
      });
    },
    QueryTable() {
      this.fetch({
        pageSize: this.pagination.defaultPageSize,
        page: this.pagination.defaultCurrent,
        ...this.RoleForm,
      });
    },
    async fetch(params = {}) {
      console.log("params:", params);
      const pagination = { ...this.pagination };
      params.page = params.page || pagination.defaultCurrent;
      params.pageSize = params.pageSize || pagination.defaultPageSize;
      this.loading = true;
      var res = (await RoleApi.getList(params)).data;
      pagination.total = Number(res.TotalElements);
      this.loading = false;
      var result = res.Data || [];
      this.data = result;
      this.pagination = pagination;
    },
    // 新增或修改语言
    async handleConfirm() {
      console.log(this.selectedRowKeys);
      var data = { UserId: this.UserId, RoleIds: this.selectedRowKeys };
      await UserApi.SaveUserRole(data);
      this.$message.success("保存成功", 2);
      this.show = false;
    },
    // 取消
    handleCancel() {
      this.show = false;
      this.$refs.queryForm.resetFields();
    },
    ResetQueryForm() {
      this.$refs.queryForm.resetFields();
    },
  },
};
</script>
