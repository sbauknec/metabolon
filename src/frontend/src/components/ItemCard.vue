<script setup>
    defineProps({
        item: {
            type: Object,
            required: true
        }
    })
</script>

<template>
    <RouterLink :to="`/item/${item.id}`" :aria-label="`Open ${item.name}`">
        <div class="card">
            <div class="descr">
                <h2>{{ item.name }}</h2>
                <p>{{ item.amount }}</p>
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
    height: 6em;
    border: 1px solid #fff;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.3);
    padding: 0.5em 2em;
}
.descr{
    margin-top: 1.25em;
}
.descr h2{
    margin: 0;
}
.descr p{
    margin: 0;
    font-family: "Segoe UI", sans-serif;
    font-size: 22px;
    font-style: italic;
    font-weight: 300;
}
.card svg{
    width: 15%;
    margin-left: 15em;
}

a{
    text-decoration: none;
}
</style>