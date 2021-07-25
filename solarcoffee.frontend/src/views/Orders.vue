<template>
  <div id="ordersContainer">
    <h1 id="ordersTitle">Sales Orders</h1>
    <hr />
    <table id="sales-orders" class="table" v-if="orders.length">
      <thead>
        <tr>
          <th>Customer</th>
          <th>Order No.</th>
          <th>Date</th>
          <th>Order Total</th>
          <th>Order Status</th>
          <th>Mark Complete</th>
        </tr>
        <tr v-for="order in orders" :key="order.id">
          <td>
            {{ order.customer.firstName + " " + order.customer.lastName }}
          </td>
          <td>{{ order.id }}</td>
          <td>{{ order.createdOn | humanizeDate }}</td>
          <td>{{ getTotal(order) | price }}</td>
          <td class="status-text"
            v-bind:class="{ paid: order.isPaid }">{{ getStatus(order) }}</td>
          <td>
            <div
              v-if="!order.isPaid"
              class="lni lni-coin pay-order"
              @click="completeOrder(order.id)"
            ></div>
          </td>
        </tr>
      </thead>
    </table>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import SolarButton from "@/components/SolarButton.vue";
import { OrderService } from "@/services/order-service";
import { ISalesOrder } from "@/types/SalesOrder";

const orderService = new OrderService();

@Component({
  name: "Orders",
  components: { SolarButton },
})
export default class Orders extends Vue {
  orders: ISalesOrder[] = [];

  getTotal(order: ISalesOrder): number {
    return order.salesOrderItems.reduce(
      (a, b) => a + b["product"]["price"] * b["quantity"],
      0
    );
  }

  getStatus(order: ISalesOrder): string {
    return order.isPaid ? "Paid in full" : "Unpaid";
  }

  async completeOrder(id: number): Promise<void> {
    await orderService.markOrderComplete(id);
    await this.fetchData();
  }

  async fetchData(): Promise<void> {
    this.orders = await orderService.getOrders();
  }

  async created(): Promise<void> {
    await this.fetchData();
  }
}
</script>

<style scoped lang="scss">
@import "src/scss/global.scss";

/*.green {
  font-weight: bold;
  color: $solar-green;
}*/

.pay-order {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.5rem;
  color: $solar-green;
}

.inventory-actions {
  display: flex;
  margin-bottom: 0.8rem;
}

.status-text{
  font-weight: bold;
  color: $solar-red;
}

.paid {
  color: $solar-green;
}
</style>
