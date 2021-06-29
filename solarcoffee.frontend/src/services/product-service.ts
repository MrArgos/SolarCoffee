import axios from "axios";
import { IProduct } from "@/types/Product";

export class ProductService {
  API_URL = process.env.VUE_APP_API_URL;

  async archiveProduct(id: number): Promise<number | boolean> {
    return axios
      .patch(`${this.API_URL}/product/${id}`)
      .then((response) => response.status);
  }

  async CreateProduct(product: IProduct): Promise<number | boolean> {
    return axios
      .post(`${this.API_URL}/product/`, product)
      .then((response) => response.status);
  }
}
