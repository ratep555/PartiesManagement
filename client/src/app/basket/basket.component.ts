import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Basket, BasketItem } from '../shared/models/basket';
import { WebshopService } from '../webshop/webshop.service';
import { BasketService } from './basket.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent implements OnInit {
  basket$: Observable<Basket>;

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
  }

  removingItemFromBasket(basketItem: BasketItem) {
    this.basketService.removingItemFromBasket(basketItem);
  }

  increaseBasketItemQuantity(basketItem: BasketItem) {
    this.basketService.increaseBasketItemQuantity(basketItem);
  }

  decreaseBasketItemQuantity(basketItem: BasketItem) {
    this.basketService.decreaseBasketItemQuantity(basketItem);
  }
}
