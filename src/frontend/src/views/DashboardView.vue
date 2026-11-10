<script setup>
    import { api } from '@/services/api'
    import { ref, onMounted } from 'vue'
    import RoomCard from '@/components/RoomCard.vue'

    const rooms = ref([])
    const loading = ref(true)
    const error = ref(null)

    onMounted(async () => {
        try {
            rooms.value = await api.get('/room/mine')
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
    <div class ="app-body">
        <h1 class="title">Dashboard</h1>

        <p v-if="loading">Loading rooms...</p>

        <p v-else-if="error">
            Failed to load rooms: {{ error }}
        </p>

        <div v-else class="room-grid">
            <RoomCard
                v-for="room in rooms"
                :key="room.id"
                :room="room"
            />
        </div>
    </div>

</template>

<style scoped>
.app-body{
    padding: 50px 25px;
}
.title{
    font-size: 78px;
    color: #000;
    font-weight: bold;
}
.room-grid{
    display: grid;
    grid-template-columns: repeat(5, 1fr);
    gap: 2em;
}
</style>