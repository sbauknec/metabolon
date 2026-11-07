<script setup>
    import Sidebar from '@/components/Sidebar.vue'
    import { RouterView } from 'vue-router'
</script>

<template>
  <div class="main-layout">
    <Sidebar />

    <main class="page-content">
      <RouterView />
    </main>
  </div>
</template>

<style scoped>
.main-layout {
  display: flex;
  min-height: 100vh;
  background: #fff;
}

.page-content {
  flex: 1;
  min-width: 0;
  padding: 40px;
}
</style>