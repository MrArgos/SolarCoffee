<template>
  <div class="inventory-container">
    <h1 id="inventoryTitle">Inventory Dashboard</h1>
    <hr />

    <div class="inventory-actions">
      <solar-button @click.native="showNewProductModal" id="addNewBtn">
        Add New Item
      </solar-button>
      <solar-button @click.native="showShipmentModal" id="receiveShipmentBtn">
        Receive Shipment
      </solar-button>
    </div>

    <table id="inventoryTable" class="table">
      <tr>
        <th>Item</th>
        <th>Quantity On Hand</th>
        <th>Unit Price</th>
        <th>Taxable</th>
        <th>Delete</th>
      </tr>

      <tr v-for="item in inventory" :key="item.id">
        <td>{{ item.product.name }}</td>
        <td>{{ item.quantityOnHand }}</td>
        <td>{{ item.product.price | price }}</td>
        <td>
          <span v-if="item.product.isTaxable">Yes</span>
          <span v-else>No</span>
        </td>
        <td>
          <div>X</div>
        </td>
      </tr>
    </table>
    <new-product-modal
      v-if="isNewProductVisible"
      @save:product="saveNewProduct"
      @close="closeModals"
    />
    <shipment-modal
      v-if="isShipmentVisible"
      :inventory="inventory"
      @save:shipment="saveNewShipment"
      @close="closeModals"
    />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { IProduct, IProductInventory } from "@/types/Product";
import { IShipment } from "@/types/Shipment";
import SolarButton from "@/components/SolarButton.vue";
import NewProductModal from "@/components/Modals/NewProductModal.vue";
import ShipmentModal from "@/components/Modals/ShipmentModal.vue";

@Component({
  name: "Inventory",
  components: { SolarButton, NewProductModal, ShipmentModal },
})
export default class Inventory extends Vue {
  isNewProductVisible = false;
  isShipmentVisible = false;

  inventory: IProductInventory[] = [
    {
      id: 1,
      product: {
        id: 1,
        name: "Some Product",
        description: "Good stuff",
        price: 100,
        createdOn: new Date(),
        updatedOn: new Date(),
        isTaxable: true,
        isArchived: false,
      },
      quantityOnHand: 150,
      idealQuantity: 200,
    },
    {
      id: 2,
      product: {
        id: 2,
        name: "Another Product",
        description: "Good stuff v2",
        price: 110,
        createdOn: new Date(),
        updatedOn: new Date(),
        isTaxable: false,
        isArchived: false,
      },
      quantityOnHand: 130,
      idealQuantity: 180,
    },
  ];

  closeModals(): void {
    this.isShipmentVisible = false;
    this.isNewProductVisible = false;
  }

  showNewProductModal(): void {
    this.isShipmentVisible = false;
    this.isNewProductVisible = true;
  }

  showShipmentModal(): void {
    this.isNewProductVisible = false;
    this.isShipmentVisible = true;
  }

  saveNewProduct(product: IProduct): void {
    console.log("saveNewProduct");
    console.log(product);
  }

  saveNewShipment(shipment: IShipment): void {
    console.log("saveNewShipment");
    console.log(shipment);
  }
}
</script>

<style scoped lang="scss"></style>
