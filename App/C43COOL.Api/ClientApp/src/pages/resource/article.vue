<template>
  <a-card :bordered="false">
    <div>
      <a-form-model layout="horizontal" ref="queryForm" :model="queryFormModel">
        <div>
          <a-row>
            <a-col :md="8" :sm="24">
              <a-form-model-item
                ref="Title"
                :label="$t('Title')"
                :labelCol="{ span: 6 }"
                :wrapperCol="{ span: 12, offset: 0 }"
                prop="Title"
              >
                <a-input v-model="queryFormModel.Title" />
              </a-form-model-item>
            </a-col>
            <a-col :md="8" :sm="24">
              <a-form-model-item
                ref="Author"
                :label="$t('Author')"
                :labelCol="{ span: 6 }"
                :wrapperCol="{ span: 12, offset: 0 }"
                prop="Author"
              >
                <a-input v-model="queryFormModel.Author" />
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
        <a-table-column key="Title" data-index="Title" :title="$t('Title')" />
        <a-table-column
          key="Author"
          data-index="Author"
          :title="$t('Author')"
        />
        <a-table-column
          key="Description"
          data-index="Description"
          :title="$t('Description')"
        />
        <a-table-column
          key="LinkCount"
          data-index="LinkCount"
          :title="$t('LinkCount')"
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
            </span>
          </template>
        </a-table-column>
      </a-table>
    </div>
  </a-card>
</template>

<script>
import ArticleApi from "@/services/article.js";
import { formatUtc } from "@/utils/timeformat";
export default {
  name: "ArticlePage",
  i18n: require("../i18n"),
  mounted() {
    this.fetch();
  },
  data() {
    return {
      Operation: "",
      selectedRowKeys: [],
      queryFormModel: {
        Title: "",
        Author: "",
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
    };
  },
  components: {},
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
      this.$router.push({ path: "/resource/articleDetail" });
    },
    async Modify(id) {
      console.log(id);
    },
    async Delete() {
      console.log("select", this.selectedRowKeys);
      var postData = this.selectedRowKeys
        .map((f) => {
          return "ids=" + f;
        })
        .join("&")
        .toString();
      console.log(postData);
      this.deleteLoading = true;
      await ArticleApi.Delete(postData);
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
      var res = (await ArticleApi.Query(params)).data;
      pagination.total = Number(res.TotalElements);
      this.loading = false;
      var result = res.Data || [];
      result.map((f) => {
        f.DateCreated = formatUtc(f.DateCreated, "yyyy-MM-DD HH:mm:ss");
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
    handleKeyDown(e) {
      var eCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
      if (eCode === 13) {
        // 调用对应的方法
        this.handleConfirm();
      }
    },
  },
};
</script>