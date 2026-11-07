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

        <button class="add-button" type="button" aria-label="Add Room">+</button>
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
.add-button {
    position: fixed;
    bottom: 30px;
    right: 30px;

    width: 60px;
    height: 60px;

    background-color: #fff;
    color: #707070;

    font-size: 36px;
    font-weight: 300;

    display: flex;
    justify-content: center;
    align-items: center;

    padding-bottom: 4px;
    line-height: 1;
    appearance: none;

    cursor: pointer;
    border: 1px solid #000;
    border-radius: 50px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.3);

    transition: background-color 0.2s, transform 0.2s;
}
</style>