<template>
  <div class="inventory-container">
    <h1 id="inventoryTitle">Inventory Dashboard</h1>
    <hr />
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
        <td>{{ item.product.price | price}}</td>
        <td>
          <span v-if="item.product.isTaxable">Yes</span>
          <span v-else>No</span>
        </td>
        <td>
          <div>X</div>
        </td>
      </tr>
    </table>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { IProductInventory } from "@/types/Product";

@Component({
  name: "Inventory",
  components: {}
})
export default class Inventory extends Vue {
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
        isArchived: false
      },
      quantityOnHand: 150,
      idealQuantity: 200
    },
    {
      id: 2,
      product: {
        id: 1,
        name: "Another Product",
        description: "Good stuff v2",
        price: 110,
        createdOn: new Date(),
        updatedOn: new Date(),
        isTaxable: false,
        isArchived: false
      },
      quantityOnHand: 130,
      idealQuantity: 180
    }
  ];
}
</script>

<style scoped lang="scss"></style>
