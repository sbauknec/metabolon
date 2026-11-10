<script setup>
    import { onMounted, ref } from 'vue'
    import { api } from '@/services/api'
    import { useRouter, useRoute } from 'vue-router'

    const room = ref(null)
    const loading = ref(true)
    const error = ref(null)

    onMounted(async () => {
        const roomId = useRoute().params.id
        try {
            room.value = await api.get(`/room/${roomId}`)
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
        <p v-if="loading">Loading rooms...</p>

        <p v-else-if="error">
            Failed to load rooms: {{ error }}
        </p>

        <div v-else class="room">
            <p>Loaded</p>
        </div>
    </div>
</template>

<style scoped>
</style>