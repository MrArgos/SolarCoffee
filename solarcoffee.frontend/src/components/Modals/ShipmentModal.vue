<template>
  <solar-modal>
    <template v-slot:header> Receive Shipment </template>
    <template v-slot:body>
      <label for="product"> Product Received: </label>
      <select v-model="selectedProduct" class="shipmentItems" id="product">
        <option disabled value="" id="selectOne">Please select one</option>
        <option v-for="item in inventory" :value="item" :key="item.product.id">
          {{ item.product.name }}
        </option>
      </select>

      <label for="qtyReceived">
        <br />
        Quantity Received
      </label>
      <input type="number" id="qtyReceived" v-model="qtyReceived" />
    </template>

    <template v-slot:footer>
      <solar-button
        type="button"
        @button:click="save"
        aria-label="save new shipment"
      >
        Save Received Shipment
      </solar-button>
      <solar-button
        type="button"
        @button:click="close"
        aria-label="Close modal"
      >
        Close
      </solar-button>
    </template>
  </solar-modal>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import SolarModal from "@/components/Modals/SolarModal.vue";
import SolarButton from "@/components/SolarButton.vue";
import { IProduct, IProductInventory } from "@/types/Product";
import { IShipment } from "@/types/Shipment";

@Component({
  name: "ShipmentModal",
  components: { SolarButton, SolarModal },
})
export default class ShipmentModal extends Vue {
  @Prop({ required: true, type: Array as () => IProductInventory[] })
  inventory!: IProductInventory;

  selectedProduct: IProduct = {
    id: 0,
    name: "",
    description: "",
    price: 0,
    createdOn: new Date(),
    updatedOn: new Date(),
    isTaxable: false,
    isArchived: false,
  };

  qtyReceived = 0;

  close() {
    this.$emit("close");
  }

  save() {
    let shipment: IShipment = {
      productId: this.selectedProduct.id,
      adjustment: this.qtyReceived,
    };
    this.$emit("save:shipment", shipment);
  }
}
</script>

<style scoped lang="scss">
@import "src/scss/global.scss";
</style>
