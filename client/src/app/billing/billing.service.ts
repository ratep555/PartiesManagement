import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { OrderClient } from '../shared/models/order';
import { ShippingOption } from '../shared/models/shippingOption';

@Injectable({
  providedIn: 'root'
})
export class BillingService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  creatingOrder(order: OrderClient) {
    return this.http.post(this.baseUrl + 'orders/oki', order);
  }

  getShippingOptions() {
    return this.http.get<ShippingOption[]>(this.baseUrl + 'orders/shippingoptions');
  }
}
