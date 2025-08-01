<script setup lang="ts">
const props = defineProps<{ subTitle?: string }>()

</script>
<template>
  <main class="not-found-page__wrapper">
    <a-result status="404" title="404" :sub-title="subTitle ?? 'Page not found'">
      <template #extra>
      </template>
    </a-result>
  </main>
</template>
<style scoped lang="scss">
.not-found-page__wrapper {
  :deep(.ant-result) {
    font-family: 'Sofia Sans Extra Condensed', sans-serif;
    .ant-result-title {
      font-size: 40px;
    }
    .ant-result-subtitle {
      font-size: 22px;
    }
  }
}
</style>