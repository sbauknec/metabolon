<script setup>
    import { useAuthStore } from '@/stores/auth';
    import { onMounted } from 'vue';
    import { ref } from 'vue';

    const auth = useAuthStore()
    const user = ref(null)

    onMounted(async () => {
        try{
            user.value = await auth.grabUser()
        }
        catch(error){
            console.error("Failed to grab user:", error)
        }
    })

</script>

<template>
  <aside class="sidebar">
        <div class="branding">
            <div class="branding-head">
                <img src="../assets/Logo-metabolon.svg" class="icon"/>
            </div>
        </div>

        <div class="sidebar-body">
            <p>Willkommen im Gerätearchiv</p>
            <p class="uname" v-if="user">{{ user.name }}</p>

            <nav class="navigation">
                <RouterLink to="/dashboard">Dashboard</RouterLink>
                <RouterLink to="/roomlist">Räume</RouterLink>
                <RouterLink to="/devicelist">Geräte</RouterLink>
                <RouterLink to="/itemlist">Gegenstände</RouterLink>
                <RouterLink to="/userlist">Nutzer</RouterLink>
            </nav>
        </div>

        <div class="sidebar-footer">
            Metabolon Verwaltung und Informationssystem
            <RouterLink to="/">Shortcut Login</RouterLink>
        </div>
    </aside>
</template>

<style scoped>
.sidebar{
    width:12%;
    min-height: 100%;
    flex-shrink: 0;

    background: #ffffff;
    padding: 40px 40px;

    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    
    box-shadow: 0 12px 40px rgba(0, 0, 0, 0.2);
}
.branding-head {
    display: flex;
    align-items: center;
    gap: 12px;
    padding-bottom: 3em;
    border-bottom: 1px solid #000000;
    margin-bottom: 6em;
}
.icon{
    width : 100%;
    margin: 0 auto;
}
.sidebar-body{
    flex-direction: column;
    font-size: 20px;
    color: #62676e;
}
.sidebar-body p{
    margin: 0;
}
.uname{
    font-weight: bold;
}
.navigation{
    width: 100%;
    margin-top: 2em;
}
.navigation a{
    font-size: 18px;
    display: block;
    width: 100%;
    color: #62676e;
    text-decoration: none;
    border-bottom: 1px solid #000;
    padding: 10px 0;
}
.navigation a:hover{
    font-weight: bold;
}
.sidebar-footer{
    color: #8295aa;
    font-size: 12px;
    margin-top: auto;
}

</style>