import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

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
}
