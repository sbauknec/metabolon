<script setup>
    import { api } from '@/services/api'
    import { ref, onMounted } from 'vue'
    import DeviceCard from '@/components/DeviceCard.vue'

    const loading = ref(true)
    const devices = ref([])
    const error = ref(null)

    onMounted(async () => {
        try {
            devices.value = await api.get('/device')
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
        <h1 class="title">Alle Geräte</h1>

        <p v-if="loading">Loading devices...</p>

        <p v-else-if="error">
            Failed to load devices: {{ error }}
        </p>

        <div v-else class="device-grid">
            <DeviceCard
                v-for="device in devices"
                :key="device.id"
                :device="device"
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
.device-grid{
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 4em;
}
</style>