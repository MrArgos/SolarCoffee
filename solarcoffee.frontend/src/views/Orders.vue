<template>
  <div id="ordersContainer">
    <h1 id="ordersTitle">Orders</h1>
    <hr />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import SolarButton from "@/components/SolarButton.vue";

@Component({
  name: "Orders",
  components: { SolarButton },
})
export default class Orders extends Vue {}
</script>

<style scoped></style>
