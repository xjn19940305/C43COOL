<template>
  <a-card :bordered="false">
    <div>
      <a-form-model layout="horizontal" ref="queryForm" :model="queryFormModel">
        <div>
          <a-row>
            <a-col :md="8" :sm="24">
              <a-form-model-item
                ref="Name"
                :label="$t('Name')"
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
        <a-table-column key="Name" data-index="Name" :title="$t('Name')" />
        <a-table-column key="Email" data-index="Email" :title="$t('Email')" />
        <a-table-column
          key="PhoneNumber"
          data-index="PhoneNumber"
          :title="$t('PhoneNumber')"
        />
        <a-table-column
          key="DateCreated"
          data-index="DateCreated"
          :title="$t('DateCreated')"
        />
        <a-table-column key="action" :title="$t('Action')">
          <template slot-scope="text, record">
            <span>
              <a style="margin-right: 8px" @click="Modify(record.Id)">
                <a-icon type="edit" />{{ $t("Modify") }}
              </a>
              <a
                style="margin-right: 8px"
                @click="$refs.roleSelectModule.ShowC(record.Id)"
              >
                <a-icon type="bold" />{{ $t("BindRole") }}
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
        ref="UserForm"
        :model="UserForm"
        :label-col="labelCol"
        :wrapper-col="wrapperCol"
        :rules="rules"
      >
        <a-form-model-item :label="$t('Name')" prop="Name">
          <a-input
            :placeholder="$t('Name')"
            @keydown.native.stop="handleKeyDown"
            v-model="UserForm.Name"
          />
        </a-form-model-item>
        <a-form-model-item :label="$t('NickName')" prop="Description">
          <a-input
            :placeholder="$t('NickName')"
            @keydown.native.stop="handleKeyDown"
            v-model="UserForm.NickName"
          />
        </a-form-model-item>
        <a-form-model-item :label="$t('PhoneNumber')" prop="Sort">
          <a-input
            :placeholder="$t('PhoneNumber')"
            @keydown.native.stop="handleKeyDown"
            v-model="UserForm.PhoneNumber"
          />
        </a-form-model-item>
      </a-form-model>
    </a-modal>
    <role-select-module ref="roleSelectModule"></role-select-module>
  </a-card>
</template>

<script>
import { UserApi } from "@/services/user.js";
import roleSelectModule from "@/pages/auth/PermissionComponent/roleSelectModule";
import Tools from "@/utils/tools";
export default {
  name: "UserList",
  i18n: require("../i18n"),
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
      UserForm: {
        Name: "",
        NickName: "",
        PhoneNumber: "",
      },
      rules: {
        Name: [
          { required: true, message: this.$t("REQUIRED"), trigger: "change" },
        ],
      },
    };
  },
  components: {
    roleSelectModule,
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
      var data = (await UserApi.Get({ UserId: id })).data;
      this.UserForm = data;
    },
    async Delete() {
      console.log("select", this.selectedRowKeys);
      this.deleteLoading = true;
      await UserApi.Delete({ data: this.selectedRowKeys });
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
      var res = (await UserApi.getList(params)).data;
      pagination.total = Number(res.TotalElements);
      this.loading = false;
      var result = res.Data || [];
      result.map((f) => {
        f.DateCreated = Tools.formatUtc(f.DateCreated, "yyyy-MM-DD HH:mm:ss");
        return f;
      });
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
      this.$refs.UserForm.resetFields();
    },
    handleKeyDown(e) {
      var eCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
      if (eCode === 13) {
        // 调用对应的方法
        this.handleConfirm();
      }
    },
    handleConfirm() {
      this.$refs.UserForm.validate(async (valid) => {
        if (valid) {
          console.log(this.UserForm);
          if (this.Operation == "Create") {
            await UserApi.Create(this.UserForm);
            this.$message.success("创建成功", 2);
          } else {
            await UserApi.Modify(this.UserForm);
            this.$message.success("修改成功", 2);
          }
          this.$refs.UserForm.resetFields();
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