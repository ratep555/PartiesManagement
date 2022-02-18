import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { OrderStatus1 } from '../shared/models/orderstatus';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getOrdersForCustomer() {
    return this.http.get(this.baseUrl + 'orders');
  }

  getOrderInfoForCustomer(id: number) {
    return this.http.get(this.baseUrl + 'orders/' + id);
  }

  getOrderStatuses() {
    return this.http.get<OrderStatus1[]>(this.baseUrl + 'orders/orderstatuses');
  }

  updateOrder(id: number, params: any) {
    return this.http.put(this.baseUrl + 'orders/' + id, params);
  }

}
