<script setup>
    import { api } from '@/services/api'
    import { ref, onMounted } from 'vue'
    import RoomCard from '@/components/RoomCard.vue'

    const loading = ref(true)
    const rooms = ref([])
    const error = ref(null)

    onMounted(async () => {
        try {
            rooms.value = await api.get('/room')
        }
        catch (err) {
            error.value = err.message
        }
        finally {
            loading.value = false
        }
    })

    const showCreateModal = ref(false)
    const roomName = ref('')

    async function createRoom(){
        await api.post('/room', { "name": roomName.value })
        showCreateModal.value = false;
        rooms.value = await api.get('/room')
    }

</script>

<template>
    <div class ="app-body">
        <h1 class="title">Alle Räume</h1>

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

        <button class="add-button" type="button" aria-label="Add Room" @click="showCreateModal = true">+</button>
    </div>

    <Transition name="modal" :duration="400">
        <div v-if="showCreateModal" class="room-create-modal">
            <div class="room-create">
                <h2>Raum erstellen</h2>
                <div class="form-group">
                    <label for="Name">Raumname</label>
                    <input
                        id="name"
                        type="text"
                        v-model="roomName"
                        placeholder="Name"
                        required
                    />
                </div>
                <div class="modal-actions">
                    <button class="cancel-button" @click="showCreateModal = false">Abbrechen</button>
                    <button :disabled="!roomName.trim()" class="create-button" @click="createRoom()">Erstellen</button>
                </div>
            </div>
        </div>
    </Transition>

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
.room-create-modal{
    position: fixed;
    inset: 0;
    z-index: 1000;
    left: 20em;

    display: flex;
    justify-content: center;
    align-items: center;
}
.room-create{
    display: flex;
    flex-direction: column;

    width: 18em;
    height: 25em;

    padding: 1.5em;

    background-color: #fff;
    border-radius: 15px; 

    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.3);
}
.room-create h2{
    margin: 0;
    padding: 1em 0;
}
.modal-actions{
    display: flex;
    flex-direction: row;
    margin-top: 75%;
    width: 100%;
    height: 25%;
}
.modal-actions button{
    padding: 5px;
    border-radius: 10px;
    font-size: 15px;
    font-weight: bold;
}
.create-button{
    margin-left: 40%;
    background-color: #06b400;
}
.cancel-button{
    background-color: #ff0000;
}
.form-group{
    display: flex;
    flex-direction: column;
    gap: 3px;
}
.form-group label{
    margin-left: 4px;
    margin-bottom: 2px;
    color: #707070;
    font-size: 20px;
    font-weight: bold;
}
.form-group input{
    min-height: 1.5em;
    border: 1px solid #40516a;
    border-radius: 25px;
    padding: 12px 16px;
}
.form-group input::placeholder{
    font-size: 15px;
}


.modal-enter-active,
.modal-leave-active {
    transition: opacity 0.35s ease;
}

.modal-enter-active .modal-card,
.modal-leave-active .modal-card {
    transition: transform 0.35s ease;
}

/* Background fading */
.modal-enter-active,
.modal-leave-active {
    transition: opacity 0.4s ease;
}

/* Card sliding */
.modal-enter-active .modal-card,
.modal-leave-active .modal-card {
    transition: transform 0.4s ease;
}

/* Initial position (entering) and final position (leaving) */
.modal-enter-from,
.modal-leave-to {
    opacity: 0;
}

.modal-enter-from .modal-card,
.modal-leave-to .modal-card {
    transform: translateY(100vh);
}

/* Final position (entering) and initial position (leaving) */
.modal-enter-to .modal-card,
.modal-leave-from .modal-card {
    transform: translateY(0);
}
</style>