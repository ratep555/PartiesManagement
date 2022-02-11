import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { WebshopService } from 'src/app/webshop/webshop.service';
import { Basket, BasketItem } from '../../models/basket';
import { Item } from '../../models/item';

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

  constructor(private basketService: BasketService,
              private webshopservice: WebshopService) { }

  ngOnInit() {
    this.basket$ = this.basketService.basket$;
  }

  decreaseBasketItemQuantity(item: BasketItem) {
    this.webshopservice.increaseStockQuantity1(item.id, 1).subscribe(() => {
    });
    this.decrease.emit(item);
  }

  increaseBasketItemQuantity(item: BasketItem) {
    if (item.stockQuantity > 2) {
      this.webshopservice.decreaseStockQuantity1(item.id, 1).subscribe(() => {
      });
      this.increase.emit(item);
    }
  }

  removingItemFromBasket(item: BasketItem) {
    this.webshopservice.increaseStockQuantity1(item.id, item.quantity).subscribe(() => {
    });
    this.remove.emit(item);
  }

}





