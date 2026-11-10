<script setup>
    import { api } from '@/services/api'
    import { ref, onMounted } from 'vue'
    import ItemCard from '@/components/ItemCard.vue'

    const loading = ref(true)
    const items = ref([])
    const error = ref(null)

    onMounted(async () => {
        try {
            items.value = await api.get('/item')
        }
        catch (err) {
            error.value = err.message
        }
        finally {
            loading.value = false
        }
    })
</script>

<template>
    <div class="app-body">
        <h1 class="title">Alle Gegenstände</h1>

        <p v-if="loading">Loading items...</p>

        <p v-else-if="error">
            Failed to load items: {{ error }}
        </p>

        <div v-else class="item-grid">
            <ItemCard
                v-for="item in items"
                :key="item.id"
                :item="item"
            />
        </div>
    </div>
</template>

<style scoped>
.title{
    font-size: 78px;
    color: #000;
    font-weight: bold;
}
.item-grid{
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 2em;
}
</style>