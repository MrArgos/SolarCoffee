<template>
  <div>
    <apexchart
      type="area"
      width="100%"
      heigth="300"
      :options="options"
      :series="series"
    >
    </apexchart>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import VueApexCharts from "vue-apexcharts";
import { IInventoryTimeline } from "@/types/InventoryGraph";

@Component({
  name: "InventoryChart",
  components: {},
})


export default class InventoryCharts extends Vue {

  snapshotTimeline: IInventoryTimeline;

  get options() {
    return {
      dataLabels: { enabled: false },
      fill: { type: "gradient" },
      stroke: { curve: "smooth" },
      xaxis: {
        categories: this.snapshotTimeline.timeline,
        type: "datetime",
      },
    };
  }
}
</script>

<style scoped lang="scss"></style>
