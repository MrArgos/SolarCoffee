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
import { Get, Sync } from "vuex-pathify";

@Component({
  name: "InventoryChart",
  components: {},
})
export default class InventoryChart extends Vue {
  @Sync("snapshotTimeline")
  snapshotTimeline: IInventoryTimeline;

  @Get("isTimelineBuilt")
  timelineBuilt?: boolean;

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

  get series() {
    return this.snapshotTimeline.productInventorySnapshots.map((snapshot) => ({
      name: snapshot.productId,
      data: snapshot.quantityOnHand,
    }));
  }

  async created(): void {
    await this.$store.dispatch("assignSnapshots");
  }
}
</script>

<style scoped lang="scss"></style>
