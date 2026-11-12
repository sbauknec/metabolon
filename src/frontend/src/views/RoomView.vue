<script setup>
    import { onMounted, ref } from 'vue'
    import { api } from '@/services/api'
    import { useRouter, useRoute } from 'vue-router'
    import DeviceCard from '@/components/DeviceCard.vue'
    import ItemCard from '@/components/ItemCard.vue'

    const room = ref(null)
    const loading = ref(true)
    const error = ref(null)

    onMounted(async () => {
        const roomId = useRoute().params.id
        try {
            room.value = await api.get(`/room/${roomId}`)
            console.log(room.value)
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
            <div class="room-header">
                <div class="room-cred">
                    <p class="supervisor">{{ room.supervisor.name }}</p>
                    <h1 class="title">{{ room.name }}</h1>
                </div>
                <div class="documents">
                    <RouterLink 
                        v-for="document in room.documents" 
                        :key="document.id" 
                        :to='`/document/${document.id}`'>{{ document.originalName }}</RouterLink>
                </div>
            </div>
            <div class="room-devices">
                <div class="subgroup-header">
                    <p>Geräte</p>
                    <button>Sortieren nach:</button>
                </div>
                <div class="subgroup-grid">
                        <DeviceCard
                            v-for="device in room.devices"
                            :key="device.id"
                            :device="device"
                        />
                    </div>
            </div>
            <div class="room-items">
                <div class="subgroup-header">
                    <p>Material</p>
                    <button>Sortieren nach:</button>
                </div>
                <div class="subgroup-grid">
                        <ItemCard
                            v-for="item in room.materials"
                            :key="item.id"
                            :item="item"
                        />
                    </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.room-header{
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
    padding: 0;
    margin: 0;
}
.room-header p, h1, div{
    margin: 0;
} 
.room-header p{
    color: #707070;
}
.title{
    font-size: 78px;
    color: #000;
    font-weight: bold;
}
.documents{
    margin-top: 2em;
    margin-left: 60%;
    border-top: 1px solid #707070;
}
.documents a{
    display: block;
    text-align: right;
    width: 15em;
    text-decoration: none;
    color: #707070;
    font-size: 20px;
    padding: 5px;
    border-bottom: 1px solid #707070;
}

.subgroup-header{
    display: flex;
    flex-directioN: row;
    justify-content: space-between;
    align-items: center;

    margin-top: 5em;
    margin-bottom: 2em;
    width: 100%;
    border-bottom: 1px solid #707070;
}
.subgroup-header p{
    margin: 0;
    padding: 5px 0;

    color: #707070;
    font-size: 16px;
}
.subgroup-header button{
    padding: 0;
}

.subgroup-grid{
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 4em;
}
</style>