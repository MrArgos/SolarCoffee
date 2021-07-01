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
        <input v-model="newItem.quantity" type="number" id="quantity" min="0" />
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
              <th>Quantity</th>
              <th>Price</th>
              <th>Subtotal</th>
              <th>Remove</th>
            </tr>
          </thead>
          <tr
            v-for="lineItem in lineItems"
            :key="`index_${lineItem.product.id}`"
          >
            <td>{{ lineItem.product.name }}</td>
            <td>{{ lineItem.product.description }}</td>
            <td class="item-quantity">
              <span
                v-if="lineItem.quantity > 1"
                class="lni lni-circle-minus item-quantity-icons"
                @click="lineItem.quantity = Number(lineItem.quantity) - 1"
              ></span>
              {{ lineItem.quantity }}
              <span
                class="lni lni-circle-plus item-quantity-icons"
                @click="lineItem.quantity = Number(lineItem.quantity) + 1"
              ></span>
            </td>
            <td>{{ lineItem.product.price }}</td>
            <td>{{ (lineItem.product.price * lineItem.quantity) | price }}</td>
            <td>
              <div
                class="lni lni-cross-circle item-delete"
                @click="removeLineItem(lineItem.product.id)"
              ></div>
            </td>
          </tr>
        </table>
      </div>
    </div>

    <div class="invoice-step" v-if="invoiceStep === 3">
      <h2>Step 3: Review and Submit</h2>
      <solar-button @button:click="submitInvoice">
        Submit Invoice
      </solar-button>
      <hr />
      <div class="invoice-step-detail" id="invoice" ref="invoice">
        <div class="invoice-logo">
          <img
            id="imgLogo"
            alt="Solar Coffee logo"
            src="../assets/images/solarcoffee-logo.png"
          />
          <h3>1337 Solar Lane</h3>
          <h3>San Somewhere, CA 90000</h3>
          <h3>USA</h3>
        </div>
        <div class="invoice-order-list" v-if="lineItems.length">
          <div class="invoice-header">
            <h3>Invoice: {{ new Date() | humanizeDate }}</h3>
            <h3>
              Customer:
              {{
                this.selectedCustomer.firstName +
                " " +
                this.selectedCustomer.lastName
              }}
            </h3>
            <h3>
              Address: {{ this.selectedCustomer.primaryAddress.addressLine1 }}
            </h3>
            <h3 v-if="this.selectedCustomer.primaryAddress.addressLine2">
              {{ this.selectedCustomer.primaryAddress.addressLine2 }}
            </h3>
            <h3>
              {{
                this.selectedCustomer.primaryAddress.city +
                " | " +
                this.selectedCustomer.primaryAddress.state +
                " | " +
                this.selectedCustomer.primaryAddress.postalCode +
                " | " +
                this.selectedCustomer.primaryAddress.country
              }}
            </h3>
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
              <td>
                {{ (lineItem.product.price * lineItem.quantity) | price }}
              </td>
            </tr>
            <tr>
              <th colspan="4"></th>
              <th>Grand Total</th>
            </tr>
            <tfoot>
              <tr>
                <td colspan="4" class="due">Balance due upon receipt:</td>
                <td class="price-final">{{ runningTotal | price }}</td>
              </tr>
            </tfoot>
          </table>
        </div>
      </div>
    </div>

    <hr />
    <div class="invoice-step-actions">
      <solar-button @button:click="prev" :disabled="!canGoPrev"
        >Previous</solar-button
      >
      <solar-button
        v-if="invoiceStep === 1"
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
import { jsPDF } from "jspdf";
import html2canvas from "html2canvas";

const customerService = new CustomerService();
const inventoryService = new InventoryService();
const invoiceService = new InvoiceService();

@Component({
  name: "CreateInvoice",
  components: { SolarButton },
})
export default class CreateInvoice extends Vue {
  $refs!: {
    invoice: HTMLElement;
  };

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
  newItem = {} as ILineItem;

  addLineItem(): void {
    let newItem: ILineItem = {
      product: this.newItem.product,
      quantity: Number(this.newItem.quantity),
    };

    let existingItems = this.lineItems.map((item) => item.product.id);

    if (existingItems.includes(newItem.product.id)) {
      let lineItem = this.lineItems.find(
        (item) => item.product.id === newItem.product.id
      );
      if (lineItem !== undefined) {
        lineItem.quantity = Number(lineItem.quantity) + newItem.quantity;
      }
    } else {
      this.lineItems.push(this.newItem);
    }

    this.newItem = {} as ILineItem;
  }

  finalizeOrder(): void {
    this.invoiceStep = 3;
  }

  async submitInvoice(): Promise<void> {
    this.invoice = {
      customerId: this.selectedCustomerId,
      lineItems: this.lineItems,
      createdOn: new Date(),
      updatedOn: new Date(),
    };

    await invoiceService.makeNewInvoice(this.invoice);

    this.downloadPdf();
    await this.$router.push("/orders");
  }

  downloadPdf(): void {
    let pdf = new jsPDF("p", "pt", "a4", true);
    let invoice = document.getElementById("invoice");

    if (invoice === null) {
      console.log(":: invoice is null :: in downloadPFD()");
      return;
    }

    let width = this.$refs.invoice.clientWidth;
    let height = this.$refs.invoice.clientHeight;

    html2canvas(invoice).then((canvas) => {
      let image = canvas.toDataURL("image/png");
      pdf.addImage(image, "PNG", 0, 0, width * 0.4, height * 0.4);
      pdf.save("invoice");
    });
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

  get selectedCustomer(): ICustomer | undefined {
    return this.customers.find((c) => c.id === this.selectedCustomerId);
  }

  startOver(): void {
    this.selectedCustomerId = 0;
    this.lineItems = [];
    this.invoice = {
      createdOn: new Date(),
      updatedOn: new Date(),
      customerId: 0,
      lineItems: [],
    };
    this.invoiceStep = 1;
  }

  removeLineItem(prodId: number): void {
    this.lineItems = this.lineItems.filter((el) => el.product.id !== prodId);
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
  text-align: center;
}

.invoice-logo {
  padding-top: 1.4rem;
  text-align: center;

  img {
    width: 120px;
  }
}

.item-delete {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.5rem;
  color: $solar-red;
}

.item-quantity {
  text-align: center;
  wrap-option: 0;
}

.item-quantity-icons {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.1rem;
  padding: 2px;
  color: $solar-blue;
}
</style>
