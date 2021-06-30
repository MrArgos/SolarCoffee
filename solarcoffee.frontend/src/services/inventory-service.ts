import axios from "axios";
import { IProductInventory } from "@/types/Product";
import { IShipment } from "@/types/Shipment";

/**
 * Inventory Service
 * Provides UI business logic associated with product inventory
 */
export class InventoryService {
  API_URL = process.env.VUE_APP_API_URL;

  public async getInventory(): Promise<IProductInventory[]> {
    return axios
      .get(`${this.API_URL}/inventory/`)
      .then((response) => response.data);
  }

  public async updateInventoryQuantity(
    shipment: IShipment
  ): Promise<IProductInventory> {
    return axios
      .patch(`${this.API_URL}/inventory/`, shipment)
      .then((response) => response.data);
  }
}
