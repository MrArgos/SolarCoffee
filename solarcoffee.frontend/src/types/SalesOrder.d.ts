import { ICustomer } from "@/types/Customer";
import { ILineItem } from "@/types/Invoice";

export interface ISalesOrder {
  id: number;
  createdOn: Date;
  updatedOn?: Date;
  customer: ICustomer;
  salesOrderItems: ILineItem[];
  isPaid: boolean;
}
