<script setup>
    defineProps({
        user: {
            type: Object,
            required: true
        }
    })
</script>

<template>
    <RouterLink :to="`/user/${user.id}`" :aria-label="`Open ${user.name}`">
        <div class="card">
            <div class="descr">
                <h2>{{ user.name }}</h2>
            </div>
            <svg
                class="arrow-icon"
                viewBox="0 0 48 48"
                fill="none"
                stroke="white"
                stroke-width="2.5"
                stroke-linecap="round"
                stroke-linejoin="round"
                aria-hidden="true"
            >
                <circle cx="24" cy="24" r="20"/>

                <path d="M15 24 H33 M26 17 L33 24 L26 31"/>
            </svg>
        </div>
    </RouterLink>
</template>

<style scoped>
.card{
    display: flex;
    background-color: #707070;
    color: #fff;
    height: 8em;
    border: 1px solid #fff;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.3);
    padding: 0.5em 2em;
}
.descr{
    margin-top: 1.25em;
}
.descr h2{
    font-size: 18px;
    margin: 0;
    width: 10em;
}
.card svg{
    margin-top: 3em;
    width: 25%;
}

a{
    text-decoration: none;
}
</style>