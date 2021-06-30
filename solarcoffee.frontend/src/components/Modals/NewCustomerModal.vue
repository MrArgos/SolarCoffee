<template>
  <solar-modal>
    <template v-slot:header> Add New Customer </template>

    <template v-slot:body>
      <h3>Customer Information</h3>
      <hr />
      <ul class="newCustomer">
        <li>
          <label for="firstName">First Name</label>
          <input type="text" id="firstName" v-model="newCustomer.firstName" />
        </li>

        <li>
          <label for="lastName">Last Name</label>
          <input type="text" id="lastName" v-model="newCustomer.lastName" />
        </li>

        <h3>Address</h3>
        <hr />

        <li>
          <label for="addressLine1">Address Line 1</label>
          <input
            type="text"
            id="addressLine1"
            v-model="newCustomerAddress.addressLine1"
          />
        </li>

        <li>
          <label for="addressLine2">Address Line 2</label>
          <input
            type="text"
            id="addressLine2"
            v-model="newCustomerAddress.addressLine2"
          />
        </li>

        <li>
          <label for="country">Country</label>
          <input
            type="text"
            id="country"
            v-model="newCustomerAddress.country"
          />
        </li>

        <li>
          <label for="state">State</label>
          <input type="text" id="state" v-model="newCustomerAddress.state" />
        </li>

        <li>
          <label for="city">City</label>
          <input type="text" id="city" v-model="newCustomerAddress.city" />
        </li>

        <li>
          <label for="postalCode">Postal Code</label>
          <input
            type="text"
            id="postalCode"
            v-model="newCustomerAddress.postalCode"
          />
        </li>
      </ul>
    </template>

    <template v-slot:footer>
      <solar-button
        type="button"
        @click.native="save"
        aria-label="save new customer"
      >
        Save Customer
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
import { Component, Vue } from "vue-property-decorator";
import SolarModal from "@/components/Modals/SolarModal.vue";
import SolarButton from "@/components/SolarButton.vue";
import { ICustomer, ICustomerAddress } from "@/types/Customer";

@Component({
  name: "NewCustomerModal",
  components: { SolarButton, SolarModal },
})
export default class NewCustomerModal extends Vue {
  newCustomerAddress: ICustomerAddress = {
    id: 0,
    createdOn: new Date(),
    updatedOn: new Date(),
    addressLine1: "",
    addressLine2: "",
    city: "",
    state: "",
    postalCode: "",
    country: "",
  };

  newCustomer: ICustomer = {
    id: 0,
    firstName: "",
    lastName: "",
    createdOn: new Date(),
    updatedOn: new Date(),
    primaryAddress: this.newCustomerAddress,
  };

  close(): void {
    this.$emit("close");
  }

  save(): void {
    console.log("Customer on Modal:", this.newCustomer);
    this.$emit("save:customer", this.newCustomer);
  }
}
</script>

<style scoped lang="scss">
@import "src/scss/global.scss";
.newCustomer {
  list-style: none;
  padding: 0;
  margin: 0;
}
</style>
