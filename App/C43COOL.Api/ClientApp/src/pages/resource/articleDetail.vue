<template>
  <a-card :bordered="false">
    <div style="margin-bottom: 15px">
      <a-breadcrumb separator=">">
        <a-breadcrumb-item href="dashboard">{{
          $t("Index")
        }}</a-breadcrumb-item>
        <a-breadcrumb-item href=""> 资源 </a-breadcrumb-item>
        <a-breadcrumb-item href="/resource/article">
          文章管理
        </a-breadcrumb-item>
        <a-breadcrumb-item>文章详情</a-breadcrumb-item>
      </a-breadcrumb>
    </div>
    <a-form-model
      ref="ArticleForm"
      :model="form"
      :rules="rules"
      :label-col="labelCol"
      :wrapper-col="wrapperCol"
    >
      <a-form-model-item ref="Title" label="标题" prop="Title">
        <a-input v-model="form.Title" placeholder="请输入标题" />
      </a-form-model-item>
      <a-form-model-item
        ref="Category"
        :label="$t('Article_Category')"
        prop="CategoryId"
      >
        <a-tree-select
          :tree-data="treeData"
          :placeholder="$t('CHOOSEONE')"
          tree-default-expand-all
          @select="GetTreeValue"
          v-model="form.CategoryId"
          :defaultExpandAllRows="true"
        >
          <span slot="title" slot-scope="{ value }" style="color: #08c"
            >Child Node1 {{ value }}</span
          >
        </a-tree-select>
      </a-form-model-item>
      <a-form-model-item ref="Author" label="作者" prop="Author">
        <a-input v-model="form.Author" placeholder="作者" />
      </a-form-model-item>
      <a-form-model-item ref="Description" label="描述" prop="Description">
        <a-input v-model="form.Description" placeholder="描述" />
      </a-form-model-item>
      <a-form-model-item ref="Img" :label="$t('ThumImage')" prop="Img">
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
      <a-form-model-item>
        <div id="div1"></div>
      </a-form-model-item>
      <a-form-model-item :wrapper-col="{ span: 14, offset: 4 }">
        <a-button type="primary" @click="onSubmit"> {{ $t("Save") }} </a-button>
        <a-button style="margin-left: 10px" @click="resetForm">
          {{ $t("Reset") }}
        </a-button>
      </a-form-model-item>
    </a-form-model>
  </a-card>
</template>
<script>
import Tools from "@/utils/tools";
import FileApi from "@/services/file";
import E from "wangeditor";
import ArticleApi from "@/services/article";
import CategoryApi from "@/services/category";
export default {
  name: "ArticleDetail",
  i18n: require("../i18n"),
  data() {
    return {
      Id: 0,
      op: "create",
      editor: null,
      MainLoading: false,
      MainImgShow: "",
      labelCol: { span: 4 },
      wrapperCol: { span: 14 },
      other: "",
      treeData: [],
      form: {
        Title: "",
        Content: "",
        Description: "",
        ThumImg: "",
        CategoryId: "",
      },
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
  mounted() {
    this.editor = new E("#div1");
    this.editor.config.onchange = (html) => {
      this.form.Content = html;
    };
    // 配置 server 接口地址
    this.editor.config.customUploadImg = function (resultFiles, insertImgFn) {
      resultFiles.forEach((file) => {
        console.log(file);
        FileApi.CreateFile(file).then((result) => {
          var res = result.data;
          console.log(res);
          insertImgFn(Tools.getImage(res.Id));
        });
      });
    };
    this.editor.create();
    this.LoadSelectTree();
    this.Init();
  },
  methods: {
    Init() {
      this.Id = this.$route.query.Id || 0;
      if (this.Id !== 0) {
        this.op = "modify";
        ArticleApi.Get(this.Id).then((response) => {
          var res = response.data;
          res.CategoryId = res.CategoryId.toString();
          if (res.ThumImg !== "") {
            this.MainImgShow = Tools.getImage(res.ThumImg);
          }
          this.editor.txt.html(res.Content);
          this.form = res;
        });
      }
    },
    async LoadSelectTree() {
      var res = (await CategoryApi.Query()).data;
      this.getTreeSelectList(res.Data, this.treeData, "");
    },
    getTreeSelectList(list, tree, parentId) {
      list.forEach((item) => {
        // 判断是否为父级菜单
        if (item.ParentId === parentId) {
          const child = {
            title: item.Name,
            key: item.Id + "",
            value: item.Id + "",
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
    onSubmit() {
      this.$refs.ArticleForm.validate(async (valid) => {
        if (valid) {
          if (this.op === "create") {
            await ArticleApi.Create(this.form);
            this.$notification.success({
              message: this.$t("Notiication"),
              description: this.$t("SaveOk"),
            });
          } else {
            await ArticleApi.Modify(this.form);
            this.$notification.success({
              message: this.$t("Notiication"),
              description: this.$t("SaveOk"),
            });
          }
          this.$router.push({ path: "/resource/article" });
        }
      });
    },
    resetForm() {
      this.$refs.ArticleForm.resetFields();
    },
    getBase64(img, callback) {
      const reader = new FileReader();
      reader.addEventListener("load", () => callback(reader.result));
      reader.readAsDataURL(img);
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
        this.form.ThumImg = result.Id;
        console.log(result);
      });
      return false;
    },
  },
};
</script>