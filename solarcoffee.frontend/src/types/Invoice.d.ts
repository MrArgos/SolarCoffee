import { IProduct } from "@/types/Product";
import { ICustomer } from "@/types/Customer";

interface ILineItem {
  product: IProduct;
  quantity: number;
}

export interface IInvoice {
  customerId: number;
  lineItems: ILineItem[];
  createdOn: Date;
  updatedOn: Date;
}
