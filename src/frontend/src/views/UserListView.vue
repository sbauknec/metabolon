<script setup>
    import { api } from '@/services/api'
    import { ref, onMounted } from 'vue'
    import UserCard from '@/components/UserCard.vue'

    const loading = ref(true)
    const users = ref([])
    const error = ref(null)

    onMounted(async () => {
        try {
            users.value = await api.get('/user')
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
        <h1 class="title">Alle Benutzer</h1>

        <p v-if="loading">Loading items...</p>

        <p v-else-if="error">
            Failed to load items: {{ error }}
        </p>

        <div v-else class="user-grid">
            <UserCard
                v-for="user in users"
                :key="user.id"
                :user="user"
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
.user-grid{
    display: grid;
    grid-template-columns: repeat(5, 1fr);
    gap: 4em;
}
</style>