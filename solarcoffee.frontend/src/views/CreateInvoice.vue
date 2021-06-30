<template>
  <div id="invoiceContainer">
    <h1 id="invoiceTitle">Create Invoice</h1>
    <hr />

    <div class="invoice-step" v-if="invoiceStep === 1">
      <h2>Step 1: Select Customer</h2>
      <div v-if="customers.length" class="invoice-step-detail">
        <label for="customer">Customer:</label>
        <select
          v-model="selectedCustomerId"
          class="invoice-customers"
          id="customer"
        >
          <option value="0" disabled>Please select a Customer</option>
          <option v-for="c in customers" :value="c.id" :key="c.id">
            {{ c.firstName + " " + c.lastName }}
          </option>
        </select>
      </div>
    </div>

    <div class="invoice-step" v-if="invoiceStep === 2">
      <h2>Step 2: Create Order</h2>
      <div v-if="inventory.length" class="invoice-step-detail">
        <label for="product">Product:</label>
        <select
          v-model="newItem.product"
          class="invoice-line-items"
          id="product"
        >
          <option value="0" disabled>Please select a Product</option>
          <option v-for="i in inventory" :value="i.product" :key="i.product.id">
            {{ i.product.name }} : {{ i.product.price | price }}
          </option>
        </select>
        <label for="quantity">Quantity:</label>
        <input v-model="newItem.quantity" id="quantity" min="0" />
      </div>

      <div class="invoice-item-actions">
        <solar-button
          :disabled="!newItem.product || !newItem.quantity"
          @button:click="addLineItem"
          >Add Line Item</solar-button
        >
        <solar-button
          :disabled="!lineItems.length"
          @button:click="finalizeOrder"
          >Finalize Order</solar-button
        >
      </div>
      <hr />
      <div class="invoice-order-list" v-if="lineItems.length">
        <div class="running-total">
          <h3>Running Total:</h3>
          {{ runningTotal | price }}
        </div>
        <table class="table">
          <thead>
            <tr>
              <th>Product</th>
              <th>Description</th>
              <th>Qty.</th>
              <th>Price</th>
              <th>Subtotal</th>
            </tr>
          </thead>
          <tr
            v-for="lineItem in lineItems"
            :key="`index_${lineItem.product.id}`"
          >
            <td>{{ lineItem.product.name }}</td>
            <td>{{ lineItem.product.description }}</td>
            <td>{{ lineItem.quantity }}</td>
            <td>{{ lineItem.product.price }}</td>
            <td>{{ (lineItem.product.price * lineItem.quantity) | price }}</td>
          </tr>
        </table>
      </div>
    </div>

    <div class="invoice-step" v-if="invoiceStep === 3"></div>
    <hr />
    <div class="invoice-step-actions">
      <solar-button @button:click="prev" :disabled="!canGoPrev"
        >Previous</solar-button
      >
      <solar-button
        v-if="invoiceStep !== 2"
        @button:click="next"
        :disabled="!canGoNext"
        >Next</solar-button
      >
      <solar-button @button:click="startOver" id="startOver"
        >Start Over</solar-button
      >
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import SolarButton from "@/components/SolarButton.vue";
import { IInvoice, ILineItem } from "@/types/Invoice";
import { ICustomer } from "@/types/Customer";
import { IProductInventory } from "@/types/Product";
import { CustomerService } from "@/services/customer-service";
import { InventoryService } from "@/services/inventory-service";
import InvoiceService from "@/services/invoice-service";

const customerService = new CustomerService();
const inventoryService = new InventoryService();
const invoiceService = new InvoiceService();

@Component({
  name: "CreateInvoice",
  components: { SolarButton },
})
export default class CreateInvoice extends Vue {
  invoiceStep = 1;
  invoice: IInvoice = {
    createdOn: new Date(),
    updatedOn: new Date(),
    customerId: 0,
    lineItems: [],
  };
  customers: ICustomer[] = [];
  selectedCustomerId = 0;
  inventory: IProductInventory[] = [];
  lineItems: ILineItem[] = [];
  newItem: ILineItem = { product: undefined, quantity: 0 };

  addLineItem(): void {
    let newItem: ILineItem = {
      product: this.newItem.product,
      quantity: parseInt(this.newItem.quantity),
    };

    let existingItems = this.lineItems.map((item) => item.product.id);

    if (existingItems.includes(newItem.product.id)) {
      let lineItem = this.lineItems.find(
        (item) => item.product.id === newItem.product.id
      );
      lineItem.quantity = parseInt(lineItem.quantity) + newItem.quantity;
    } else {
      this.lineItems.push(this.newItem);
    }

    this.newItem = { product: undefined, quantity: 0 };
  }

  finalizeOrder(): void {
    this.invoiceStep = 3;
  }

  get runningTotal(): number {
    return this.lineItems.reduce(
      (a, b) => a + b["product"]["price"] * b["quantity"],
      0
    );
  }

  get canGoNext(): boolean {
    if (this.invoiceStep === 1) {
      return this.selectedCustomerId !== 0;
    }

    return !(this.invoiceStep == 2 && this.lineItems.length === 0);
  }

  get canGoPrev(): boolean {
    return this.invoiceStep !== 1;
  }

  startOver(): void {
    this.selectedCustomerId = 0;
    this.lineItems = [];
    this.invoice = { customerId: 0, lineItems: [] };
    this.invoiceStep = 1;
  }

  prev(): void {
    if (this.invoiceStep === 1) {
      return;
    }
    this.invoiceStep -= 1;
  }

  next(): void {
    if (this.invoiceStep === 3) {
      return;
    }
    this.invoiceStep += 1;
  }

  async fetchData(): Promise<void> {
    customerService.getAllCustomers().then((res) => (this.customers = res));
    inventoryService.getInventory().then((res) => (this.inventory = res));
  }

  async created(): Promise<void> {
    await this.fetchData();
  }
}
</script>

<style scoped lang="scss">
@import "src/scss/global.scss";

.invoice-step {
}

.invoice-step-detail {
  margin: 1.2rem;
}

.invoice-step-actions {
  display: flex;
  width: 100%;
}

.invoice-order-list {
  margin-top: 1.2rem;
  padding: 0.8rem;

  .line-item {
    display: flex;
    border-bottom: 1px dashed #ccc;
    padding: 0.8rem;
  }

  .item-col {
    flex-grow: 1;
  }
}

.invoice-item-actions {
  display: flex;
}

.price-pre-tax {
  font-weight: bold;
}

.price-final {
  font-weight: bold;
  color: $solar-green;
}

.running-total {
  display: flex;
  font-weight: bold;
  margin: 1rem;
}

.due {
  font-weight: bold;
}

.invoice-header {
  width: 100%;
  margin-bottom: 1.2rem;
}

.invoice-logo {
  padding-top: 1.4rem;
  text-align: center;

  img {
    width: 280px;
  }
}
</style>
