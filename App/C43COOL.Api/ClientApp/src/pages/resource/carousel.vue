<template>
  <a-card :bordered="false">
    <div class="carousel">
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
        <a-table-column key="Link" data-index="Link" :title="$t('Link')" />
        <a-table-column key="FileId" data-index="FileId" :title="$t('Image')">
          <template slot-scope="text, record">
            <span>
              <vue-preview
                :slides="record.ThumbsList"
                @close="handleClose()"
              ></vue-preview>
            </span>
          </template>
        </a-table-column>
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
        ref="CarouselForm"
        :model="form"
        :rules="rules"
        :label-col="labelCol"
        :wrapper-col="wrapperCol"
      >
        <a-form-model-item label="标题" prop="Title">
          <a-input v-model="form.Title" placeholder="请输入标题" />
        </a-form-model-item>
        <a-form-model-item label="链接" prop="Link">
          <a-input v-model="form.Link" placeholder="请输入链接" />
        </a-form-model-item>
        <a-form-model-item label="排序" prop="Sort">
          <a-input v-model="form.Sort" placeholder="排序" />
        </a-form-model-item>
        <a-form-model-item label="轮播图" prop="FileId">
          <a-upload
            name="avatar"
            list-type="picture-card"
            class="avatar-uploader"
            :show-upload-list="false"
            :before-upload="beforeUpload"
          >
            <img
              v-if="MainImgShow"
              :src="MainImgShow"
              alt="avatar"
              style="max-width: 562px; object-fit: cover"
            />
            <div v-else>
              <a-icon :type="MainLoading ? 'loading' : 'plus'" />
              <div class="ant-upload-text"></div>
            </div>
          </a-upload>
        </a-form-model-item>
      </a-form-model>
    </a-modal>
  </a-card>
</template>

<script>
import CarouselApi from "@/services/carousel.js";
import Tools from "@/utils/tools";
import FileApi from "@/services/file";
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
      data: [],
      loading: false,
      pagination: {
        defaultCurrent: 1,
        defaultPageSize: 20,
        showSizeChanger: true,
        pageSizeOptions: ["50", "100", "500"],
      },
      dialogTitle: "",
      show: false,
      deleteDisable: true,
      deleteLoading: false,
      MainLoading: false,
      MainImgShow: "",
      form: {},
      labelCol: { span: 4 },
      wrapperCol: { span: 14 },
      rules: {
        Title: [
          {
            required: true,
            message: "请输入标题",
            trigger: "change",
          },
        ],
      },
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
      this.Operation = "Create";
      this.show = true;
      this.dialogTitle = this.$t("Create");
      this.$refs.CarouselForm.resetFields();
      this.MainImgShow = "";
    },
    async Modify(id) {
      this.Operation = "Modify";
      this.show = true;
      this.dialogTitle = this.$t("Modify");
      var data = (await CarouselApi.Get(id)).data;
      if (data.FileId !== "") {
        this.MainImgShow = Tools.getImage(data.FileId);
      }
      this.form = data;
    },
    async Delete() {
      console.log("select", this.selectedRowKeys);      
      this.deleteLoading = true;
      await CarouselApi.Delete({ data: this.selectedRowKeys });
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
      var res = (await CarouselApi.Query(params)).data;
      pagination.total = Number(res.TotalElements);
      this.loading = false;
      var result = res.Data || [];
      result.map((f) => {
        f.DateCreated = Tools.formatUtc(f.DateCreated, "yyyy-MM-DD HH:mm:ss");
        f.ThumbsList = f.ThumbsList || [];
        var p = {
          w: 600,
          h: 400,
          src: Tools.getImage(f.FileId),
          msrc: Tools.getImage(f.FileId),
        };
        f.ThumbsList.push(p);
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
        ...filters,
      });
    },
    query() {
      var current = this.pagination.current || this.pagination.defaultCurrent;
      var base = {
        pageSize: this.pagination.defaultPageSize,
        page: current,
      };
      var queryData = { ...base };
      this.fetch(queryData);
    },
    handleKeyDown(e) {
      var eCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
      if (eCode === 13) {
        // 调用对应的方法
        this.handleConfirm();
      }
    },
    // 取消
    handleCancel() {
      this.show = false;
      this.$refs.CarouselForm.resetFields();
    },
    // 新增或修改语言
    async handleConfirm() {
      var that = this;
      this.$refs.CarouselForm.validate(async (valid) => {
        if (valid) {
          if (that.Operation === "Create") {
            await CarouselApi.Create(this.form);
            that.$notification.success({
              message: that.$t("Notiication"),
              description: that.$t("SaveOk"),
            });
          } else {
            await CarouselApi.Modify(that.form);
            that.$notification.success({
              message: that.$t("Notiication"),
              description: that.$t("SaveOk"),
            });
          }
          that.selectedRowKeys = [];
          that.fetch();
          that.show = false;
          that.$refs.CarouselForm.resetFields();
        }
      });
    },
    beforeUpload(file) {
      const isJpgOrPng =
        file.type === "image/jpeg" || file.type === "image/png";
      if (!isJpgOrPng) {
        this.$message.error("You can only upload JPG file!");
      }
      const isLt2M = file.size / 1024 / 1024 < 2;
      if (!isLt2M) {
        this.$message.error("Image must smaller than 2MB!");
      }
      this.MainLoading = true;
      FileApi.CreateFile(file).then((res) => {
        var result = res.data;
        this.MainImgShow = Tools.getImage(result.Id);
        this.MainLoading = false;
        this.form.FileId = result.Id;
        console.log(result);
      });
      return false;
    },
  },
};
</script>
<style>
.carousel img {
  width: 200px;
  height: 100px;
  object-fit: cover;
}
</style>
