import axios from "axios";
import { IServiceResponse } from "@/types/ServiceResponse";
import { ISalesOrder } from "@/types/SalesOrder";

/**
 * Order Service
 * Provides UI business logic associated with sales orders.
 */
export class OrderService {
  API_URL = process.env.VUE_APP_API_URL;

  async getOrders(): Promise<ISalesOrder[]> {
    return axios.get(`${this.API_URL}/order/`).then((res) => res.data);
  }

  async markOrderComplete(id: number): Promise<IServiceResponse<boolean>> {
    return axios
      .patch(`${this.API_URL}/order/complete/${id}`)
      .then((res) => res.data);
  }
}
