<template>
  <div class="customers-container">
    <h1 id="customersTitle">Manage Customers</h1>
    <hr />

    <div class="customers-actions">
      <solar-button @button:click="showNewCustomerModal" id="addNewBtn">
        Add New Customer
      </solar-button>
    </div>

    <table id="customersTable" class="table">
      <tr>
        <th>Customer</th>
        <th>Address</th>
        <th>City</th>
        <th>Country</th>
        <th>Since</th>
        <th>Delete</th>
      </tr>

      <tr v-for="customer in customers" :key="customer.id">
        <td>{{ customer.firstName + " " + customer.lastName }}</td>
        <td>
          {{ customer.primaryAddress.addressLine1 }},
          {{ customer.primaryAddress.addressLine2 }}
        </td>
        <td>{{ customer.primaryAddress.city }}</td>
        <td>{{ customer.primaryAddress.country }}</td>
        <td>{{ customer.createdOn | humanizeDate }}</td>
        <td>
          <div
            class="lni lni-cross-circle customer-delete"
            @click="deleteCustomer(customer.id)"
          ></div>
        </td>
      </tr>
    </table>
    <new-customer-modal
      v-if="isNewCustomerVisible"
      @save:customer="saveNewCustomer"
      @close="closeModals"
    >
    </new-customer-modal>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { ICustomer } from "@/types/Customer";
import SolarButton from "@/components/SolarButton.vue";
import NewCustomerModal from "@/components/Modals/NewCustomerModal.vue";
import { CustomerService } from "@/services/customer-service";

const customerService = new CustomerService();

@Component({
  name: "Customers",
  components: { SolarButton, NewCustomerModal },
})
export default class Customers extends Vue {
  customers: ICustomer[] = [];
  private isNewCustomerVisible = false;

  async created(): Promise<void> {
    await this.fetchData();
  }

  async fetchData(): Promise<void> {
    this.customers = await customerService.getAllCustomers();
  }

  async deleteCustomer(id: number): Promise<void> {
    await customerService.deleteCustomer(id);
    await this.fetchData();
  }

  showNewCustomerModal(): void {
    this.isNewCustomerVisible = true;
  }

  async saveNewCustomer(customer: ICustomer): Promise<void> {
    await customerService.saveCustomer(customer);
    this.isNewCustomerVisible = false;
    await this.fetchData();
  }

  closeModals(): void {
    this.isNewCustomerVisible = false;
  }
}
</script>

<style scoped lang="scss">
@import "src/scss/global";

.customers-actions {
  display: flex;
  margin-bottom: 0.8rem;
}

.customer-delete {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.5rem;
  color: $solar-red;
}
</style>
