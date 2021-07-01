import { IProduct } from "@/types/Product";

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
