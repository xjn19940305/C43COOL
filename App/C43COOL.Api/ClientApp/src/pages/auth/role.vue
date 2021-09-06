<template>
  <page-header-wrapper>
    <a-card :bordered="false">
      <div class="table-page-search-wrapper">
        <a-form layout="inline">
          <a-row :gutter="48">
            <a-col :md="8" :sm="24">
              <a-form-item :label="$t('RoleName')">
                <a-input
                  v-model="RoleName"
                  placeholder
                  @keydown.native.stop="handleQuery"
                />
              </a-form-item>
            </a-col>
            <a-col :md="8" :sm="24">
              <a-button type="primary" @click="QueryTable">{{
                $t("Query")
              }}</a-button>
              <a-button style="margin-left: 8px">{{ $t("Reset") }}</a-button>
            </a-col>
          </a-row>
        </a-form>
      </div>
      <a-form layout="inline" style="margin-bottom: 10px">
        <a-button type="primary" @click="Add()">
          {{ $t("Public_Add") }}
        </a-button>
        <a-button
          :disabled="deleteDisable"
          type="danger"
          style="margin-left: 15px"
          @click="Delete()"
          :loading="deleteLoading"
        >
          {{ $t("Public_Delete") }}</a-button
        >
      </a-form>
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
