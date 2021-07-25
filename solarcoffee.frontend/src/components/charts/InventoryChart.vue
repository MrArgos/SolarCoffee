<template>
  <div v-if="isTimelineBuilt">
    <apexchart
      type="area"
      width="800"
      heigth="300"
      :options="options"
      :series="series"
    >
    </apexchart>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { IInventoryTimeline } from "@/types/InventoryGraph";
import { Get, Sync } from "vuex-pathify";

import VueApexCharts from "vue-apexcharts";
Vue.component("apexchart", VueApexCharts);

@Component({
  name: "InventoryChart",
  components: {},
})
export default class InventoryChart extends Vue {
  @Sync("snapshotTimeline")
  snapshotTimeline?: IInventoryTimeline;

  @Get("isTimelineBuilt")
  isTimelineBuilt?: boolean;

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
    let res = this.snapshotTimeline.productInventorySnapshots.map(
      (snapshot) => ({
        name: snapshot.productId,
        data: snapshot.quantityOnHand,
      })
    );
    console.log(":: series ::", res);
    return res;
  }

  async created() {
    await this.$store.dispatch("assignSnapshots");
  }
}
</script>

<style scoped lang="scss"></style>
