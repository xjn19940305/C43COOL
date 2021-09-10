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
      <a-tree
        checkable
        v-model="SelectTreeData"
        :defaultExpandAll="true"
        :checkStrictly="true"
        :tree-data="data"
      />
    </a-modal>
  </div>
</template>
<script>
import ModuleApi from "@/services/modules.js";
import RoleApi from "@/services/role.js";
export default {
  i18n: require("../../i18n"),
  data() {
    return {
      dialogTitle: "绑定角色",
      ModuleData: [],
      data: [],
      show: false,
      loading: false,
      RoleId: 0,
      autoExpandParent: true,
      temp: [],
      SelectTreeData: [],
    };
  },
  mounted() {
    this.fetch();
  },
  watch: {},
  computed: {},
  methods: {
    async fetch() {
      this.data = [];
      this.loading = true;
      var local = (await ModuleApi.Query()).data.Data;
      this.ModuleData = local;
      this.ToTreeList(local, this.data, "");
      this.loading = false;
    },
    ToTreeList(list, tree, parentId) {
      list.forEach((item) => {
        // 判断是否为父级菜单
        if (item.ParentId === parentId) {
          const child = {
            key: item.Id,
            title: item.Name,
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
    async Show(Id) {
      this.RoleId = Id;
      var data = (await RoleApi.GetRoleMenus({ RoleId: this.RoleId })).data;
      this.SelectTreeData = data;
      this.show = true;
    },
    handleCancel() {},
    async handleConfirm() {
      var data = this.SelectTreeData.checked
        ? this.SelectTreeData.checked
        : this.SelectTreeData;
      data = this.ModuleData.map((f) => f.Id).filter(function (val) {
        return data.indexOf(val) > -1;
      });
      await RoleApi.AuthorizeRoleMenu({
        RoleId: this.RoleId,
        ModuleIds: data,
      });
      this.$message.success("保存成功", 2);
      this.show = false;
    },
  },
};
</script>
