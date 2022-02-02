import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { Basket, BasketItem } from '../../models/basket';

@Component({
  selector: 'app-basket-review',
  templateUrl: './basket-review.component.html',
  styleUrls: ['./basket-review.component.css']
})
export class BasketReviewComponent implements OnInit {
  basket$: Observable<Basket>;
  @Output() decrease: EventEmitter<BasketItem> = new EventEmitter<BasketItem>();
  @Output() increase: EventEmitter<BasketItem> = new EventEmitter<BasketItem>();
  @Output() remove: EventEmitter<BasketItem> = new EventEmitter<BasketItem>();
  @Input() isBasket = true;
 // @Input() items: BasketItem[] | OrderItem[] = [];
  @Input() isOrder = false;

  constructor(private basketService: BasketService) { }

  ngOnInit() {
    this.basket$ = this.basketService.basket$;
  }

  decreaseBasketItemQuantity(item: BasketItem) {
    this.decrease.emit(item);
  }

  increaseBasketItemQuantity(item: BasketItem) {
    this.increase.emit(item);
  }

  removingItemFromBasket(item: BasketItem) {
    this.remove.emit(item);
  }

}





