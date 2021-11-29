<template>
  <a-card :bordered="false">
    <div>
      <a-form-model layout="horizontal" ref="queryForm" :model="queryFormModel">
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
              <a-button type="primary" @click="query">{{
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
    <div>
      <a-space class="operator" style="margin-bottom: 10px">
        <a-button @click="Create" type="primary">{{ $t("Create") }}</a-button>
        <a-button
          @click="Delete"
          :disabled="deleteDisable"
          type="danger"
          :loading="deleteLoading"
          >{{ $t("Delete") }}</a-button
        >
      </a-space>
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
        <a-table-column key="action" :title="$t('Action')">
          <template slot-scope="text, record">
            <span>
              <a style="margin-right: 8px" @click="Modify(record.Id)">
                <a-icon type="edit" />{{ $t("Modify") }}
              </a>
              <a
                style="margin-right: 8px"
                @click="$refs.menuSelectModule.Show(record.Id)"
              >
                <a-icon type="edit" />{{ $t("BindModule") }}
              </a>
            </span>
          </template>
        </a-table-column>
      </a-table>
    </div>
    <a-modal
      :title="dialogTitle"
      v-model="show"
      :centered="true"
      :maskClosable="false"
      :width="800"
      @cancel="handleCancel"
      @ok="handleConfirm"
    >
      <a-form-model
        ref="RoleForm"
        :model="RoleForm"
        :label-col="labelCol"
        :wrapper-col="wrapperCol"
        :rules="rules"
      >
        <a-form-model-item :label="$t('RoleName')" prop="Name">
          <a-input
            :placeholder="$t('RoleName')"
            @keydown.native.stop="handleKeyDown"
            v-model="RoleForm.Name"
          />
        </a-form-model-item>
        <a-form-model-item :label="$t('Description')" prop="Description">
          <a-input
            :placeholder="$t('Description')"
            @keydown.native.stop="handleKeyDown"
            v-model="RoleForm.Description"
          />
        </a-form-model-item>
        <a-form-model-item :label="$t('Sort')" prop="Sort">
          <a-input
            :placeholder="$t('Sort')"
            @keydown.native.stop="handleKeyDown"
            v-model="RoleForm.Sort"
          />
        </a-form-model-item>
      </a-form-model>
    </a-modal>
    <menu-select-module ref="menuSelectModule"></menu-select-module>
  </a-card>
</template>

<script>
import RoleApi from "@/services/role.js";
import menuSelectModule from "@/pages/auth/PermissionComponent/menuSelectModule";
export default {
  name: "RoleList",
  i18n: require("../i18n"),
  components: {
    menuSelectModule,
  },
  mounted() {
    this.fetch();
  },
  data() {
    return {
      Operation: "",
      selectedRowKeys: [],
      queryFormModel: {
        Name: "",
      },
      data: [],
      loading: false,
      pagination: {
        defaultCurrent: 1,
        defaultPageSize: 20,
        showSizeChanger: true,
        pageSizeOptions: ["50", "100", "500"],
      },
      show: false,
      deleteDisable: true,
      deleteLoading: false,
      dialogTitle: "",
      labelCol: { span: 4 },
      wrapperCol: { span: 14 },
      RoleForm: {
        Name: "",
        Description: "",
        Sort: 0,
      },
      rules: {
        Name: [
          { required: true, message: this.$t("REQUIRED"), trigger: "change" },
        ],
      },
    };
  },
  computed: {
    rowSelection: function () {
      return {
        selectedRowKeys: this.selectedRowKeys,
        onChange: this.onSelectChange,
      };
    },
  },
  methods: {
    onSelectChange(selectedRowKeys) {
      selectedRowKeys.length > 0
        ? (this.deleteDisable = false)
        : (this.deleteDisable = true);
      this.selectedRowKeys = selectedRowKeys;
    },
    Create() {
      this.Operation = "Create";
      this.show = true;
      this.dialogTitle = this.$t("Create");
    },
    async Modify(id) {
      this.Operation = "Modify";
      this.show = true;
      this.dialogTitle = this.$t("Modify");
      var data = (await RoleApi.Get({ Id: id })).data;
      this.RoleForm = data;
    },
    async Delete() {
      console.log("select", this.selectedRowKeys);
      this.deleteLoading = true;
      await RoleApi.Delete({ data: this.selectedRowKeys });
      this.selectedRowKeys = [];
      this.deleteLoading = false;
      this.$message.success("删除成功", 2);
      this.query();
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
    handleTableChange(pagination, filters) {
      console.log(pagination);
      const pager = { ...this.pagination };
      pager.current = pagination.current;
      pager.defaultPageSize = pagination.pageSize;
      this.pagination = pager;
      this.fetch({
        pageSize: pagination.pageSize,
        page: pagination.current,
        ...this.queryFormModel,
        ...filters,
      });
    },
    query() {
      var current = this.pagination.current || this.pagination.defaultCurrent;
      var base = {
        pageSize: this.pagination.defaultPageSize,
        page: current,
      };
      var queryData = { ...base, ...this.queryFormModel };
      this.fetch(queryData);
    },
    ResetQueryForm() {
      this.$refs.queryForm.resetFields();
    },
    handleCancel() {
      console.log("cancel");
      this.$refs.RoleForm.resetFields();
    },
    handleKeyDown(e) {
      var eCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
      if (eCode === 13) {
        // 调用对应的方法
        this.handleConfirm();
      }
    },
    handleConfirm() {
      this.$refs.RoleForm.validate(async (valid) => {
        if (valid) {
          console.log(this.RoleForm);
          if (this.Operation == "Create") {
            await RoleApi.Create(this.RoleForm);
            this.$message.success("创建成功", 2);
          } else {
            await RoleApi.Modify(this.RoleForm);
            this.$message.success("修改成功", 2);
          }
          this.$refs.RoleForm.resetFields();
          this.show = false;
          this.query();
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
  },
};
</script>