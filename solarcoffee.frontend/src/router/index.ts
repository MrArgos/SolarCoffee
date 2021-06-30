import Inventory from "@/views/Inventory.vue";
import Vue from "vue";
import VueRouter, { RouteConfig } from "vue-router";
import Customers from "@/views/Customers.vue";

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
  {
    path: "/",
    name: "home",
    component: Inventory,
  },
  {
    path: "/inventory",
    name: "inventory",
    component: Inventory,
  },
  {
    path: "/customers",
    name: "customers",
    component: Customers,
  },
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes,
});

export default router;
