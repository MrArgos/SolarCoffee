import axios from "axios";
import { IInvoice } from "@/types/Invoice";
import { IServiceResponse } from "@/types/ServiceResponse";

export default class InvoiceService {
  API_URL = process.env.VUE_APP_API_URL;

  async makeNewInvoice(invoice: IInvoice): Promise<IServiceResponse<boolean>> {
    const now = new Date();
    invoice.createdOn = now;
    invoice.updatedOn = now;

    return axios
      .post(`${this.API_URL}/invoice/`, invoice)
      .then((response) => response.data);
  }
}
