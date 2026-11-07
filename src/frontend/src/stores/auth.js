import { defineStore } from 'pinia'
import { ref } from 'vue'
import { api } from '@/services/api'

export const useAuthStore = defineStore('auth', () => {
    let user = ref(null)
    let pendingRequest = null

    async function grabUser(){
        if(user.value !== null)
            return user.value

        if(pendingRequest !== null)
            return pendingRequest

        pendingRequest = api.get('/user/me')
            .then(result => {
                user.value = result
                return result
            })
            .finally(() => {
                pendingRequest = null
            })
        
        return pendingRequest
    }

    function clearUser() {
        user.value = null
    }

    return { grabUser, clearUser }
})