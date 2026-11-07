<script setup>
    import { ref } from 'vue'
    import { api } from '@/services/api'
    import { useRouter } from 'vue-router'
 
    const router = useRouter()

    const email = ref('')
    const password = ref('')

    async function handleLogin() {

        await api.post('/User/login', {
            Mail: email.value,
            Password: password.value
        })

        router.push('/dashboard')
    }
</script>

<template>
  <main class="login-page">
    <aside class="sidebar">
        <div class="branding">
            <div class="branding-head">
                <img src="../assets/Logo-metabolon.svg" class="icon"/>
            </div>
            <p>Willkommen im Gerätearchiv <br> des :metabolon Instituts.</p>
            <p style="font-weight:bold;">Bitte melden Sie sich an um fortzufahren.</p>
        </div>

        

        <div class="sidebar-footer">
            Metabolon Verwaltung und Informationssystem
        </div>
    </aside>

    <section class="login-form">
        <form class="login-card" @submit.prevent="handleLogin">
            <div class="form-group">
                <label for="email">E-Mail Adresse</label>
                <input
                    id="email"
                    tpye="email"
                    v-model="email"
                    placeholder="name@example.com"
                    autocomplete="username"
                    required
                />
            </div>
            <div class="form-group">
                <label for="password">Passwort</label>
                <input
                    id="password"
                    type="password"
                    v-model="password"
                    placeholder="Passwort"
                    autocomplete="current-password"
                    required
                />
            </div>

            <div class="form-options">
                <button type="button" class="forgot-password">
                    Passwort vergessen?
                </button>
            </div>
            <div class="login-button">
                <button type="submit" class="login-button-btn">
                    EINLOGGEN
                </button>
            </div>
        </form>
    </section>
  </main>
</template>

<style scoped>
.login-page {
    display: flex;
    min-height: 100vh;
    width: 100%;

    background: #d4d4d4;
}
.login-form{
    width: 40em;
    height: 20em;

    background: #fff;

    padding: 36px;

    display: flex;
    margin: auto auto;

    box-shadow: 0 12px 40px rgba(0, 0, 0, 0.2);
}
.login-card{
    width: 100%;
    margin: 0 auto;
}
.form-group{
    display: flex;
    flex-direction: column;
    gap: 3px;
    margin: 2em auto;
}
.form-group label{
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
.form-options{
    display: flex;
    justify-content: flex-end;
    width: 100%;
    margin-top: -1.5em;
}
.form-options button{
    border: none;
    background: none;
    color: #304056;
    text-decoration: underline;
}
.form-options button:hover{
    color: #5781bb
}
.login-button{
    margin-top: 2.5em;
    display: flex;
    justify-content: flex-end;
    height: 100%
}
.login-button-btn{
    width: 35%;
    height: 15%;
    border: 2px solid #000;
    border-radius: 10px;
    background: #fff;
    font-size: 20px;
    font-weight: bold;
}



.sidebar{
    width:12%;
    min-height: 100%;
    flex-shrink: 0;

    background: #ffffff;
    padding: 40px 40px;

    display: flex;
    flex-direction: column;
    justify-content: space-between;
    
    box-shadow: 0 12px 40px rgba(0, 0, 0, 0.2);
}
.branding{
    color: #304056;
}
.branding p{
    font-size: 20px;
    color: #62676e;
    padding: 10px 0;
}
.branding-head {
    display: flex;
    align-items: center;
    gap: 12px;
    padding-bottom: 3em;
    border-bottom: 1px solid #000000;
    margin-bottom: 6em;
}
.branding h1{
    margin: 0;
    font-size: 28px;
}
.sidebar-footer{
    color: #8295aa;
    font-size: 12px;
}
.icon{
    width : 100%;
    margin: 0 auto;
}
</style>
