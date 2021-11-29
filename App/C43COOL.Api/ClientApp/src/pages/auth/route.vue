<template>
  <a-card :bordered="false">
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
      v-if="data.length > 0"
      :data-source="data"
      :row-selection="rowSelection"
      :row-key="(record) => record.Id"
      :loading="loading"
      :defaultExpandAllRows="true"
    >
      <a-table-column key="Name" data-index="Name" :title="$t('Module')" />
      <a-table-column key="Path" data-index="Path" :title="$t('Path')" />
      <a-table-column
        key="ComponentUrl"
        data-index="ComponentUrl"
        :title="$t('Menu_Component')"
      />
      <a-table-column
        key="ModuleName"
        data-index="ModuleName"
        :title="$t('ModuleName')"
      />
      <a-table-column key="Sort" data-index="Sort" :title="$t('Sort')" />
      <a-table-column key="Icon" data-index="Icon" :title="$t('Icon')" />
      <a-table-column
        key="actionType"
        data-index="actionType"
        :title="$t('IsBtn')"
      >
      </a-table-column>
      <a-table-column key="action" :title="$t('Action')">
        <template slot-scope="text, record">
          <span>
            <a @click="Save(record.Id)">{{ $t("Modify") }}</a>
            <a-divider type="vertical" />
            <!-- <a-popconfirm
              :title="$t('confirmDelete')"
              @confirm="() => Delete(record.id)"
            >
              <a href="javascript:;">{{ $t("Delete") }}</a>
            </a-popconfirm> -->
          </span>
        </template>
      </a-table-column>
    </a-table>
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
        ref="ModuleForm"
        :model="ModuleForm"
        :label-col="labelCol"
        :wrapper-col="wrapperCol"
        :rules="rules"
      >
        <a-form-model-item :label="$t('Name')" prop="Name">
          <a-input
            :placeholder="$t('Name')"
            @keydown.native.stop="handleKeyDown"
            v-model="ModuleForm.Name"
          />
        </a-form-model-item>
        <a-form-model-item :label="$t('Path')" prop="Path">
          <a-input
            :placeholder="$t('Path')"
            @keydown.native.stop="handleKeyDown"
            v-model="ModuleForm.Path"
          />
        </a-form-model-item>
        <a-form-model-item :label="$t('ComponentUrl')" prop="ComponentUrl">
          <a-input
            :placeholder="$t('ComponentUrl')"
            @keydown.native.stop="handleKeyDown"
            v-model="ModuleForm.ComponentUrl"
          />
        </a-form-model-item>
        <a-form-model-item :label="$t('ModuleName')" prop="ModuleName">
          <a-input
            :placeholder="$t('ModuleName')"
            @keydown.native.stop="handleKeyDown"
            v-model="ModuleForm.ModuleName"
          />
        </a-form-model-item>
        <a-form-model-item :label="$t('Sort')" prop="Sort">
          <a-input
            :placeholder="$t('Sort')"
            @keydown.native.stop="handleKeyDown"
            v-model="ModuleForm.Sort"
          />
        </a-form-model-item>
        <a-form-model-item :label="$t('ModuleType')" prop="ModuleType">
          <a-select
            v-model="ModuleForm.ModuleType"
            placeholder="请选择按钮类型"
          >
            <a-select-option :value="0"> {{ $t("Module") }} </a-select-option>
            <a-select-option :value="1"> {{ $t("Button") }} </a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item :label="$t('Parent')" prop="Parent">
          <a-tree-select
            :tree-data="treeData"
            :placeholder="$t('CHOOSEONE')"
            tree-default-expand-all
            v-model="ModuleForm.ParentId"
            :defaultExpandAllRows="true"
          >
            <span slot="title" slot-scope="{ value }" style="color: #08c"
              >Child Node1 {{ value }}</span
            >
          </a-tree-select>
        </a-form-model-item>
      </a-form-model>
    </a-modal>
  </a-card>
</template>

<script>
import ModuleApi from "@/services/modules.js";
export default {
  name: "ModulePage",
  i18n: require("../i18n"),
  components: {},
  mounted() {
    this.Init();
  },
  data() {
    return {
      selectedRowKeys: [],
      ModuleData: [],
      data: [],
      treeData: [
        {
          title: "root",
          key: "root",
          value: "",
          parentId: "",
          children: [],
        },
      ],
      loading: false,
      show: false,
      deleteDisable: true,
      deleteLoading: false,
      dialogTitle: "",
      labelCol: { span: 4 },
      wrapperCol: { span: 14 },
      ModuleForm: {
        Name: "",
        Path: "",
        ComponentUrl: "",
        ModuleName: "",
        Icon: "",
        ParentId: "",
        Sort: 0,
        ModuleType: 0,
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
    handleKeyDown(e) {
      var eCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
      if (eCode === 13) {
        // 调用对应的方法
        this.handleConfirm();
      }
    },
    async Init() {
      await this.LoadTable();
      await this.LoadSelectTree();
    },
    onSelectChange(selectedRowKeys) {
      selectedRowKeys.length > 0
        ? (this.deleteDisable = false)
        : (this.deleteDisable = true);
      this.selectedRowKeys = selectedRowKeys;
    },
    async LoadTable() {
      this.data = [];
      this.loading = true;
      var local = (await ModuleApi.Query()).data.Data;
      this.ModuleData = local;
      console.log(local);
      local.forEach((f) => {
        if (f.ModuleType === 0) {
          f.actionType = this.$t("Module");
        } else {
          f.actionType = this.$t("Button");
        }
      });
      this.ToTreeList(local, this.data, "");
      this.loading = false;
    },
    ToTreeList(list, tree, parentId) {
      list.forEach((item) => {
        // 判断是否为父级菜单
        if (item.ParentId === parentId) {
          item.meta = item.meta || {};
          const child = {
            ...item,
            key: item.key || item.Name,
            icon: item.Icon || "",
            children: [],
          };
          // 迭代 list， 找到当前菜单相符合的所有子菜单
          this.ToTreeList(list, child.children, item.Id);
          // 删掉不存在 children 值的属性
          if (child.children.length <= 0) {
            delete child.children;
          }
          // 加入到树中
          tree.push(child);
        }
      });
    },
    async LoadSelectTree() {
      this.treeData[0].children = [];
      this.ModuleForm.ParentId = "";
      this.getTreeSelectList(
        this.ModuleData.filter((f) => f.ModuleType == 0),
        this.treeData[0].children,
        ""
      );
    },
    getTreeSelectList(list, tree, parentId) {
      list.forEach((item) => {
        // 判断是否为父级菜单
        if (item.ParentId === parentId) {
          const child = {
            title: item.Name,
            key: item.Id,
            value: item.Id,
            parentId: item.ParentId,
            children: [],
          };
          // 迭代 list， 找到当前菜单相符合的所有子菜单
          this.getTreeSelectList(list, child.children, item.Id);
          // 删掉不存在 children 值的属性
          if (child.children.length <= 0) {
            delete child.children;
          }
          // 加入到树中
          tree.push(child);
        }
      });
    },
    Create() {
      this.Operation = "Create";
      this.show = true;
      this.dialogTitle = this.$t("Create");
    },
    async Save(id) {
      this.Operation = "Modify";
      this.show = true;
      this.dialogTitle = this.$t("Modify");
      var data = (await ModuleApi.Get({ Id: id })).data;
      this.ModuleForm = data;
    },
    handleCancel() {
      this.$refs.ModuleForm.resetFields();
    },
    handleConfirm() {
      this.$refs.ModuleForm.validate(async (valid) => {
        if (valid) {
          console.log(this.ModuleForm);
          if (this.Operation == "Create") {
            await ModuleApi.Create(this.ModuleForm);
            this.$message.success("创建成功", 2);
          } else {
            await ModuleApi.Modify(this.ModuleForm);
            this.$message.success("修改成功", 2);
          }
          this.$refs.ModuleForm.resetFields();
          this.show = false;
          await this.LoadTable();
          await this.LoadSelectTree();
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
    async Delete() {
      console.log("select", this.selectedRowKeys);
      this.deleteLoading = true;
      await ModuleApi.Delete({ data: this.selectedRowKeys });
      this.selectedRowKeys = [];
      this.deleteLoading = false;
      this.$message.success("删除成功", 2);
      await this.LoadTable();
      await this.LoadSelectTree();
    },
  },
};
</script>