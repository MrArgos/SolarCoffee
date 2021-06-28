<template>
  <solar-modal>
    <template v-slot:header> Add New Product </template>

    <template v-slot:body>
      <ul class="newProduct">
        <li>
          <label for="isTaxable">Is this product taxable?</label>
          <input
            type="checkbox"
            id="isTaxable"
            v-model="newProduct.isTaxable"
          />
        </li>

        <li>
          <label for="productName">Name</label>
          <input type="text" id="productName" v-model="newProduct.name" />
        </li>

        <li>
          <label for="productDesc">Description</label>
          <input
            type="text"
            id="productDesc"
            v-model="newProduct.description"
          />
        </li>

        <li>
          <label for="productPrice">Price (EUR)</label>
          <input type="number" id="productPrice" v-model="newProduct.price" />
        </li>
      </ul>
    </template>

    <template v-slot:footer>
      <solar-button
        type="button"
        @click.native="save"
        aria-label="save new item"
      >
        Save Product
      </solar-button>
      <solar-button
        type="button"
        @click.native="close"
        aria-label="close modal"
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

@Component({
  name: "NewProductModal",
  components: { SolarButton, SolarModal },
})
export default class NewProductModal extends Vue {
  newProduct: IProduct = {
    id: 0,
    name: "",
    description: "",
    price: 0,
    createdOn: new Date(),
    updatedOn: new Date(),
    isTaxable: false,
    isArchived: false,
  };

  close() {
    this.$emit("close");
  }

  save() {
    this.$emit("save:product", this.newProduct);
  }
}
</script>

<style scoped lang="scss">
@import "src/scss/global.scss";
.newProduct {
  list-style: none;
  padding: 0;
  margin: 0;
}
</style>
