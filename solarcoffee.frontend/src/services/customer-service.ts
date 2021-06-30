import axios from "axios";
import { ICustomer } from "@/types/Customer";

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

  async saveCustomer(customer: ICustomer): Promise<any> {
    console.log("Customer on Service: ",customer);
    return axios
      .post(`${this.API_URL}/customer/`, customer)
      .then((response) => response.status);
  }

  async deleteCustomer(id: number): Promise<any> {
    return axios
      .delete(`${this.API_URL}/customer/${id}`)
      .then((response) => response.data);
  }
}
