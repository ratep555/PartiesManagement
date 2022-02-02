import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { OrderClient } from '../shared/models/order';
import { PayingOption } from '../shared/models/payingOption';
import { ShippingOption } from '../shared/models/shippingOption';

@Injectable({
  providedIn: 'root'
})
export class Billing1Service {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  creatingOrder(order: OrderClient) {
    return this.http.post(this.baseUrl + 'orders/okimail', order);
  }

  creatingOrderForStripe(order: OrderClient) {
    return this.http.post(this.baseUrl + 'orders/okimail1', order);
  }

  getShippingOptions() {
    return this.http.get<ShippingOption[]>(this.baseUrl + 'orders/shippingoptions');
  }

  getPayingOptions() {
    return this.http.get<PayingOption[]>(this.baseUrl + 'orders/payingoptions');
  }

  getStripePayingOption() {
    return this.http.get<PayingOption>(this.baseUrl + 'orders/stripepay');
  }
}










