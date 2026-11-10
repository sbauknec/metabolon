<script setup>
    defineProps({
        room: {
            type: Object,
            required: true
        }
    })
</script>

<template>
    <RouterLink :to="`/room/${room.id}`">
        <div class="card">
            <h2>{{ room.name }}</h2>
            <img src="../assets/csm_Lagerarten_3c4e8dd43f.jpg" />
            <p>Kurzbeschreibung [...]</p>
            <p>Geräte</p>
            <p>Materialien</p>
            <p>Dokumente</p>
        </div>
    </RouterLink>
</template>

<style scoped>
.card{
    width: 18em;
    min-height: 21em;
    background-color: #fff;
    box-shadow: 0 10px 10px rgba(0, 0, 0, 0.2);
    color: #707070;
}
.card:hover{
    background-color: #000;
    color: #fff;
}
.card h2{
    background-color: #000;
    margin: 0;
    color: #fff;
    padding: 15px 22px;
    font-size: 22px;
}
.card img{
    width: 100%;
    max-height: 4.5em;
}
.card p{
    margin: 0 22px;
    padding: 10px 0;
    border-bottom: 1px solid #707070;
}

a{
    text-decoration: none;
}
</style>