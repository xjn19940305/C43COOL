<template>
  <page-header-wrapper>
    <a-card :bordered="false">
      <div :class="advanced ? 'search' : null">
        <a-form-model
          layout="horizontal"
          ref="queryForm"
          :model="queryFormModel"
        >
          <div :class="advanced ? null : 'fold'">
            <a-row>
              <a-col :md="8" :sm="24">
                <a-form-model-item
                  ref="Name"
                  :label="$t('角色名称')"
                  :labelCol="{ span: 6 }"
                  :wrapperCol="{ span: 18, offset: 0 }"
                  prop="Name"
                >
                  <a-input v-model="queryFormModel.Name" />
                </a-form-model-item>
              </a-col>
            </a-row>
          </div>
          <span style="float: right; margin-top: 3px">
            <a-button type="primary" @click="query">{{ $t("query") }}</a-button>
            <a-button style="margin-left: 8px" @click="ResetQueryForm">{{
              $t("reset")
            }}</a-button>
            <a @click="toggleAdvanced" style="margin-left: 8px">
              {{ advanced ? "收起" : "展开" }}
              <a-icon :type="advanced ? 'up' : 'down'" />
            </a>
          </span>
        </a-form-model>
      </div>
      <div>
        <a-space class="operator">
          <a-button @click="addNew" type="primary">新建</a-button>
          <a-button>批量操作</a-button>
          <a-dropdown>
            <a-menu @click="handleMenuClick" slot="overlay">
              <a-menu-item key="delete">删除</a-menu-item>
              <a-menu-item key="audit">审批</a-menu-item>
            </a-menu>
            <a-button> 更多操作 <a-icon type="down" /> </a-button>
          </a-dropdown>
        </a-space>
        <a-table
          :row-key="(record) => record.Id"
          :data-source="data"
          :loading="loading"
          :pagination="pagination"
          :row-selection="rowSelection"
          @change="handleTableChange"
        >
          <a-table-column
            key="RoleName"
            data-index="RoleName"
            :title="$t('RoleName')"
          />
          <a-table-column
            key="Description"
            data-index="Description"
            :title="$t('Description')"
          />
          <a-table-column key="Sort" data-index="Sort" :title="$t('Sort')" />
          <a-table-column key="action" :title="$t('Action')">
            <template slot-scope="text, record">
              <span>
                <a-button
                  type="primary"
                  @click="Save(record.Id)"
                  :loading="updateLoading"
                  >{{ $t("Public_Update") }}</a-button
                >
              </span>
            </template>
          </a-table-column>
        </a-table>
      </div>
    </a-card>
    <a-modal
      :title="dialogTitle"
      v-model="show"
      :centered="true"
      :maskClosable="false"
      :width="800"
      @cancel="handleCancel"
      @ok="handleConfirm"
    >
      <a-form :form="form">
        <a-form-item :hidden="true">
          <a-input v-decorator="['Id']" />
        </a-form-item>
        <a-form-item :label="$t('RoleName')">
          <a-input
            :placeholder="$t('RoleName')"
            @keydown.native.stop="handleKeyDown"
            v-decorator="[
              'RoleName',
              {
                rules: [
                  { required: true, max: 50, message: this.$t('REQUIRE') },
                ],
              },
            ]"
          />
        </a-form-item>
        <a-form-item :label="$t('Description')">
          <a-input
            :placeholder="$t('Description')"
            @keydown.native.stop="handleKeyDown"
            v-decorator="['Description']"
          />
        </a-form-item>
        <a-form-item :label="$t('Sort')">
          <a-input
            :placeholder="$t('Sort')"
            @keydown.native.stop="handleKeyDown"
            v-decorator="['Sort']"
          />
        </a-form-item>
      </a-form>
    </a-modal>
  </page-header-wrapper>
</template>

<script>
export default {
  name: "RoleList",
  i18n: require("./i18n"),
  components: {},
  data() {
    return {
      advanced: false,
      selectedRows: [],
      queryFormModel: {
        Name: "",
      },
    };
  },
  methods: {
    toggleAdvanced() {
      this.advanced = !this.advanced;
    },
    query() {
      console.log("query", this.queryFormModel);
    },
    ResetQueryForm() {
      this.$refs.queryForm.resetFields();
    },
  },
};
</script>

<style lang="less" scoped>
.search {
  margin-bottom: 54px;
}
.fold {
  width: calc(100% - 216px);
  display: inline-block;
}
.operator {
  margin-bottom: 18px;
}
@media screen and (max-width: 900px) {
  .fold {
    width: 100%;
  }
}
</style>