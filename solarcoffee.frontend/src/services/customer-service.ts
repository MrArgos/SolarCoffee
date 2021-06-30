import axios from "axios";
import { ICustomer } from "@/types/Customer";
import { IServiceResponse } from "@/types/ServiceResponse";

/**
 * Customer Service
 * Provides UI business logic associated with Customers in our system
 */
export class CustomerService {
  API_URL = process.env.VUE_APP_API_URL;

  async getAllCustomers(): Promise<ICustomer[]> {
    return axios
      .get(`${this.API_URL}/customer/`)
      .then((response) => response.data);
  }

  async getCustomerById(id: number): Promise<ICustomer> {
    return axios
      .get(`${this.API_URL}/customer/${id}`)
      .then((response) => response.data);
  }

  async saveCustomer(
    customer: ICustomer
  ): Promise<IServiceResponse<ICustomer>> {
    return axios
      .post(`${this.API_URL}/customer/`, customer)
      .then((response) => response.data);
  }

  async deleteCustomer(id: number): Promise<IServiceResponse<boolean>> {
    return axios
      .delete(`${this.API_URL}/customer/${id}`)
      .then((response) => response.data);
  }
}
